using System.Collections.Generic;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IAnswerRepository : IInterfaceRepository<Answer>
    {
        IEnumerable<Answer> getByQuestionId(int questionId);
    }
}