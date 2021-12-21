using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
	public class NotificationDto
	{
		public int id { get; set; }
		public string userId { get; set; }
		public int notifierId { get; set; }
		public DateTime time { get; set; }

		public NotificationDto convert(CourseNotification notification) {
			return new NotificationDto
			{
				id = notification.id,
				userId = notification.userId,
				notifierId = notification.courseId,
				time = notification.time
			};
		}
		public NotificationDto convert(QuestionNotification notification) {
			return new NotificationDto
			{
				id = notification.id,
				userId = notification.userId,
				notifierId = notification.questionId,
				time = notification.time
			};
		}
	}
}