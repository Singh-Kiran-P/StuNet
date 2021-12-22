using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IQuestionRepository : IInterfaceRepository<Question>
    {
        Task<IEnumerable<Question>> getByCourseIdAsync(int courseId);
        //  Task<IEnumerable<Question>> getAllAsync();
        //  Task<User> getAsync(int id);
        //  Task createAsync(Question question);
        //  Task updateAsync(Question question);
        //  Task deleteAsync(int questionId);
    }
}
