using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record NotificationDto
    {
        public int id { get; set; }
        public int notifierId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }

        public static NotificationDto convert(AnswerNotification notification)
        {
            return new NotificationDto
            {
                id = notification.id,
                notifierId = notification.answerId,
                title = notification.answer.title,
                body = notification.answer.body,
                time = notification.time
            };
        }

        public static NotificationDto convert(QuestionNotification notification)
        {
            return new NotificationDto
            {
                id = notification.id,
                notifierId = notification.questionId,
                title = notification.question.title,
                body = notification.question.body,
                time = notification.time
            };
        }
    }
}