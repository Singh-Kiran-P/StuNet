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
                User answerUser = await _userManager.FindByIdAsync(answer.userId);
                User questionUser = await _userManager.FindByIdAsync(answer.question.userId);
                res.Add(GetAnswerDto.Convert(answer, answerUser, questionUser));
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
                User answerUser = await _userManager.FindByIdAsync(answer.userId);
                User questionUser = await _userManager.FindByIdAsync(answer.question.userId);
                if (answerUser == null || questionUser == null) return NotFound();
                res.Add(GetAnswerDto.Convert(answer, answerUser, questionUser));
            }
            return Ok(res);
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAnswerDto>> GetAnswer(int id)
        {
            var answer = await _answerRepository.GetAsync(id);
            User answerUser = await _userManager.FindByIdAsync(answer.userId);
            User questionUser = await _userManager.FindByIdAsync(answer.question.userId);
            if (answer == null || answerUser == null || questionUser == null) return NotFound();
            return Ok(GetAnswerDto.Convert(answer, answerUser, questionUser));
        }

        [HttpGet("getGivenAnswersByEmail")]
        public async Task<ActionResult<IEnumerable<GetAnswerDto>>> GetGivenAnswers(string email)
        {
            User answerUser = await _userManager.FindByEmailAsync(email);
            if (answerUser == null) return Ok(new List<GetAnswerDto>());
            var answers = await _answerRepository.GetGivenByUserId(answerUser.Id);
            List<GetAnswerDto> res = new List<GetAnswerDto>();
            foreach (var answer in answers) {
                User questionUser = await _userManager.FindByIdAsync(answer.question.userId);
                if (questionUser == null) return NotFound();
                res.Add(GetAnswerDto.Convert(answer, answerUser, questionUser));
            }
            return Ok(res);
        }

        [Authorize(Roles = "student,prof")]
        [HttpPost]
        public async Task<ActionResult<GetAnswerDto>> CreateAnswer(CreateAnswerDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "userref")) return Unauthorized();
            string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
            Question question = await _questionRepository.GetAsync(dto.questionId);
            User answerUser = await _userManager.FindByIdAsync(userId);
            User questionUser = await _userManager.FindByIdAsync(question.userId);
            if (question == null || answerUser == null || questionUser == null) NotFound();
            Answer answer = new() {
                body = dto.body,
                title = dto.title,
                question = question,
                time = DateTime.UtcNow,
                userId = answerUser.Id,
                isAccepted = currentUser.IsInRole("prof")
            };

            await _answerRepository.CreateAsync(answer);
            IEnumerable<string> subscriberIds = (await _subscriptionRepository.GetBySubscribedId(question.id)).Select(sub => sub.userId);
            await _notificationRepository.CreateAllAync(subscriberIds.Select(userId => new AnswerNotification {
                userId = userId,
                answer = answer,
                time = answer.time,
                answerId = answer.id
            }));

            var ret = GetAnswerDto.Convert(answer, answerUser, questionUser);
            await _hubContext.Clients.Group("Question " + question.id).SendAsync("AnswerNotification", ret);
            return Ok(ret);
        }

        [Authorize(Roles = "prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
            var existing = await _answerRepository.GetAsync(id);
            if (existing == null) return NotFound();
            await _answerRepository.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAnswer(int id, CreateAnswerDto dto)
        {
            var existing = await _answerRepository.GetAsync(id);
            if (existing == null) return NotFound();
            Answer updatedAnswer = new() {
                body = dto.body,
                id = existing.id,
                title = dto.title,
                time = DateTime.UtcNow,
                userId = existing.userId,
                question = existing.question,
                isAccepted = existing.isAccepted
            };

            await _answerRepository.UpdateAsync(updatedAnswer);
            return NoContent();
        }

        [Authorize(Roles = "student,prof")]
        [HttpPut("SetAccepted/{id}")]
        public async Task<ActionResult> SetAnswerAccepted(int id, bool accepted)
        {
            Answer existing = await _answerRepository.GetAsync(id);
            if (existing == null) return NotFound();
            existing.isAccepted = accepted;
            await _answerRepository.UpdateAsync(existing);
            return NoContent();
        }
    }
}
