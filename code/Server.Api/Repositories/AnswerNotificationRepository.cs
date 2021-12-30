using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Server.Api.DataBase;
using Server.Api.Models;

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
