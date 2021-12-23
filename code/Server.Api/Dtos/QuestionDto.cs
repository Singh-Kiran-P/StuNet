using System;
using Server.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Api.Dtos
{
    public record GetQuestionDto
    {
        public int id { get; set; }
        public ResponseUserDto user { get; set; }
        public GetPartialCourseDto course { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public ICollection<getOnlyTopicDto> topics { get; set; }
        public DateTime time { get; set; }

        public static GetQuestionDto Convert(Question question, User user)
        {
            return new GetQuestionDto
            {
                id = question.id,
                course = GetPartialCourseDto.Convert(question.course),
                title = question.title,
                body = question.body,
                topics = question.topics.Select(topic => getOnlyTopicDto.Convert(topic)).ToList(),
                time = question.time
            };
        }
    }

    public record OnlyQuestionDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }
        public ICollection<getOnlyTopicDto> topics { get; set; }

        public static OnlyQuestionDto Convert(Question question)
        {
            return new OnlyQuestionDto
            {
                id = question.id,
                title = question.title,
                body = question.body,
                topics = question.topics.Select(topic => getOnlyTopicDto.Convert(topic)).ToList(),
                time = question.time
            };
        }
    }

    // public record GetPartialQuestionDto
    // {
    //     public int id { get; set; }
    //     public Course course { get; set; }
    //     public string title { get; set; }
    //     public ResponseUserDto user { get; set; }
    //     public string body { get; set; }
    //     public ICollection<getOnlyTopicDto> topics { get; set; }
    //     public DateTime time { get; set; }

    //     public static onlyQuestionUserDto Convert(Question question, User user)
    //     {
    //         return new onlyQuestionUserDto
    //         {
    //             id = question.id,
    //             title = question.title,
    //             user = ResponseUserDto.Convert(user),
    //             body = question.body,
    //             topics = question.topics.Select(topic => getOnlyTopicDto.Convert(topic)).ToList(),
    //             time = question.time
    //         };
    //     }
    // }

    // public record GetPartialAnonymousQuestionDto
    // {
    //     public int id { get; set; }
    //     public Course course { get; set; }
    //     public string title { get; set; }
    //     public string body { get; set; }
    //     public ICollection<getOnlyTopicDto> topics { get; set; }
    //     public DateTime time { get; set; }

    //     public static questionAnonymousDto Convert(Question question, User user)
    //     {
    //         throw new System.Exception("method not implement");
    //     }
    // }
    
    public record createQuestionDto
    {
        public int courseId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public ICollection<int> topicIds { get; set; }

        public static createQuestionDto Convert(Question question, User user)
        {
            throw new System.Exception("method not implement");
        }
    }
}
