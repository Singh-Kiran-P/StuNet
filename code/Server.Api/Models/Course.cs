using System.Collections.Generic;

namespace Server.Api.Models
{
    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string profEmail { get; set; }
        public string description { get; set; }
        public string courseEmail { get; set; }
        public ICollection<Topic> topics { get; set; }
        public ICollection<Question> questions { get; set; }
        public ICollection<TextChannel> channels { get; set; }
    }
}
