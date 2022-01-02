using Server.Api.Models;
using Server.Api.DataBase;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
            return GetDbSet()
                .Include(n => n.subscribedItem).ThenInclude(q => q.course)
                .Include(n => n.subscribedItem).ThenInclude(q => q.topics);
        }
    }
}
