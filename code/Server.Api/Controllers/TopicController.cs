using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using System.Linq;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController: ControllerBase
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ICourseRepository _courseRepository;

        private static getTopicDto toDto(Topic topic) {
			return new getTopicDto
			{
				id = topic.id,
				name = topic.name,
				course = new getOnlyCourseDto {
                    id = topic.course.id,
                    name = topic.course.name,
                    number = topic.course.number,
                },
				questions = topic.questions.Select(question => new onlyQuestionDto
				{
					id = question.id,
					title = question.title,
					body = question.body,
					time = question.dateTime
				}).ToList()
			};
		}
        public TopicController(ITopicRepository topicRepository, ICourseRepository courseRepository)
        {
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<getTopicDto>>> GetTopics()
        {
            var topics = await _topicRepository.getAllAsync();
            return Ok(topics.Select(topic => toDto(topic)));
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<getTopicDto>> GetTopic(int id)
        {
            var topic = await _topicRepository.getAsync(id);
            if(topic == null)
                return NotFound();

			return Ok(toDto(topic));
		}
    
        [HttpPost]
        public async Task<ActionResult<Topic>> CreateTopic(createTopicDto dto)
        {
            Topic topic = new()
            {
                name = dto.name,
                course = await _courseRepository.getAsync(dto.courseId),
                questions = new List<Question>()
            };
    
            await _topicRepository.createAsync(topic);
            return Ok(topic);
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(int id)
        {
            await _topicRepository.deleteAsync(id);
            return NoContent();
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTopic(int id, createTopicDto dto)
        {
            Topic topic = new()
            {
                id = id,
                name = dto.name,
                course = await _courseRepository.getAsync(dto.courseId)
            };
    
            await _topicRepository.updateAsync(topic);
            return NoContent();
        }
    }
}