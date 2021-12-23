using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record createMessageDto
    {
        public int channelId { get; set; }
        public string userMail { get; set; }
        public string body { get; set; }

        public static createMessageDto Convert(Message message)
        {
            throw new Exception("method not implement");
        }
    }

    public record MessageDto
    {
        public string userMail { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }

        public static MessageDto Convert(Message message)
        {
            return new()
            {
                userMail = message.userMail,
                body = message.body,
                time = message.time
            };
        }
    }
}
