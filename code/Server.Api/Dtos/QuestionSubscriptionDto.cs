using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record getByIdsQuestionSubscriptionDto
    {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public static getByIdsQuestionSubscriptionDto convert(QuestionSubscription subscription)
        {
            return new getByIdsQuestionSubscriptionDto
            {
                id = subscription.id,
                dateTime = subscription.dateTime,
            };
        }
    }

    public record getQuestionSubscriptionDto
    {
        public DateTime dateTime { get; set; }
        public string userId { get; set; }
        public int questionId { get; set; }
        public static getQuestionSubscriptionDto convert(QuestionSubscription subscription)
        {
            return new getQuestionSubscriptionDto
            {
                dateTime = subscription.dateTime,
                userId = subscription.userId,
                questionId = subscription.questionId,
            };
        }
    }

    public record createQuestionSubscriptionDto
    {
        public int questionId { get; set; }
        public static getQuestionSubscriptionDto convert(QuestionSubscription subscription)
        {
            return new getQuestionSubscriptionDto
            {
                userId = subscription.userId,
                questionId = subscription.questionId,
            };
        }
    }
}
