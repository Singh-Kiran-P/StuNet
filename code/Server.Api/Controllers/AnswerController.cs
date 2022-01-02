using System;
using System.Linq;
using Server.Api.Dtos;
using ChatSample.Hubs;
using Server.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly INotificationRepository<AnswerNotification> _notificationRepository;
        private readonly ISubscriptionRepository<QuestionSubscription> _subscriptionRepository;

        public AnswerController(IAnswerRepository answerRepository, UserManager<User> userManager, IQuestionRepository questionRepository, IHubContext<ChatHub> hubContext, INotificationRepository<AnswerNotification> notificationRepository, ISubscriptionRepository<QuestionSubscription> subscriptionRepository)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _notificationRepository = notificationRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAnswerDto>>> GetAnswers()
        {
            IEnumerable<Answer> answers = await _answerRepository.GetAllAsync();
            List<GetAnswerDto> res = new List<GetAnswerDto>();
            foreach (var answer in answers) {
                User user = await _userManager.FindByIdAsync(answer.userId);
                res.Add(GetAnswerDto.Convert(answer, user));
            }
            return Ok(res);
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("GetAnswersByQuestionId/{questionId}")]
        public async Task<ActionResult<IEnumerable<GetAnswerDto>>> GetAnswersByQuestionId(int questionId)
        {
            IEnumerable<Answer> answers = await _answerRepository.GetByQuestionId(questionId);
            List<GetAnswerDto> res = new List<GetAnswerDto>();
            foreach (var answer in answers) {
                User user = await _userManager.FindByIdAsync(answer.userId);
                res.Add(GetAnswerDto.Convert(answer, user));
            }
            return Ok(res);
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAnswerDto>> GetAnswer(int id)
        {
            var answer = await _answerRepository.GetAsync(id);
            User user = await _userManager.FindByIdAsync(answer.userId);
            if (answer == null) return NotFound();
            return Ok(GetAnswerDto.Convert(answer, user));
        }

        [HttpGet("getGivenAnswersByEmail")]
        public async Task<ActionResult<IEnumerable<GetAnswerDto>>> GetGivenAnswers(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Ok(new List<GetAnswerDto>());
            var answers = await _answerRepository.GetGivenByUserId(user.Id);
            return Ok(answers.Select(q => GetAnswerDto.Convert(q, user)));
        }

        [Authorize(Roles = "student,prof")]
        [HttpPost]
        public async Task<ActionResult<GetAnswerDto>> CreateAnswer(CreateAnswerDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "username")) return Unauthorized();
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);
            Question question = await _questionRepository.GetAsync(dto.questionId);
            if (user == null || question == null) NotFound();
            Answer answer = new() {
                body = dto.body,
                userId = user.Id,
                title = dto.title,
                question = question,
                time = DateTime.UtcNow
            };

            await _answerRepository.CreateAsync(answer);
            IEnumerable<string> subscriberIds = (await _subscriptionRepository.GetBySubscribedId(question.id)).Select(sub => sub.userId);
            await _notificationRepository.CreateAllAync(subscriberIds.Select(userId => new AnswerNotification {
                userId = userId,
                answer = answer,
                time = answer.time,
                answerId = answer.id
            }));

            var ret = GetAnswerDto.Convert(answer, user);
            await _hubContext.Clients.Group("Question " + question.id).SendAsync("AnswerNotification", ret);
            return Ok(ret);
        }

        [Authorize(Roles = "prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
            var existing = _answerRepository.GetAsync(id);
            if (existing == null) return NotFound();
            await _answerRepository.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAnswer(int id, CreateAnswerDto dto)
        {
            Answer existingAnswer = await _answerRepository.GetAsync(id);
            Question question = existingAnswer.question;
            if (existingAnswer == null || question == null) return NotFound();
            Answer updatedAnswer = new() {
                body = dto.body,
                title = dto.title,
                question = question,
                time = DateTime.UtcNow,
                id = existingAnswer.id,
                userId = existingAnswer.userId,
                isAccepted = existingAnswer.isAccepted
            };

            await _answerRepository.UpdateAsync(updatedAnswer);
            return NoContent();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("SetAccepted/{id}")]
        public async Task<ActionResult> SetAnswerAccepted(int id, bool accepted)
        {
            Answer existingAnswer = await _answerRepository.GetAsync(id);
            if (existingAnswer == null) return NotFound();
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "userref")) return Unauthorized();
            string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
            if (existingAnswer.question.userId != userId) return Unauthorized();
            existingAnswer.isAccepted = accepted;
            await _answerRepository.UpdateAsync(existingAnswer);
            return NoContent();
        }
    }
}
