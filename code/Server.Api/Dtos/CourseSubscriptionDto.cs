

using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public class getByIdsCourseSubscriptionDto
    {  
        public int id { get; set; }      
		public DateTime dateTime { get; set; }
        public static getByIdsCourseSubscriptionDto convert(CourseSubscription subscription)
        {
			return new getByIdsCourseSubscriptionDto
			{
                id = subscription.id,
				dateTime = subscription.dateTime,
			};
		}
    }

    public class getCourseSubscriptionDto
    {  
		public DateTime dateTime { get; set; }
        public int userId { get; set; }      
        public int courseId { get; set; }      
        public static getCourseSubscriptionDto convert(CourseSubscription subscription)
        {
			return new getCourseSubscriptionDto
			{
				dateTime = subscription.dateTime,
                userId = subscription.userId,
                courseId = subscription.courseId,
			};
		}
    }

    public class createCourseSubscriptionDto
    {  
        public int userId { get; set; }      
        public int courseId { get; set; }      
        public static getCourseSubscriptionDto convert(CourseSubscription subscription)
        {
			return new getCourseSubscriptionDto
			{
                userId = subscription.userId,
                courseId = subscription.courseId,
			};
		}
    }
}
