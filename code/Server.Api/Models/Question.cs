using System;
using System.Collections.Generic;

namespace Server.Api.Models
{
	public class Question
	{
		public int id { get; set; }
		public string userId { get; set; }
		public Course course { get; set; }
	    public string title { get; set; }
        public string body { get; set; }

        // public File[] files { get; set; }
        public ICollection<Topic> topics { get; set; }
		public DateTime time { get; set; }
    }
}