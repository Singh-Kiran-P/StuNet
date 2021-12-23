using System.Collections.Generic;

namespace Server.Api.Models
{
    public class Topic
    {
        public int id { get; set; }
        public string name { get; set; }
        public int courseId { get; set; }
        public Course course { get; set; }
        public ICollection<Question> questions { get; set; }
    }
}
