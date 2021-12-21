using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using Microsoft.AspNetCore.SignalR;
using ChatSample.Hubs;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly UserManager<User> _userManager;
        // private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
		private readonly IHubContext<ChatHub> _hubContext;
		public AnswerController(IAnswerRepository answerRepository, UserManager<User> userManager, IQuestionRepository questionRepository, IHubContext<ChatHub> hubContext /*, ITopicRepository topicRepository*/ )
        {
            _answerRepository = answerRepository;
            _userManager = userManager;
            _questionRepository = questionRepository;
			_hubContext = hubContext;
			// _topicRepository = topicRepository;
			// _courseRepository = courseRepository;
		}

        //[Authorize(Roles = "student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseAnswerDto>>> GetAnswers()
        {
            try
            {
                IEnumerable<Answer> answers = await _answerRepository.getAllAsync();
                List<ResponseAnswerDto> res = new List<ResponseAnswerDto>();
                foreach (var answer in answers) {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    res.Add(ResponseAnswerDto.convert(answer, user));
                }
                return Ok(res);
            }
            catch { return BadRequest("Error finding all answers"); }
        }

        //[Authorize(Roles = "student,prof")]
        [HttpGet("GetAnswersByQuestionId/{questionId}")]
        public async Task<ActionResult<IEnumerable<ResponseAnswerDto>>> GetAnswersByQuestionId(int questionId)
        {
            try
            {
                IEnumerable<Answer> answers = await _answerRepository.getByQuestionId(questionId);
                List<ResponseAnswerDto> res = new List<ResponseAnswerDto>();
                foreach (var answer in answers) {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    res.Add(ResponseAnswerDto.convert(answer, user));
                }
                return Ok(res);
            }
            catch { return BadRequest("Error finding all answers"); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAnswerDto>> GetAnswer(int id)
        {
            try
            {
                var answer = await _answerRepository.getAsync(id);
                User user = await _userManager.FindByIdAsync(answer.userId);
                if (answer == null)
                    return NotFound();

                return Ok(ResponseAnswerDto.convert(answer, user));
            }
            catch { return BadRequest("Error finding all answers"); }
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
                await _hubContext.Clients.Group("Question " + question.id).SendAsync("AnswerNotification", answer.id);
                return Ok(ResponseAnswerDto.convert(answer, user));
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
                userId = user.Id,
                question = question,
                title = dto.title,
                body = dto.body,
                // files = createAnswerDto.files
                time = DateTime.UtcNow
            };

            await _answerRepository.updateAsync(updatedAnswer);
            return NoContent();
        }
    }
}
