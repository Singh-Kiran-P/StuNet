using System;

namespace Server.Api.Models
{
	public class Question
	{
		public int id { get; set; }
		// public User user { get; set; }
		// public Course course { get; set; }
	    public string title { get; set; }
        public string body { get; set; }

        // public File[] files { get; set; }
        // public Topic[] topics { get; set; }
		public DateTime dateTime { get; set; }
    }
}