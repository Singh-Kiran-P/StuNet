using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ChatSample.Hubs;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotificationRepository<QuestionNotification> _notificationRepository;
        private readonly ICourseSubscriptionRepository _courseSubscriptionRepository;
        private readonly IQuestionSubscriptionRepository _questionSubscriptionRepository;

        public QuestionController(IQuestionRepository questionRepository, ITopicRepository topicRepository, ICourseRepository courseRepository, UserManager<User> userManager, IHubContext<ChatHub> hubContext, INotificationRepository<QuestionNotification> notificationRepository, ICourseSubscriptionRepository subscriptionRepository, IQuestionSubscriptionRepository questionSubscriptionRepository)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
            _userManager = userManager;
            _hubContext = hubContext;
            _courseSubscriptionRepository = subscriptionRepository;
            _notificationRepository = notificationRepository;
            _questionSubscriptionRepository = questionSubscriptionRepository;
        }

        // private static questionDto toDto(Question question, User user)
        // {
        //     return new questionDto
        //     {
        //         id = question.id,
        //         user = question.user,
        //         course = new getOnlyCourseDto
        //         {
        //             id = question.course.id,
        //             name = question.course.name,
        //             number = question.course.number,
        //         },
        //         title = question.title,
        //         body = question.body,
        //         topics = question.topics.Select(topic => new getOnlyTopicDto
        //         {
        //             id = topic.id,
        //             name = topic.name
        //         }).ToList(),
        //         time = question.time
        //     };
        // }

        //[Authorize(Roles = "student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<questionDto>>> GetQuestions()
        {
            // var questions = await _questionRepository.getAllAsync();
            // return Ok(questions.Select(question => questionDto.convert(question)));
            try
            {
                var questions = await _questionRepository.getAllAsync();
                List<questionDto> res = new List<questionDto>();
                foreach (var q in questions)
                {
                    User user = await _userManager.FindByIdAsync(q.userId);
                    res.Add(questionDto.convert(q, user));
                }
                return Ok(res);
            }
            catch { return BadRequest("Error finding all questions"); }
        }

        [HttpGet("subscribed")]
        public async Task<ActionResult<IEnumerable<questionDto>>> getSubscribedQuestions()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "userref"))
            {
                string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
                IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.getByUserId(userId);
                IEnumerable<int> subscribedQuestionIds = subscriptions.Select(sub => sub.questionId);
                IEnumerable<Question> subscribedQuestions = subscribedQuestionIds.Select(id => _questionRepository.getAsync(id))
                                                                                    .Select(task => task.Result);

                User user = await _userManager.FindByIdAsync(userId);
                return Ok(subscribedQuestions.Select(q => questionDto.convert(q, user)));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<questionDto>> GetQuestion(int id)
        {
            // var question = await _questionRepository.getAsync(id);
            // if (question == null)
            //     return NotFound();
            // return Ok(toDto(question));
            try
            {
                var question = await _questionRepository.getAsync(id);
                if (question == null)
                    return NotFound();
                User user = await _userManager.FindByIdAsync(question.userId);

                return Ok(questionDto.convert(question, user));
            }
            catch { return BadRequest("Error finding question"); }
        }

        [HttpGet("GetQuestionsByCourseId/search/{courseId}")]
        public async Task<ActionResult<GetCourseDto>> searchByName(int courseId, [FromQuery] string name)
        {
            var questions = await _questionRepository.getByCourseIdAsync(courseId);
            IEnumerable<Question> matches = StringMatcher.FuzzyMatchObject(questions, name);
            List<questionDto> res = new List<questionDto>();
            foreach (var q in matches)
            {
                User user = await _userManager.FindByIdAsync(q.userId);
                res.Add(questionDto.convert(q, user));
            }
            return Ok(res);
        }

        //[Authorize(Roles = "student")]
        [HttpPost]
        public async Task<ActionResult<questionDto>> CreateQuestion(createQuestionDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                User user = await _userManager.FindByEmailAsync(userEmail);
                Course c = _courseRepository.getAsync(dto.courseId).Result;

                ICollection<Topic> topics = new List<Topic>();
                topics = dto.topicIds.Select(id => _topicRepository.getAsync(id)) //TODO: Dit is een probleem als 1 van de topics niet bestaat, er wordt niet null teruggegeven maar een lijst met een null in en dit gaat niet in de db; voorlopige oplossing zie lijn 78
                                                .Select(task => task.Result)
                                                .ToList();

                if (c == null) { return BadRequest("Course does not exist"); }
                if (topics.Contains(null)) { return BadRequest("One of the topics does not exist"); }
                Question question = new()
                {
                    title = dto.title,
                    userId = user.Id, //TODO
                    course = c,
                    body = dto.body,
                    // files = createQuestionDto.files TODO
                    topics = topics,
                    time = DateTime.UtcNow
                };

                await _questionRepository.createAsync(question);

                await _questionSubscriptionRepository.createAsync(new QuestionSubscription
                {
                    userId = user.Id,
                    questionId = question.id,
                    dateTime = question.time
                });

                await _hubContext.Groups.AddToGroupAsync(UserHandler.ConnectedIds[user.Id], "Question " + question.id.ToString());

                IEnumerable<string> subscriberIds = (await _courseSubscriptionRepository.getByCourseId(c.id)).Select(sub => sub.userId);
                await _notificationRepository.createAllAync(subscriberIds.Select(userId => new QuestionNotification
                {
                    userId = userId,
                    questionId = question.id,
                    question = question,
                    time = question.time
                }));

                var ret = questionDto.convert(question, user);
                await _hubContext.Clients.Group("Course " + c.id).SendAsync("QuestionNotification", ret);
                return Ok(ret);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            var existingQuestion = await _questionRepository.getAsync(id);
            if (existingQuestion is null)
            {
                return NotFound();
            }

            await _questionRepository.deleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, createQuestionDto dto)
        {

            var existingQuestion = await _questionRepository.getAsync(id);
            if (existingQuestion is null)
            {
                return NotFound();
            }

            Question updatedQuestion = new()
            {
                title = dto.title,
                // user = updateQuestionDto.user,
                course = _courseRepository.getAsync(dto.courseId).Result,
                body = dto.body,
                // files = updateQuestionDto.files
                topics = dto.topicIds.Select(id => _topicRepository.getAsync(id))
                                                .Select(task => task.Result)
                                                .ToList(),
                time = DateTime.UtcNow
            };

            await _questionRepository.updateAsync(updatedQuestion);
            return NoContent();
        }
    }
}
