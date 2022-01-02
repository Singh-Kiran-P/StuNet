using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Api.Repositories
{
    public interface IRestfulRepository<T>
    {
        Task<T> GetAsync(int id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
