using System.Linq;
using Server.Api.Dtos;
using Server.Api.Models;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

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
            return new GetTopicDto {
                id = topic.id,
                name = topic.name,
                questions = topic.questions.Select(question => GetPartialQuestionDto.Convert(question)).ToList(),
                course = new GetPartialCourseDto {
                    id = topic.course.id,
                    name = topic.course.name,
                    number = topic.course.number
                }
            };
        }

        public TopicController(ITopicRepository topicRepository, ICourseRepository courseRepository)
        {
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTopicDto>>> GetTopics()
        {
            var topics = await _topicRepository.GetAllAsync();
            return Ok(topics.Select(topic => ToDto(topic)));
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTopicDto>> GetTopic(int id)
        {
            var topic = await _topicRepository.GetAsync(id);
            if (topic == null) return NotFound();
            return Ok(ToDto(topic));
        }

        [Authorize(Roles = "prof")]
        [HttpPost]
        public async Task<ActionResult<Topic>> CreateTopic(CreateTopicDto dto)
        {
            Topic topic = new() {
                name = dto.name,
                course = await _courseRepository.GetAsync(dto.courseId),
                questions = new List<Question>()
            };

            await _topicRepository.CreateAsync(topic);
            return Ok(topic);
        }

        [Authorize(Roles = "prof")]

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(int id)
        {
            var existing = await _topicRepository.GetAsync(id);
            if (existing == null) return NotFound();
            await _topicRepository.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTopic(int id, CreateTopicDto dto)
        {
            var existing = await _topicRepository.GetAsync(id);
            if (existing == null) return NotFound();
            Topic topic = new() {
                id = id,
                name = dto.name
            };

            await _topicRepository.UpdateAsync(topic);
            return NoContent();
        }
    }
}
