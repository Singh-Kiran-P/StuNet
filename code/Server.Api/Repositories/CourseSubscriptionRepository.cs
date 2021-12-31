using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgCourseSubscriptionRepository : PgSubscriptionRepository<CourseSubscription>
    {
        public PgCourseSubscriptionRepository(IDataContext context) : base(context) { }

        protected override DbSet<CourseSubscription> GetDbSet()
        {
            return _context.CourseSubscriptions;
        }
    }
}
