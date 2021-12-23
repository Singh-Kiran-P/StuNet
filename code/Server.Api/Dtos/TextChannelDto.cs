using Server.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Api.Dtos
{
    public record getOnlyChannelDto
    {
        public int id { get; set; }
        public string name { get; set; }

        public static getOnlyChannelDto convert(TextChannel channel)
        {
            return new getOnlyChannelDto
            {
                id = channel.id,
                name = channel.name
            };
        }
    }

    public record getChannelDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public CourseDto course { get; set; }
        public ICollection<MessageDto> messages { get; set; }

        public static getChannelDto convert(TextChannel channel)
        {
            return new getChannelDto
            {
                id = channel.id,
                name = channel.name,
                course = CourseDto.convert(channel.course),
                messages = channel.messages.Select(m => MessageDto.convert(m)).ToList()
            };
        }
    }

    public record createChannelDto
    {
        public string name { get; set; }
        public int courseId { get; set; }
    }
}
