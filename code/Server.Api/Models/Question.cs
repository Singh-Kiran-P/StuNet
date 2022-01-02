using System;
using System.Collections.Generic;

namespace Server.Api.Models
{
    public class Question
    {
        public int id { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public int courseId { get; set; }
        public Course course { get; set; }
        public string userId { get; set; }
        public DateTime time { get; set; }
        public ICollection<Topic> topics { get; set; }
    }
}
