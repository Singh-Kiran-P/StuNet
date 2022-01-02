using Server.Api.Models;
using Server.Api.DataBase;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Server.Api.Repositories
{
    public class PgCourseSubscriptionRepository : PgSubscriptionRepository<CourseSubscription, ICollection<Topic>>
    {
        public PgCourseSubscriptionRepository(IDataContext context) : base(context) { }

        protected override DbSet<CourseSubscription> GetDbSet()
        {
            return _context.CourseSubscriptions;
        }

        protected override IIncludableQueryable<CourseSubscription, ICollection<Topic>> GetIncludes()
        {
            return GetDbSet().Include(n => n.subscribedItem).ThenInclude(q => q.topics);
        }
    }
}
