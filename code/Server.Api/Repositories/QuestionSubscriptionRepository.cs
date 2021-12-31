using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgQuestionSubscriptionRepository : PgSubscriptionRepository<QuestionSubscription>
    {
        public PgQuestionSubscriptionRepository(IDataContext context) : base(context) { }

        protected override DbSet<QuestionSubscription> GetDbSet()
        {
            return _context.QuestionSubscriptions;
        }
    }
}
