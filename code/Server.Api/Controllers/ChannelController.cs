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
        public async Task<ActionResult<IEnumerable<GetPartialChannelDto>>> GetChannels()
        {
            var channels = await _channelRepository.getAllAsync();
            return Ok(channels.Select(channel => GetPartialChannelDto.Convert(channel)));
        }

        [HttpGet("GetChannelsByCourseId/{courseId}")] //FIXME: Make route lower case
        public async Task<ActionResult<IEnumerable<GetPartialChannelDto>>> GetChannelsByCourseId(int courseId)
        {
            var channels = await _channelRepository.getByCourseIdAsync(courseId);
            return Ok(channels.Select(channel => GetPartialChannelDto.Convert(channel)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetChannelDto>> GetChannel(int id)
        {
            var channel = await _channelRepository.getAsync(id);
            if (channel == null)
                return NotFound();

            return Ok(GetChannelDto.Convert(channel));
        }

        [HttpPost]
        public async Task<ActionResult<GetPartialChannelDto>> CreateChannel(CreateChannelDto dto)
        {
            TextChannel channel = new()
            {
                name = dto.name,
                course = await _courseRepository.getAsync(dto.courseId),
                messages = new List<Message>()
            };

            await _channelRepository.createAsync(channel);
            return Ok(GetPartialChannelDto.Convert(channel));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChannel(int id)
        {
            await _channelRepository.deleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateChannel(int id, CreateChannelDto dto)
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
