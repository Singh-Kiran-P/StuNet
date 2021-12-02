using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController: ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly UserManager<User> _userManager;
        // private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        public AnswerController(IAnswerRepository answerRepository,UserManager<User> userManager, IQuestionRepository questionRepository/*, ITopicRepository topicRepository*/)
        {
            _answerRepository = answerRepository;
            _userManager = userManager;
            _questionRepository = questionRepository;
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
                      
                //put all users in the dto
                IEnumerable<Task<ResponseAnswerDto>> answerTasks = answers.Select(async answer => {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    return ResponseAnswerDto.convert(answer, user);
                    });

                ResponseAnswerDto[] res = await Task.WhenAll(answerTasks);
                return Ok(res);
            }
            catch{return BadRequest("Error finding all answers");}
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("GetAnswersByQuestionId")]
        public async Task<ActionResult<IEnumerable<ResponseAnswerDto>>> GetAnswersByQuestionId(int questionId)
        {
            try
            {
                 IEnumerable<Answer> answers = _answerRepository.getByQuestionId(questionId);
                      
                //put all users in the dto
                IEnumerable<Task<ResponseAnswerDto>> answerTasks = answers.Select(async answer => {
                    User user = await _userManager.FindByIdAsync(answer.userId);
                    return ResponseAnswerDto.convert(answer, user);
                    });

                ResponseAnswerDto[] res = await Task.WhenAll(answerTasks);
                return Ok(res);
            }
            catch{return BadRequest("Error finding all answers");}
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAnswerDto>> GetAnswer(int id)
        {
            var answer = await _answerRepository.getAsync(id);
            User user = await _userManager.FindByIdAsync(answer.userId);
            if(answer == null)
                return NotFound();
    
            return Ok(ResponseAnswerDto.convert(answer, user));
        }
    
        //[Authorize(Roles = "student")]
        [HttpPost]
        public async Task<ActionResult<ResponseAnswerDto>> CreateAnswer(PostAnswerDto dto)
        {
            string userId = dto.userId;
            try{userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;} catch {Console.Write("userid failed");}
            Console.Write(userId);
            User user = await _userManager.FindByIdAsync(userId); //TODO: kunnen we dit miscchien uit de jwt van request halen?
            Question question = await _questionRepository.getAsync(dto.questionId);
            if(user == null || question==null){return BadRequest("User or Question related to answer not found");}
			Answer answer = new()
			{
                userId = userId,
                question = question,
				title = dto.title,
				body = dto.body,
				// files = createAnswerDto.files
			    dateTime = DateTime.Now
            };

			await _answerRepository.createAsync(answer);
            return Ok(ResponseAnswerDto.convert(answer, user));
        }

        [Authorize(Roles = "prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
			var existingAnswer = await _answerRepository.getAsync(id);
            if (existingAnswer is null) {
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
            if (existingAnswer == null || user == null || question == null) {
				return NotFound();
			}

			Answer updatedAnswer = new()
            {
                userId = user.Id,
                question = question,
				title = dto.title,
				body = dto.body,
				// files = createAnswerDto.files
			    dateTime = DateTime.Now
            };
    
            await _answerRepository.updateAsync(updatedAnswer);
            return NoContent();
        }
    }
}