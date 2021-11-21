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
		public getOnlyCourseDto course { get; set; }
		public ICollection<onlyQuestionDto> questions { get; set; }
	}
}