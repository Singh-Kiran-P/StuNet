using System;

namespace Server.Api.Models
{
    public class Answer
    {
        public int id { get; set; }
		public User user { get; set; }
		public Question question { get; set; }
	    public string title { get; set; }
        public string body { get; set; }
        // public File[] files { get; set; }
		public DateTime dateTime { get; set; }
    }
}