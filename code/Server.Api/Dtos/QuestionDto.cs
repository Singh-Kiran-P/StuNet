using System;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Controllers
{
    public record QuestionDto
    {
		// public User user { get; set; }
		// public Course course { get; set; }
	    public string title { get; set; }
        public string body { get; set; }

        // public File[] files { get; set; }
        public ICollection<int> topics { get; set; }
    }
}