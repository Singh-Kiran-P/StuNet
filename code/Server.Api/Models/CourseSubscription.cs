using System;

namespace Server.Api.Models
{
    public class CourseSubscription
    {
        public int userId { get; set; }
        public int courseId { get; set; }
        
		public DateTime dateTime { get; set; }
    }
}