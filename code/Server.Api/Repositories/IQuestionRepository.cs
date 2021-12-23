using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IQuestionRepository : IInterfaceRepository<Question>
    {
        Task<IEnumerable<Question>> getByCourseIdAsync(int courseId);
    }
}
