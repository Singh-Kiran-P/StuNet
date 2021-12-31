using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface ISubscriptionRepository<T> : IRestfulRepository<T> where T : Subscription 
    {
        Task<ICollection<T>> GetByUserId(string userId);
        Task<ICollection<T>> GetBySubscribedId(int id);
        Task<ICollection<T>> GetByUserIdAndSubscribedIdAsync(string userId, int subscribedId);
        Task<T> GetSingleByUserIdAndSubscribedIdAsync(string userId, int SubscribedId);
    }

    public abstract class PgSubscriptionRepository<T> : ISubscriptionRepository<T> where T : Subscription
    {
        protected readonly IDataContext _context;

        public PgSubscriptionRepository(IDataContext context)
        {
            _context = context;
        }

        protected abstract DbSet<T> GetDbSet();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetDbSet()
                .ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await GetDbSet()
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<T>> GetByUserId(string userId)
        {
            return await GetDbSet()
                .Where(s => userId == s.userId)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetBySubscribedId(int subscribedId) 
        {
            return await GetDbSet()
                .Where(s => subscribedId == s.subscribedItemId)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetByUserIdAndSubscribedIdAsync(string userId, int subscribedId)
        {
            return await GetDbSet()
                .Where(s => subscribedId == s.subscribedItemId && s.userId == userId)
                .ToListAsync();
        }

        public async Task<T> GetSingleByUserIdAndSubscribedIdAsync(string userId, int SubscribedId)
        {
            return (await GetByUserIdAndSubscribedIdAsync(userId, SubscribedId))
                .FirstOrDefault(null);
        }

        public async Task CreateAsync(T subscription)
        {
            GetDbSet().Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T subscription)
        {
            //FIXME: this method doesn't belong here...
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T subscription = await GetDbSet().FindAsync(id);
            if (subscription == null)
            {
                throw new NullReferenceException();
            }
            GetDbSet().Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
