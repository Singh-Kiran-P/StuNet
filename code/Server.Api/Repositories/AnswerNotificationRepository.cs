using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
	public interface IAnswerNotificationRepository : IInterfaceRepository<AnswerNotification>
    {
		Task<ICollection<AnswerNotification>> getByUserId(string userId);
		Task createAllAync(IEnumerable<AnswerNotification> Notifications);

	}

    public class PgAnswerNotificationRepository : IAnswerNotificationRepository
    {
        private readonly IDataContext _context;

        public PgAnswerNotificationRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AnswerNotification>> getByUserId(string userId) {
			return await _context.AnswerNotifications.Where(s => userId == s.userId).ToListAsync();
		}

        public async Task<IEnumerable<AnswerNotification>> getAllAsync()
        {
            return await _context.AnswerNotifications
                .ToListAsync();
        }

        public async Task<AnswerNotification> getAsync(int id)
        {
            return await _context.AnswerNotifications
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task updateAsync(AnswerNotification AnswerNotification)
        {
            //FIXME: this method doesn't belong here...
            await _context.SaveChangesAsync();
        }

        public async Task createAsync(AnswerNotification AnswerNotification)
        {
            _context.AnswerNotifications.Add(AnswerNotification);
            await _context.SaveChangesAsync();
        }

		public async Task createAllAync(IEnumerable<AnswerNotification> Notifications) {
			_context.AnswerNotifications.AddRange(Notifications);
			await _context.SaveChangesAsync();
		}

        public async Task deleteAsync(int id)
        {
            AnswerNotification AnswerNotification = await _context.AnswerNotifications.FindAsync(id);
            if (AnswerNotification == null)
                throw new NullReferenceException();

            _context.AnswerNotifications.Remove(AnswerNotification);
            await _context.SaveChangesAsync();
        }
    }
}
