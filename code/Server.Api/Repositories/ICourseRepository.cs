using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface ICourseRepository : IRestfulRepository<Course>
    {
        Task<Course> getByNameAsync(string name);
    }
}
