using Server.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Api.Dtos
{
    public record GetChannelDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public GetPartialCourseDto course { get; set; }
        public ICollection<MessageDto> messages { get; set; }

        public static GetChannelDto Convert(TextChannel channel)
        {
            return new GetChannelDto
            {
                id = channel.id,
                name = channel.name,
                course = GetPartialCourseDto.Convert(channel.course),
                messages = channel.messages.Select(m => MessageDto.Convert(m)).ToList()
            };
        }
    }

    public record GetPartialChannelDto
    {
        public int id { get; set; }
        public string name { get; set; }

        public static GetPartialChannelDto Convert(TextChannel channel)
        {
            return new GetPartialChannelDto
            {
                id = channel.id,
                name = channel.name
            };
        }
    }

    public record CreateChannelDto
    {
        public string name { get; set; }
        public int courseId { get; set; }

        public static CreateChannelDto Convert(TextChannel channel, User user)
        {
            throw new System.Exception("method not implement");
        }
    }
}
