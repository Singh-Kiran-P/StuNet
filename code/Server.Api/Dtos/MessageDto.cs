using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record createMessageDto
    {
        public int channelId { get; set; }
        public string userMail { get; set; }
        public string body { get; set; }
    }

    public record MessageDto
    {
        public string userMail { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }

        public static MessageDto convert(Message msg)
        {
            return new()
            {
                userMail = msg.userMail,
                body = msg.body,
                time = msg.time
            };
        }
    }
}
