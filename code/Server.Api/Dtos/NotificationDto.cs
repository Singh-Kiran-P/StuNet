using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record GetNotificationDto
    {
        public int id { get; set; }
        public int notifierId { get; set; }
        public DateTime time { get; set; }
    }

    public record GetQuestionNotificationDto : GetNotificationDto
    {
        public Question notifier { get; set; }
        public static GetQuestionNotificationDto Convert(QuestionNotification notification)
        {
            return new GetQuestionNotificationDto
            {
                id = notification.id,
                notifierId = notification.questionId,
                notifier = notification.question,
                time = notification.time
            };
        }
    }

    public record GetAnswerNotificationDto : GetNotificationDto
    {
        public Answer notifier { get; set; }
        public static GetAnswerNotificationDto Convert(AnswerNotification notification)
        {
            return new GetAnswerNotificationDto
            {
                id = notification.id,
                notifierId = notification.answerId,
                notifier = notification.answer,
                time = notification.time
            };
        }
    }
}