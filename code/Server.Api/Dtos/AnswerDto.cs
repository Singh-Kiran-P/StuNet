using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record ResponseAnswerDto
    {
        public int id { get; set; }
        public ResponseUserDto user { get; set; }
        public questionDto question { get; set; }
        // public getOnlyCourseDto course { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }
        public bool isAccepted { get; set; }

        public static ResponseAnswerDto convert(Answer answer, User user)
        {
            return new ResponseAnswerDto
            {
                id = answer.id,
                user = ResponseUserDto.convert(user),
                question = questionDto.convert(answer.question, user),
                // course = getOnlyCourseDto.convert(answer.question.course),
                title = answer.title,
                body = answer.body,
                time = answer.time,
                isAccepted = answer.isAccepted
            };
        }
    }

    public record PostAnswerDto
    {
        public string userId { get; set; }
        public int questionId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}
