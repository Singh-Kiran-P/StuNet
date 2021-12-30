using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Services;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ISubscriptionRepository<CourseSubscription> _courseSubscriptionRepository;
        private readonly ISubscriptionRepository<QuestionSubscription> _questionSubscriptionRepository;

        public QuestionController(IQuestionRepository questionRepository, ITopicRepository topicRepository, ICourseRepository courseRepository, UserManager<User> userManager, IHubContext<ChatHub> hubContext, INotificationRepository<QuestionNotification> notificationRepository, ISubscriptionRepository<CourseSubscription> subscriptionRepository, ISubscriptionRepository<QuestionSubscription> questionSubscriptionRepository)
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

        // FIXME: I removed commented questionDto of 3 weeks old here

        //[Authorize(Roles = "student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetQuestionDto>>> GetQuestions()
        {
            try
            {
                var questions = await _questionRepository.GetAllAsync();
                List<GetQuestionDto> res = new List<GetQuestionDto>();
                foreach (var q in questions)
                {
                    User user = await _userManager.FindByIdAsync(q.userId);
                    res.Add(GetQuestionDto.Convert(q, user));
                }
                return Ok(res);
            }
            catch
            {
                return BadRequest("Error finding all questions");
            }
        }

        [HttpGet("subscribed")]
        public async Task<ActionResult<IEnumerable<GetQuestionDto>>> GetSubscribedQuestions()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "userref"))
            {
                string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
                IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.GetByUserId(userId);
                IEnumerable<int> subscribedQuestionIds = subscriptions.Select(sub => sub.subscribedItemId);
                IEnumerable<Question> subscribedQuestions = subscribedQuestionIds.Select(id => _questionRepository.GetAsync(id))
                                                                                    .Select(task => task.Result);

                User user = await _userManager.FindByIdAsync(userId);
                return Ok(subscribedQuestions.Select(q => GetQuestionDto.Convert(q, user)));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetQuestionDto>> GetQuestion(int id)
        {
            try
            {
                var question = await _questionRepository.GetAsync(id);
                if (question == null)
                {
                    return NotFound();
                }
                User user = await _userManager.FindByIdAsync(question.userId);

                return Ok(GetQuestionDto.Convert(question, user));
            }
            catch
            {
                return BadRequest("Error finding question");
            }
        }

        [HttpGet("GetQuestionsByCourseId/search/{courseId}")] //FIXME: Make route lower case
        public async Task<ActionResult<GetCourseDto>> SearchByName(int courseId, [FromQuery] string name)
        {
            var questions = await _questionRepository.GetByCourseIdAsync(courseId);
            IEnumerable<Question> matches = StringMatcher.FuzzyMatchObject(questions, name);
            List<GetQuestionDto> res = new List<GetQuestionDto>();
            foreach (var q in matches)
            {
                User user = await _userManager.FindByIdAsync(q.userId);
                res.Add(GetQuestionDto.Convert(q, user));
            }
            return Ok(res);
        }

        //[Authorize(Roles = "student")]
        [HttpPost]
        public async Task<ActionResult<GetQuestionDto>> CreateQuestion(CreateQuestionDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                User user = await _userManager.FindByEmailAsync(userEmail);
                Course c = _courseRepository.GetAsync(dto.courseId).Result;
                ICollection<Topic> topics = dto.topicIds
                    .Select(id => _topicRepository.GetAsync(id)) //FIXME: Dit is een probleem als 1 van de topics niet bestaat, er wordt niet null teruggegeven maar een lijst met een null in en dit gaat niet in de db; voorlopige oplossing zie lijn 78
                    .Select(task => task.Result)
                    .ToList();

                if (c == null)
                {
                    return BadRequest("Course does not exist");
                }
                else if (topics.Contains(null))
                {
                    return BadRequest("One of the topics does not exist");
                }
                Question question = new()
                {
                    title = dto.title,
                    userId = user.Id, //TODO: do what?
                    course = c,
                    body = dto.body,
                    // files = createQuestionDto.files TODO
                    topics = topics,
                    time = DateTime.UtcNow
                };

                await _questionRepository.CreateAsync(question);

                await _questionSubscriptionRepository.CreateAsync(new QuestionSubscription
                {
                    userId = user.Id,
                    subscribedItemId = question.id,
                    dateTime = question.time
                });

                await _hubContext.Groups.AddToGroupAsync(UserHandler.ConnectedIds[user.Id], "Question " + question.id.ToString());

                IEnumerable<string> subscriberIds = (await _courseSubscriptionRepository.GetBySubscribedId(c.id)).Select(sub => sub.userId);
                await _notificationRepository.CreateAllAync(subscriberIds.Select(userId => new QuestionNotification
                {
                    userId = userId,
                    questionId = question.id,
                    question = question,
                    time = question.time
                }));

                var ret = GetQuestionDto.Convert(question, user);
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
            var existingQuestion = await _questionRepository.GetAsync(id);
            if (existingQuestion is null)
            {
                return NotFound();
            }
            await _questionRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, CreateQuestionDto dto)
        {

            var existingQuestion = await _questionRepository.GetAsync(id);
            if (existingQuestion is null)
            {
                return NotFound();
            }

            Question updatedQuestion = new()
            {
                title = dto.title,
                // user = updateQuestionDto.user,
                course = _courseRepository.GetAsync(dto.courseId).Result,
                body = dto.body,
                // files = updateQuestionDto.files
                topics = dto.topicIds.Select(id => _topicRepository.GetAsync(id))
                                                .Select(task => task.Result)
                                                .ToList(),
                time = DateTime.UtcNow
            };
            await _questionRepository.UpdateAsync(updatedQuestion);
            return NoContent();
        }
    }
}
