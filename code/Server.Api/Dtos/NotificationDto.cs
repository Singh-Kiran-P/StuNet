using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record GetNotificationDto
    {
        public int id { get; set; }
        public int notifierId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime time { get; set; }

        public static GetNotificationDto Convert(AnswerNotification notification)
        {
            return new GetNotificationDto
            {
                id = notification.id,
                notifierId = notification.answerId,
                title = notification.answer.title,
                body = notification.answer.body,
                time = notification.time
            };
        }

        public static GetNotificationDto Convert(QuestionNotification notification)
        {
            return new GetNotificationDto
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