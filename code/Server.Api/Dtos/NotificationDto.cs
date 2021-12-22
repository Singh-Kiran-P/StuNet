using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
	public class NotificationDto
	{
		public int id { get; set; }
		public int notifierId { get; set; }
		public DateTime time { get; set; }

		public static NotificationDto convert(AnswerNotification notification) {
			return new NotificationDto
			{
				id = notification.id,
				notifierId = notification.answerId,
				time = notification.time
			};
		}
		public static NotificationDto convert(QuestionNotification notification) {
			return new NotificationDto
			{
				id = notification.id,
				notifierId = notification.questionId,
				time = notification.time
			};
		}
	}
}