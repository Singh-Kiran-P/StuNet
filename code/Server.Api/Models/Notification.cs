using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Api.Models
{
	public class CourseNotification
    {
        public int id { get; set; }
		public string userId { get; set; }
		public int courseId { get; set; }
		public DateTime time { get; set; }
	}
    public class QuestionNotification
    {
        public int id { get; set; }
		public string userId { get; set; }
		public int questionId { get; set; }
		public DateTime time { get; set; }
	}
}