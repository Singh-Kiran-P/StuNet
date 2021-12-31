using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Server.Api.DataBase;
using Server.Api.Models;

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
