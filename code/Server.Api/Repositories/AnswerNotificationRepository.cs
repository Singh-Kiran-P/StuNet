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

    public class PgAnswerNotificationRepository : PgNotificationRepository<AnswerNotification, Answer>
    {

        public PgAnswerNotificationRepository(IDataContext context) : base(context) { }

        protected override DbSet<AnswerNotification> getDbSet()
        {
            return _context.AnswerNotifications;
        }

        protected override IIncludableQueryable<AnswerNotification, Answer> getIncludes()
        {
            return getDbSet().Include(n => n.answer);
        }
    }
}
