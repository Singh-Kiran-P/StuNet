using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
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

    // public record CreateMessageDto
    // {
    //     public int channelId { get; set; }
    //     public string userMail { get; set; }
    //     public string body { get; set; }

    //     public static CreateMessageDto Convert(Message message)
    //     {
    //         throw new Exception("method not implement");
    //     }
    // }
}
