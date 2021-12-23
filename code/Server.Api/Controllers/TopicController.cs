using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ICourseRepository _courseRepository;

        private static GetTopicDto ToDto(Topic topic)
        {
            return new GetTopicDto
            {
                id = topic.id,
                name = topic.name,
                course = new GetPartialCourseDto
                {
                    id = topic.course.id,
                    name = topic.course.name,
                    number = topic.course.number,
                },
                questions = topic.questions.Select(question => GetPartialQuestionDto.Convert(question)).ToList()
            };
        }
        
        public TopicController(ITopicRepository topicRepository, ICourseRepository courseRepository)
        {
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTopicDto>>> GetTopics()
        {
            var topics = await _topicRepository.getAllAsync();
            return Ok(topics.Select(topic => ToDto(topic)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTopicDto>> GetTopic(int id)
        {
            var topic = await _topicRepository.getAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return Ok(ToDto(topic));
        }

        [HttpPost]
        public async Task<ActionResult<Topic>> CreateTopic(CreateTopicDto dto)
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
        public async Task<ActionResult> UpdateTopic(int id, CreateTopicDto dto)
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
