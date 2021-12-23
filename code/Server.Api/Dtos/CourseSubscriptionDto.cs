using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record getByIdsCourseSubscriptionDto
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

    public record getCourseSubscriptionDto
    {
        public DateTime dateTime { get; set; }
        public string userId { get; set; }
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

    public record createCourseSubscriptionDto
    {
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
