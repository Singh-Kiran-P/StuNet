using System;

namespace Server.Api.Models
{
    public class Subscription
    {
        public int id { get; set; }
        public string userId { get; set; }
        public DateTime dateTime { get; set; }
        public int subscribedItemId { get; set; }
    }

    public class CourseSubscription : Subscription
    {
        public Course subscribedItem { get; set; }
    }

    public class QuestionSubscription : Subscription
    {
        public Question subscribedItem { get; set; }
    }
}
