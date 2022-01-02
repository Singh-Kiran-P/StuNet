using System;

namespace Server.Api.Models
{
    public class Message
    {
        public int id { get; set; }
        public string body { get; set; }
        public int channelId { get; set; }
        public DateTime time { get; set; }
        public string userMail { get; set; }
        public TextChannel channel { get; set; }
    }
}
