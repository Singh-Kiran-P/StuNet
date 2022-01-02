using Server.Api.Models;
using Server.Api.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Server.Api.Repositories
{
    public class PgAnswerNotificationRepository : PgNotificationRepository<AnswerNotification, Answer>
    {

        public PgAnswerNotificationRepository(IDataContext context) : base(context)
        {

        }

        protected override DbSet<AnswerNotification> GetDbSet()
        {
            return _context.AnswerNotifications;
        }

        protected override IIncludableQueryable<AnswerNotification, Answer> GetIncludes()
        {
            return GetDbSet().Include(n => n.answer);
        }
    }
}
