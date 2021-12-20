using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IQuestionSubscriptionRepository : IInterfaceRepository<QuestionSubscription>
    {
        Task<ICollection<QuestionSubscription>> getByUserIdAndQuestionIdAsync(string userId, int questionId);
        Task<QuestionSubscription> getSingleByUserIdAndQuestionIdAsync(string userId, int questionId);
    }
}
