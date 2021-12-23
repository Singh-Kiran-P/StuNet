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
    public interface INotificationRepository<T> : IInterfaceRepository<T> where T : Notification
    {
        Task<ICollection<T>> getByUserId(string userId);
        Task createAllAync(IEnumerable<T> Notifications);

    }

    public abstract class PgNotificationRepository<T, V> : INotificationRepository<T> where T : Notification
    {
        protected readonly IDataContext _context;

        public PgNotificationRepository(IDataContext context)
        {
            _context = context;
        }

        protected abstract DbSet<T> getDbSet();

        protected abstract IIncludableQueryable<T, V> getIncludes();

        public async Task<ICollection<T>> getByUserId(string userId)
        {
            return await getIncludes()
                .Where(s => userId == s.userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> getAllAsync()
        {
            return await getIncludes()
                .ToListAsync();
        }

        public async Task<T> getAsync(int id)
        {
            return await getIncludes()
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task updateAsync(T notification)
        {
            //FIXME: this method doesn't belong here...
            await _context.SaveChangesAsync();
        }

        public async Task createAsync(T notification)
        {
            getDbSet().Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task createAllAync(IEnumerable<T> notifications)
        {
            getDbSet().AddRange(notifications);
            await _context.SaveChangesAsync();
        }

        public async Task deleteAsync(int id)
        {
            T notification = await getDbSet().FindAsync(id);
            if (notification == null)
            {
                throw new NullReferenceException();
            }
            getDbSet().Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
