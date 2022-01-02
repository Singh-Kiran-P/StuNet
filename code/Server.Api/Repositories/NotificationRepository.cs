using System;
using System.Linq;
using Server.Api.Models;
using Server.Api.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Server.Api.Repositories
{
    public interface INotificationRepository<T> : IRestfulRepository<T> where T : Notification
    {
        Task<ICollection<T>> GetByUserId(string userId);
        Task CreateAllAync(IEnumerable<T> Notifications);
    }

    public abstract class PgNotificationRepository<T, V> : INotificationRepository<T> where T : Notification
    {
        protected readonly IDataContext _context;

        public PgNotificationRepository(IDataContext context)
        {
            _context = context;
        }

        protected abstract DbSet<T> GetDbSet();

        protected abstract IIncludableQueryable<T, V> GetIncludes();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetIncludes()
                .ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await GetIncludes()
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<T>> GetByUserId(string userId)
        {
            return await GetIncludes()
                .Where(s => userId == s.userId)
                .ToListAsync();
        }

        public async Task CreateAsync(T notification)
        {
            GetDbSet().Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAllAync(IEnumerable<T> notifications)
        {
            GetDbSet().AddRange(notifications);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T notification)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T notification = await GetDbSet().FindAsync(id);
            if (notification == null) throw new NullReferenceException();
            GetDbSet().Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
