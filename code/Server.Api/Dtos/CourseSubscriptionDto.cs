

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
}
