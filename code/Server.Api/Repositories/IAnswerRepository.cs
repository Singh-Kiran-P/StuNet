using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IAnswerRepository : IRestfulRepository<Answer>
    {
        Task<IEnumerable<Answer>> getByQuestionId(int questionId);
    }
}
