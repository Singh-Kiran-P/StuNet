using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using System.Linq;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController: ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        public QuestionController(IQuestionRepository questionRepository, ITopicRepository topicRepository)
        {
            _questionRepository = questionRepository;
			_topicRepository = topicRepository;
		}
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            var questions = await _questionRepository.getAllAsync();
            return Ok(questions);
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _questionRepository.getAsync(id);
            if(question == null)
                return NotFound();
    
            return Ok(question);
        }
    
        [HttpPost]
        public async Task<ActionResult> CreateQuestion(QuestionDto createQuestionDto)
        {
			Question question = new()
			{
				title = createQuestionDto.title,
				// user = createQuestionDto.user,
				// course = createQuestionDto.course,
				body = createQuestionDto.body,
				// files = createQuestionDto.files
				topics = createQuestionDto.topics.Select(id => _topicRepository.getAsync(id))
												.Select(task => task.Result)
												.ToList(),
			    dateTime = DateTime.Now
            };

			await _questionRepository.createAsync(question);
            return Ok();
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            await _questionRepository.deleteAsync(id);
            return Ok();
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, QuestionDto updateQuestionDto)
        {
            Question question = new()
            {
                id = id,
                title = updateQuestionDto.title,
                // user = updateQuestionDto.user,
                // course = updateQuestionDto.course,
                body = updateQuestionDto.body,
                // files = updateQuestionDto.files
                // Topics = updateQuestionDto.Topics
                dateTime = DateTime.Now
            };
    
            await _questionRepository.updateAsync(question);
            return Ok();
        }
    }
}