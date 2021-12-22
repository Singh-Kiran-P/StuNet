using System;

namespace Server.Api.Models
{
    public class CourseSubscription
    {
        public int id { get; set; }
        public string userId { get; set; }
        public int courseId { get; set; }

        public DateTime dateTime { get; set; }
    }
}
