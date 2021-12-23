using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IFieldOfStudyRepository : IRestfulRepository<FieldOfStudy>
    {
        Task<FieldOfStudy> getByFullNameAsync(string fullName);
    }
}
