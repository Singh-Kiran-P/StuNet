

using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public class getByIdsCourseSubscriptionDto
    {        
		public DateTime dateTime { get; set; }
        public static getByIdsCourseSubscriptionDto convert(CourseSubscription subscription) {
			return new getByIdsCourseSubscriptionDto
			{
				dateTime = subscription.dateTime,
			};
		}
    }
}
