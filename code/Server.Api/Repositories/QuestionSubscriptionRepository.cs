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
    public class PgQuestionSubscriptionRepository : PgSubscriptionRepository<QuestionSubscription, ICollection<Topic>>
    {
        public PgQuestionSubscriptionRepository(IDataContext context) : base(context) { }

        protected override DbSet<QuestionSubscription> GetDbSet()
        {
            return _context.QuestionSubscriptions;
        }

        protected override IIncludableQueryable<QuestionSubscription, ICollection<Topic>> GetIncludes()
        {
            return GetDbSet().Include(n => n.subscribedItem).ThenInclude(q => q.course)
                            .Include(n => n.subscribedItem).ThenInclude(q => q.topics);
        }
    }
}
