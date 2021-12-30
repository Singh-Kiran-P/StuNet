using System.Collections.Generic;

namespace Server.Api.Models
{
    public class TextChannel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int courseId { get; set; }
        public Course course { get; set; }
        public List<Message> messages { get; set; }
    }
}
