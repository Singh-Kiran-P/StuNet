using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
	public interface IQuestionNotificationRepository : IInterfaceRepository<QuestionNotification>
    {
		Task<ICollection<QuestionNotification>> getByUserId(string userId);
		Task createAllAync(IEnumerable<QuestionNotification> Notifications);

	}

    public class PgQuestionNotificationRepository : IQuestionNotificationRepository
    {
        private readonly IDataContext _context;

        public PgQuestionNotificationRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<QuestionNotification>> getByUserId(string userId) {
			return await _context.QuestionNotifications
                .Include(n => n.question)
                .Where(s => userId == s.userId)
                .ToListAsync();
		}

        public async Task<IEnumerable<QuestionNotification>> getAllAsync()
        {
            return await _context.QuestionNotifications
                .Include(n => n.question)
                .ToListAsync();
        }

        public async Task<QuestionNotification> getAsync(int id)
        {
            return await _context.QuestionNotifications
                .Include(n => n.question)
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task updateAsync(QuestionNotification questionNotification)
        {
            //FIXME: this method doesn't belong here...
            await _context.SaveChangesAsync();
        }

        public async Task createAsync(QuestionNotification questionNotification)
        {
            _context.QuestionNotifications.Add(questionNotification);
            await _context.SaveChangesAsync();
        }

        public async Task createAllAync(IEnumerable<QuestionNotification> Notifications) {
			_context.QuestionNotifications.AddRange(Notifications);
			await _context.SaveChangesAsync();
		}

        public async Task deleteAsync(int id)
        {
            QuestionNotification questionNotification = await _context.QuestionNotifications.FindAsync(id);
            if (questionNotification == null)
                throw new NullReferenceException();

            _context.QuestionNotifications.Remove(questionNotification);
            await _context.SaveChangesAsync();
        }
    }
}
