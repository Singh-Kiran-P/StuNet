using System;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
    public record createQuestionDto {
		public int courseId { get; set; }
	    public string title { get; set; }
        public string body { get; set; }
        public ICollection<int> topicIds { get; set; }
    }

    public record onlyQuestionDto {
        public int id { get; set; }
		public String title { get; set; }
        public String body { get; set; }
        public DateTime time { get; set; }
    }

    public record questionDto {
        public int id { get; set; }
        public User user { get; set; }
        public getOnlyCourseDto course { get; set; }
		public string title { get; set; }
        public string body { get; set; }
        public ICollection<getOnlyTopicDto> topics { get; set; }
        public DateTime time { get; set; }
	}

    public record questionAnonymousDto {
        public int id { get; set; }
        public Course course { get; set; }
		public string title { get; set; }
        public string body { get; set; }
        public ICollection<getOnlyTopicDto> topics { get; set; }
        public DateTime time { get; set; }
    }
}