using System;

namespace Server.Api.Models
{
	public class Notification 
	{
		public int id { get; set; }
		public string userId { get; set; }
		public DateTime time { get; set; }
	}

	public class AnswerNotification : Notification
    {
		public int answerId { get; set; }
		public Answer answer { get; set; }
	}

    public class QuestionNotification : Notification
    {
		public int questionId { get; set; }
		public Question question { get; set; }
	}
}
