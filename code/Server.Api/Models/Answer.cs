using System;

namespace Server.Api.Models
{
    public class Answer
    {
        public int id { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public DateTime time { get; set; }
        public string userId { get; set; }
        public int questionId { get; set; }
        public bool isAccepted { get; set; } = false;
        public virtual Question question { get; set; }
    }
}
