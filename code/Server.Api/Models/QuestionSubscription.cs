using System;

namespace Server.Api.Models
{
    public class QuestionSubscription
    {
        public int id { get; set; }
        public string userId { get; set; }
        public int questionId { get; set; }

        public DateTime dateTime { get; set; }
    }
}
