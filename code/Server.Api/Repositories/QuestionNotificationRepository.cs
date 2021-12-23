using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgQuestionNotificationRepository : PgNotificationRepository<QuestionNotification, Question>
    {
        public PgQuestionNotificationRepository(IDataContext context) : base(context) { }

        protected override DbSet<QuestionNotification> getDbSet()
        {
            return _context.QuestionNotifications;
        }

        protected override IIncludableQueryable<QuestionNotification, Question> getIncludes()
        {
            return getDbSet().Include(n => n.question);
        }
    }
}
