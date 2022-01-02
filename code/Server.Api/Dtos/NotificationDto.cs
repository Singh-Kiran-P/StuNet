using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record GetNotificationDto
    {
        public int id { get; set; }
        public DateTime time { get; set; }
        public int notifierId { get; set; }
    }

    public record GetQuestionNotificationDto : GetNotificationDto
    {
        public Question notifier { get; set; }
        public static GetQuestionNotificationDto Convert(QuestionNotification notification)
        {
            return new GetQuestionNotificationDto {
                id = notification.id,
                time = notification.time,
                notifier = notification.question,
                notifierId = notification.questionId
            };
        }
    }

    public record GetAnswerNotificationDto : GetNotificationDto
    {
        public Answer notifier { get; set; }
        public static GetAnswerNotificationDto Convert(AnswerNotification notification)
        {
            return new GetAnswerNotificationDto {
                id = notification.id,
                time = notification.time,
                notifier = notification.answer,
                notifierId = notification.answerId
            };
        }
    }
}