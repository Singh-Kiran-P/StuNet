using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController: ControllerBase
    {
        private readonly ITopicRepository _topicRepository;
        public TopicController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
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
        public async Task<ActionResult> CreateTopic(TopicDto createTopicDto)
        {
            Topic topic = new()
            {
                name = createTopicDto.name
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
        public async Task<ActionResult> UpdateTopic(int id, TopicDto updateTopicDto)
        {
            Topic topic = new()
            {
                TopicId = id,
                name = updateTopicDto.name
            };
    
            await _topicRepository.updateAsync(topic);
            return Ok();
        }
    }
}