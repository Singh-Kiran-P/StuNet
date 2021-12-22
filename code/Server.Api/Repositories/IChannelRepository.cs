using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IChannelRepository : IInterfaceRepository<TextChannel>
    {
        public Task<ICollection<TextChannel>> getByCourseIdAsync(int courseId);
    }
}
