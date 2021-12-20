using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface ICourseSubscriptionRepository : IInterfaceRepository<CourseSubscription>
    {
        Task<ICollection<CourseSubscription>> getByUserIdAndCourseIdAsync(string userId, int courseId);
        Task<CourseSubscription> getSingleByUserIdAndCourseIdAsync(string userId, int courseId);
    }
}
