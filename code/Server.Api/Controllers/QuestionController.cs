using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ICourseRepository _courseRepository;
        public QuestionController(IQuestionRepository questionRepository, ITopicRepository topicRepository, ICourseRepository courseRepository)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
        }

        private static questionDto toDto(Question question)
        {
            return new questionDto
            {
                id = question.id,
                user = question.user,
                course = new getOnlyCourseDto
                {
                    id = question.course.id,
                    name = question.course.name,
                    number = question.course.number,
                },
                title = question.title,
                body = question.body,
                topics = question.topics.Select(topic => new getOnlyTopicDto
                {
                    id = topic.id,
                    name = topic.name
                }).ToList(),
                time = question.dateTime
            };
        }

        [Authorize(Roles = "student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<questionDto>>> GetQuestions()
        {
            var questions = await _questionRepository.getAllAsync();
            return Ok(questions.Select(question => toDto(question)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<questionDto>> GetQuestion(int id)
        {
            var question = await _questionRepository.getAsync(id);
            if (question == null)
                return NotFound();

            return Ok(toDto(question));
        }

        [HttpPost]
        public async Task<ActionResult<questionDto>> CreateQuestion(createQuestionDto dto)
        {
            Question question = new()
            {
                title = dto.title,
                // user = createQuestionDto.user,
                course = _courseRepository.getAsync(dto.courseId).Result,
                body = dto.body,
                // files = createQuestionDto.files
                topics = dto.topicIds.Select(id => _topicRepository.getAsync(id))
                                                .Select(task => task.Result)
                                                .ToList(),
                dateTime = DateTime.Now
            };

            await _questionRepository.createAsync(question);
            return Ok(question);
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
                dateTime = DateTime.Now
            };

            await _questionRepository.updateAsync(updatedQuestion);
            return NoContent();
        }
    }
}
