using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record GetCourseSubscriptionDto
    {
        public int courseId { get; set; }
        public string userId { get; set; }
        public DateTime dateTime { get; set; }
        
        public static GetCourseSubscriptionDto Convert(CourseSubscription subscription)
        {
            return new GetCourseSubscriptionDto
            {
                userId = subscription.userId,
                dateTime = subscription.dateTime,
                courseId = subscription.subscribedItemId
            };
        }
    }

    public record GetByIdsCourseSubscriptionDto
    {
        public int id { get; set; }
        public DateTime dateTime { get; set; }

        public static GetByIdsCourseSubscriptionDto Convert(CourseSubscription subscription)
        {
            return new GetByIdsCourseSubscriptionDto {
                id = subscription.id,
                dateTime = subscription.dateTime,
            };
        }
    }

    public record CreateCourseSubscriptionDto
    {
        public int courseId { get; set; }

        public static GetCourseSubscriptionDto Convert(CourseSubscription subscription)
        {
            return new GetCourseSubscriptionDto {
                userId = subscription.userId,
                courseId = subscription.subscribedItemId,
            };
        }
    }
}
