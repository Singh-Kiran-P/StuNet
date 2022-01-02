using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record GetAnswerDto
    {
        public int id { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public DateTime time { get; set; }
        public bool isAccepted { get; set; }
        public ResponseUserDto user { get; set; }
        public GetQuestionDto question { get; set; }

        public static GetAnswerDto Convert(Answer answer, User user)
        {
            return new GetAnswerDto {
                id = answer.id,
                body = answer.body,
                time = answer.time,
                title = answer.title,
                isAccepted = answer.isAccepted,
                user = ResponseUserDto.Convert(user),
                question = GetQuestionDto.Convert(answer.question, user)
            };
        }
    }

    public record CreateAnswerDto
    {
        public string body { get; set; }
        public string title { get; set; }
        public int questionId { get; set; }
    }
}
