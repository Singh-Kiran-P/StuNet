using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Api.Repositories
{
    public interface IInterfaceRepository<T>
    {
        Task<T> getAsync(int id);
        Task<IEnumerable<T>> getAllAsync();
        Task createAsync(T item);
        Task updateAsync(T item);
        Task deleteAsync(int id);
    }
}
