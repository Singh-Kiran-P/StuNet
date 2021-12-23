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
        private readonly IQuestionSubscriptionRepository _subscriptionRepository;

        public AnswerController(IAnswerRepository answerRepository, UserManager<User> userManager, IQuestionRepository questionRepository, IHubContext<ChatHub> hubContext, INotificationRepository<AnswerNotification> notificationRepository, IQuestionSubscriptionRepository subscriptionRepository)
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
        public async Task<ActionResult<IEnumerable<ResponseAnswerDto>>> GetAnswers()
        {
            try
            {
                IEnumerable<Answer> answers = await _answerRepository.getAllAsync();
                List<ResponseAnswerDto> res = new List<ResponseAnswerDto>();
                foreach (var answer in answers)
                {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    res.Add(ResponseAnswerDto.convert(answer, user));
                }
                return Ok(res);
            }
            catch
            {
                return BadRequest("Error finding all answers");
            }
        }

        //[Authorize(Roles = "student,prof")]
        [HttpGet("GetAnswersByQuestionId/{questionId}")] //FIXME: Make route lower case
        public async Task<ActionResult<IEnumerable<ResponseAnswerDto>>> GetAnswersByQuestionId(int questionId)
        {
            try
            {
                IEnumerable<Answer> answers = await _answerRepository.getByQuestionId(questionId);
                List<ResponseAnswerDto> res = new List<ResponseAnswerDto>();
                foreach (var answer in answers)
                {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    res.Add(ResponseAnswerDto.convert(answer, user));
                }
                return Ok(res);
            }
            catch
            {
                return BadRequest("Error finding all answers");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAnswerDto>> GetAnswer(int id)
        {
            try
            {
                var answer = await _answerRepository.getAsync(id);
                User user = await _userManager.FindByIdAsync(answer.userId);
                if (answer == null)
                {
                    return NotFound();
                }

                return Ok(ResponseAnswerDto.convert(answer, user));
            }
            catch
            {
                return BadRequest("Error finding all answers");
            }
        }

        //[Authorize(Roles = "student")]
        [HttpPost]
        public async Task<ActionResult<ResponseAnswerDto>> CreateAnswer(PostAnswerDto dto)
        {

            // Get user from token
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                User user = await _userManager.FindByEmailAsync(userEmail);

                Question question = await _questionRepository.getAsync(dto.questionId);
                if (user == null || question == null) { return BadRequest("User or Question related to answer not found"); }
                Answer answer = new()
                {
                    userId = user.Id,
                    question = question,
                    title = dto.title,
                    body = dto.body,
                    // files = createAnswerDto.files
                    time = DateTime.UtcNow
                };

                await _answerRepository.createAsync(answer);

                IEnumerable<string> subscriberIds = (await _subscriptionRepository.getByQuestionId(question.id)).Select(sub => sub.userId);
                await _notificationRepository.createAllAync(subscriberIds.Select(userId => new AnswerNotification
                {
                    userId = userId,
                    answerId = answer.id,
                    answer = answer,
                    time = answer.time
                }));

                var ret = ResponseAnswerDto.convert(answer, user);
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
            var existingAnswer = await _answerRepository.getAsync(id);
            if (existingAnswer is null)
            {
                return NotFound();
            }

            await _answerRepository.deleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAnswer(int id, PostAnswerDto dto)
        {

            Answer existingAnswer = await _answerRepository.getAsync(id);
            User user = await _userManager.FindByIdAsync(dto.userId); //TODO: kunnen we dit miscchien uit de jwt van request halen?
            Question question = await _questionRepository.getAsync(dto.questionId);
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
                // files = createAnswerDto.files
                time = DateTime.UtcNow,
                isAccepted = existingAnswer.isAccepted
            };

            await _answerRepository.updateAsync(updatedAnswer);
            return NoContent();
        }

        // [Authorize(Roles = "prof")]
        [HttpPut("SetAccepted/{id}")] //FIXME: Make route lower case
        public async Task<ActionResult> SetAnswerAccepted(int id, bool accepted)
        {
            Answer existingAnswer = await _answerRepository.getAsync(id);
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "userref"))
            {
                string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
                Question question = await _questionRepository.getAsync(existingAnswer.questionId);
                if (question.userId == userId)
                {
                    existingAnswer.isAccepted = accepted;
                    await _answerRepository.updateAsync(existingAnswer);
                    return NoContent();
                }
            }
            return Unauthorized();
        }
    }
}
