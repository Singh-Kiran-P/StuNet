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
    public interface ISubscriptionRepository<T> : IRestfulRepository<T> where T : Subscription 
    {
        Task<ICollection<T>> GetByUserId(string userId);
        Task<ICollection<T>> GetBySubscribedId(int id);
        Task<ICollection<T>> GetByUserIdAndSubscribedIdAsync(string userId, int subscribedId);
        Task<T> GetSingleByUserIdAndSubscribedIdAsync(string userId, int SubscribedId);
    }

    public abstract class PgSubscriptionRepository<T, V> : ISubscriptionRepository<T> where T : Subscription
    {
        protected readonly IDataContext _context;

        public PgSubscriptionRepository(IDataContext context)
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

        public async Task<ICollection<T>> GetBySubscribedId(int subscribedId) 
        {
            return await GetIncludes()
                .Where(s => subscribedId == s.subscribedItemId)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetByUserIdAndSubscribedIdAsync(string userId, int subscribedId)
        {
            return await GetIncludes()
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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T subscription = await GetDbSet().FindAsync(id);
            if (subscription == null) throw new NullReferenceException();
            GetDbSet().Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
