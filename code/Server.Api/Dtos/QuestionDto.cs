using System;
using System.Linq;
using Server.Api.Models;
using System.Collections.Generic;

namespace Server.Api.Dtos
{
    public record GetQuestionDto
    {
        public int id { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public DateTime time { get; set; }
        public ResponseUserDto user { get; set; }
        public GetPartialCourseDto course { get; set; }
        public ICollection<GetPartialTopicDto> topics { get; set; }

        public static GetQuestionDto Convert(Question question, User user)
        {
            return new GetQuestionDto
            {
                id = question.id,
                time = question.time,
                body = question.body,
                title = question.title,
                user = ResponseUserDto.Convert(user),
                course = GetPartialCourseDto.Convert(question.course),
                topics = question.topics.Select(topic => GetPartialTopicDto.Convert(topic)).ToList()
            };
        }
    }

    public record GetPartialQuestionDto
    {
        public int id { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public DateTime time { get; set; }
        public ICollection<GetPartialTopicDto> topics { get; set; }

        public static GetPartialQuestionDto Convert(Question question)
        {
            return new GetPartialQuestionDto {
                id = question.id,
                time = question.time,
                body = question.body,
                title = question.title,
                topics = question.topics.Select(topic => GetPartialTopicDto.Convert(topic)).ToList()
            };
        }
    }
    
    public record CreateQuestionDto
    {
        public int courseId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public ICollection<int> topicIds { get; set; }
    }
}
