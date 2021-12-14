using System;

namespace Server.Api.Models
{
    public class TextChannel
    {
        public int id { get; set; }
        public string name { get; set; }
		public int courseId { get; set; }
		public Course course { get; set; }

		// public ICollection<Message> messages { get; set; }
	}
}