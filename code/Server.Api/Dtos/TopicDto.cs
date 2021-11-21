using System;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
	public record createTopicDto {
		public string name { get; set; }
		public int courseId { get; set; }
	}

	public record getOnlyTopicDto {
        public int id { get; set; }
		public String name { get; set; }
	}

	public record getTopicDto {
		public int id { get; set; }
		public String name { get; set; }
		public Course course { get; set; }
		public ICollection<Question> questions { get; set; }
	}
}