using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController: ControllerBase
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ICourseRepository _courseRepository;
        public TopicController(ITopicRepository topicRepository, ICourseRepository courseRepository)
        {
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
            var topics = await _topicRepository.getAllAsync();
            return Ok(topics);
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopic(int id)
        {
            var topic = await _topicRepository.getAsync(id);
            if(topic == null)
                return NotFound();
    
            return Ok(topic);
        }
    
        [HttpPost]
        public async Task<ActionResult> CreateTopic(createTopicDto dto)
        {
            Topic topic = new()
            {
                name = dto.name,
                course = _courseRepository.getAsync(dto.courseId).Result
            };
    
            await _topicRepository.createAsync(topic);
            return Ok();
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(int id)
        {
            await _topicRepository.deleteAsync(id);
            return Ok();
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTopic(int id, createTopicDto dto)
        {
            Topic topic = new()
            {
                id = id,
                name = dto.name,
                course = _courseRepository.getAsync(dto.courseId).Result
            };
    
            await _topicRepository.updateAsync(topic);
            return Ok();
        }
    }
}