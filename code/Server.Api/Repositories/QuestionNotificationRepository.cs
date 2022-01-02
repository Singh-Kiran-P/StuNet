using Server.Api.Models;
using Server.Api.DataBase;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Server.Api.Repositories
{
    public class PgQuestionNotificationRepository : PgNotificationRepository<QuestionNotification, ICollection<Topic>>
    {
        public PgQuestionNotificationRepository(IDataContext context) : base(context) { }

        protected override DbSet<QuestionNotification> GetDbSet()
        {
            return _context.QuestionNotifications;
        }

        protected override IIncludableQueryable<QuestionNotification, ICollection<Topic>> GetIncludes()
        {
            return GetDbSet()
                .Include(n => n.question).ThenInclude(q => q.course)
                .Include(n => n.question).ThenInclude(q => q.topics);
        }
    }
}
