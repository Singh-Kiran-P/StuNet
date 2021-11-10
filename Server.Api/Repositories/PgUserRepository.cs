using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgUserRepository : IUserRepository
    {
        public Task createAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task deleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> getAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task updateAsynct(User user)
        {
            throw new NotImplementedException();
        }
    }
}