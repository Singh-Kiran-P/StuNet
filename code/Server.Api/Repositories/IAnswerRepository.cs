using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IAnswerRepository : IInterfaceRepository<Answer>
    {
        Task<IEnumerable<Answer>> getByQuestionId(int questionId);
    }
}