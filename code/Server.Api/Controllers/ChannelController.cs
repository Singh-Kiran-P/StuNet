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
    public class ChannelController : ControllerBase
    {
        private readonly IChannelRepository _channelRepository;
        private readonly ICourseRepository _courseRepository;

        public ChannelController(IChannelRepository channelRepository, ICourseRepository courseRepository)
        {
            _channelRepository = channelRepository;
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<getOnlyChannelDto>>> GetChannels()
        {
            var channels = await _channelRepository.getAllAsync();
            return Ok(channels.Select(channel => getOnlyChannelDto.convert(channel)));
        }

        [HttpGet("GetChannelsByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<getOnlyChannelDto>>> GetChannelsByCourseId(int courseId)
        {
            var channels = await _channelRepository.getByCourseIdAsync(courseId);
            return Ok(channels.Select(channel => getOnlyChannelDto.convert(channel)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<getChannelDto>> GetChannel(int id)
        {
            var channel = await _channelRepository.getAsync(id);
            if (channel == null)
                return NotFound();

            return Ok(getChannelDto.convert(channel));
        }

        [HttpPost]
        public async Task<ActionResult<getOnlyChannelDto>> CreateChannel(createChannelDto dto)
        {
            TextChannel channel = new()
            {
                name = dto.name,
                course = await _courseRepository.getAsync(dto.courseId),
                messages = new List<Message>()
            };

            await _channelRepository.createAsync(channel);
            return Ok(getOnlyChannelDto.convert(channel));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChannel(int id)
        {
            await _channelRepository.deleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateChannel(int id, createChannelDto dto)
        {
            TextChannel channel = new()
            {
                id = id,
                name = dto.name,
                course = await _courseRepository.getAsync(dto.courseId)
            };

            await _channelRepository.updateAsync(channel);
            return NoContent();
        }
    }
}
