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
    public class ChannelController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IChannelRepository _channelRepository;

        public ChannelController(IChannelRepository channelRepository, ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            _channelRepository = channelRepository;
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPartialChannelDto>>> GetChannels()
        {
            var channels = await _channelRepository.GetAllAsync();
            return Ok(channels.Select(channel => GetPartialChannelDto.Convert(channel)));
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("GetChannelsByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<GetPartialChannelDto>>> GetChannelsByCourseId(int courseId)
        {
            var channels = await _channelRepository.GetByCourseIdAsync(courseId);
            return Ok(channels.Select(channel => GetPartialChannelDto.Convert(channel)));
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetChannelDto>> GetChannel(int id)
        {
            var channel = await _channelRepository.GetAsync(id);
            if (channel == null)
                return NotFound();

            return Ok(GetChannelDto.Convert(channel));
        }

        [Authorize(Roles = "prof")]
        [HttpPost]
        public async Task<ActionResult<GetPartialChannelDto>> CreateChannel(CreateChannelDto dto)
        {
            TextChannel channel = new() {
                name = dto.name,
                messages = new List<Message>(),
                course = await _courseRepository.GetAsync(dto.courseId)
            };

            await _channelRepository.CreateAsync(channel);
            return Ok(GetPartialChannelDto.Convert(channel));
        }

        [Authorize(Roles = "prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChannel(int id)
        {
            var existing = await _channelRepository.GetAsync(id);
            if (existing == null) return NotFound();
            await _channelRepository.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateChannel(int id, CreateChannelDto dto)
        {
            var existing = await _channelRepository.GetAsync(id);
            if (existing == null) return NotFound();
            TextChannel channel = new() {
                id = id,
                name = dto.name
            };

            await _channelRepository.UpdateAsync(channel);
            return NoContent();
        }
    }
}
