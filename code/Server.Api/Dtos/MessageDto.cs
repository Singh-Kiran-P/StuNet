using System;
using Server.Api.Dtos;
using Server.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Api.Dtos
{
    public class createMessageDto
    {
        public int channelId { get; set; }
        public string userMail { get; set; }
        public string body { get; set; }

    }

    public class MessageDto
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
