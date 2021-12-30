using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface ICourseRepository : IInterfaceRepository<Course>
    {
        Task<Course> getByNameAsync(string name);        
        Task<Course> getByCourseMail(string courseMail);        

    }
}
