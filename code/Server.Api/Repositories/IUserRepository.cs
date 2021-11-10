using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IUserRepository: IInterfaceRepository<User>
    {
        //  Task<IEnumerable<User>> getAllAsync();
        //  Task<User> getAsync(int id);
        //  Task createAsync(User user);
        //  Task updateAsync(User user);
        //  Task deleteAsync(int userId);
    }
}