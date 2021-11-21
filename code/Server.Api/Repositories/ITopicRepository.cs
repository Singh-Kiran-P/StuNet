using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface ITopicRepository: IInterfaceRepository<Topic>
    {
        //  Task<IEnumerable<Topic>> getAllAsync();
        //  Task<Topic> getAsync(int id);
        //  Task createAsync(Topic topic);
        //  Task updateAsync(Topic topic);
        //  Task deleteAsync(int topicId);
    }
}