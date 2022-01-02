using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using ChatSample.Hubs;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly UserManager<User> _userManager;
        private readonly IQuestionRepository _questionRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotificationRepository<AnswerNotification> _notificationRepository;
        private readonly ISubscriptionRepository<QuestionSubscription> _subscriptionRepository;

        public AnswerController(IAnswerRepository answerRepository, UserManager<User> userManager, IQuestionRepository questionRepository, IHubContext<ChatHub> hubContext, INotificationRepository<AnswerNotification> notificationRepository, ISubscriptionRepository<QuestionSubscription> subscriptionRepository)
        {
            _answerRepository = answerRepository;
            _userManager = userManager;
            _questionRepository = questionRepository;
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        //[Authorize(Roles = "student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAnswerDto>>> GetAnswers()
        {
            try
            {
                IEnumerable<Answer> answers = await _answerRepository.GetAllAsync();
                List<GetAnswerDto> res = new List<GetAnswerDto>();
                foreach (var answer in answers)
                {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    res.Add(GetAnswerDto.Convert(answer, user));
                }
                return Ok(res);
            }
            catch
            {
                return BadRequest("Error finding all answers");
            }
        }

        //[Authorize(Roles = "student,prof")]
        [HttpGet("GetAnswersByQuestionId/{questionId}")]
        public async Task<ActionResult<IEnumerable<GetAnswerDto>>> GetAnswersByQuestionId(int questionId)
        {
            try
            {
                IEnumerable<Answer> answers = await _answerRepository.GetByQuestionId(questionId);
                List<GetAnswerDto> res = new List<GetAnswerDto>();
                foreach (var answer in answers)
                {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    res.Add(GetAnswerDto.Convert(answer, user));
                }
                return Ok(res);
            }
            catch
            {
                return BadRequest("Error finding all answers");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAnswerDto>> GetAnswer(int id)
        {
            try
            {
                var answer = await _answerRepository.GetAsync(id);
                User user = await _userManager.FindByIdAsync(answer.userId);
                if (answer == null)
                {
                    return NotFound();
                }

                return Ok(GetAnswerDto.Convert(answer, user));
            }
            catch
            {
                return BadRequest("Error finding all answers");
            }
        }

        [HttpGet("getGivenAnswersByEmail")]
        public async Task<ActionResult<IEnumerable<GetAnswerDto>>> GetGivenAnswers(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var answers = await _answerRepository.GetGivenByUserId(user.Id);
                return Ok(answers.Select(q => GetAnswerDto.Convert(q, user)));
            }
            else
            {
                return Ok(new List<GetAnswerDto>());
            }
        }

        //[Authorize(Roles = "student")]
        [HttpPost]
        public async Task<ActionResult<GetAnswerDto>> CreateAnswer(CreateAnswerDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                User user = await _userManager.FindByEmailAsync(userEmail);

                Question question = await _questionRepository.GetAsync(dto.questionId);
                if (user == null || question == null) { return BadRequest("User or Question related to answer not found"); }
                Answer answer = new()
                {
                    userId = user.Id,
                    question = question,
                    title = dto.title,
                    body = dto.body,
                    time = DateTime.UtcNow
                };

                await _answerRepository.CreateAsync(answer);

                IEnumerable<string> subscriberIds = (await _subscriptionRepository.GetBySubscribedId(question.id)).Select(sub => sub.userId);
                await _notificationRepository.CreateAllAync(subscriberIds.Select(userId => new AnswerNotification
                {
                    userId = userId,
                    answerId = answer.id,
                    answer = answer,
                    time = answer.time
                }));

                var ret = GetAnswerDto.Convert(answer, user);
                await _hubContext.Clients.Group("Question " + question.id).SendAsync("AnswerNotification", ret);
                return Ok(ret);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize(Roles = "prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
            var existingAnswer = await _answerRepository.GetAsync(id);
            if (existingAnswer is null)
            {
                return NotFound();
            }

            await _answerRepository.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAnswer(int id, CreateAnswerDto dto)
        {

            Answer existingAnswer = await _answerRepository.GetAsync(id);
            User user = await _userManager.FindByIdAsync(dto.userId);
            Question question = existingAnswer.question;
            if (existingAnswer == null || user == null || question == null)
            {
                return NotFound();
            }

            Answer updatedAnswer = new()
            {
                id = existingAnswer.id,
                userId = user.Id,
                question = question,
                title = dto.title,
                body = dto.body,
                time = DateTime.UtcNow,
                isAccepted = existingAnswer.isAccepted
            };

            await _answerRepository.UpdateAsync(updatedAnswer);
            return NoContent();
        }

        // [Authorize(Roles = "prof")]
        [HttpPut("SetAccepted/{id}")]
        public async Task<ActionResult> SetAnswerAccepted(int id, bool accepted)
        {
            Answer existingAnswer = await _answerRepository.GetAsync(id);
            if (existingAnswer == null) 
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "userref"))
            {
                string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
                if (existingAnswer.question.userId == userId)
                {
                    existingAnswer.isAccepted = accepted;
                    await _answerRepository.UpdateAsync(existingAnswer);
                    return NoContent();
                }
            }
            return Unauthorized();
        }
    }
}
