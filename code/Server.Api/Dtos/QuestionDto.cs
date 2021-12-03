using System;
using Server.Api.Models;
using System.Collections.Generic;
using System.Linq;

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
    public record onlyQuestionUserDto {
        public int id { get; set; }
		public String title { get; set; }
        public ResponseUserDto user { get; set; }
        public String body { get; set; }
        public DateTime dateTime { get; set; }
        public static onlyQuestionUserDto convert(Question question, User user) {
            return new onlyQuestionUserDto {
                    id = question.id,
                    title = question.title,
                    user = ResponseUserDto.convert(user),
                    body = question.body,
                    dateTime = question.dateTime
                };
        }        
    }

    public record questionDto {
        public int id { get; set; }
        public ResponseUserDto user { get; set; }
        public getOnlyCourseDto course { get; set; }
		public string title { get; set; }
        public string body { get; set; }
        public ICollection<getOnlyTopicDto> topics { get; set; }
        public DateTime time { get; set; }

        public static questionDto convert(Question question, User user) {
            return new questionDto {
                    id = question.id,
                    course = new getOnlyCourseDto
                    {
                        id = question.course.id,
                        name = question.course.name,
                        number = question.course.number,
                    },
                    title = question.title,
                    body = question.body,
                    topics = question.topics.Select(topic => new getOnlyTopicDto
                    {
                        id = topic.id,
                        name = topic.name
                    }).ToList(),
                    time = question.dateTime
                };
        }       
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