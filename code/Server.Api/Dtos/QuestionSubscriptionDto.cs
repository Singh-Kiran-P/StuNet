using System;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record GetQuestionSubscriptionDto
    {
        public string userId { get; set; }
        public int questionId { get; set; }
        public DateTime dateTime { get; set; }

        public static GetQuestionSubscriptionDto Convert(QuestionSubscription subscription)
        {
            return new GetQuestionSubscriptionDto {
                userId = subscription.userId,
                dateTime = subscription.dateTime,
                questionId = subscription.subscribedItemId
            };
        }
    }

    public record GetByIdsQuestionSubscriptionDto
    {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        
        public static GetByIdsQuestionSubscriptionDto Convert(QuestionSubscription subscription)
        {
            return new GetByIdsQuestionSubscriptionDto {
                id = subscription.id,
                dateTime = subscription.dateTime
            };
        }
    }
    public record CreateQuestionSubscriptionDto
    {
        public int questionId { get; set; }

        public static GetQuestionSubscriptionDto Convert(QuestionSubscription subscription)
        {
            return new GetQuestionSubscriptionDto {
                userId = subscription.userId,
                questionId = subscription.subscribedItemId
            };
        }
    }
}
