using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IUserRepository
    {
         Task<User> getAsync(int id);
         Task<IEnumerable<User>> getAllAsync();
         Task createAsync(User user);
         Task updateAsynct(User user);
         Task deleteAsync(int userId);
    }
}