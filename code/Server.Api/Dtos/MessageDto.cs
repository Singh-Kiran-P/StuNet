using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record MessageDto
    {
        public string body { get; set; }
        public DateTime time { get; set; }
        public string userMail { get; set; }

        public static MessageDto Convert(Message message)
        {
            return new() {
                body = message.body,
                time = message.time,
                userMail = message.userMail
            };
        }
    }
}
