using System;
using Server.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Api.Dtos
{
    public record GetQuestionDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }
        public ResponseUserDto user { get; set; }
        public GetPartialCourseDto course { get; set; }
        public ICollection<GetPartialTopicDto> topics { get; set; }

        public static GetQuestionDto Convert(Question question, User user)
        {
            return new GetQuestionDto
            {
                id = question.id,
                course = GetPartialCourseDto.Convert(question.course),
                title = question.title,
                body = question.body,
                topics = question.topics.Select(topic => GetPartialTopicDto.Convert(topic)).ToList(),
                time = question.time
            };
        }
    }

    public record GetPartialQuestionDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }
        public ICollection<GetPartialTopicDto> topics { get; set; }

        public static GetPartialQuestionDto Convert(Question question)
        {
            return new GetPartialQuestionDto
            {
                id = question.id,
                title = question.title,
                body = question.body,
                topics = question.topics.Select(topic => GetPartialTopicDto.Convert(topic)).ToList(),
                time = question.time
            };
        }
    }
    
    public record CreateQuestionDto
    {
        public int courseId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public ICollection<int> topicIds { get; set; }

        public static CreateQuestionDto Convert(Question question, User user)
        {
            throw new System.Exception("method not implement");
        }
    }
}
