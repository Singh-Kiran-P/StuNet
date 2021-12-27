using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IQuestionSubscriptionRepository : IRestfulRepository<QuestionSubscription>
    {
        Task<ICollection<QuestionSubscription>> GetByUserId(string userId);
        Task<ICollection<QuestionSubscription>> GetByQuestionId(int id);
        Task<ICollection<QuestionSubscription>> GetByUserIdAndQuestionIdAsync(string userId, int questionId);
        Task<QuestionSubscription> GetSingleByUserIdAndQuestionIdAsync(string userId, int questionId);
    }
    
    public class PgQuestionSubscriptionRepository : IQuestionSubscriptionRepository
    {
        private readonly IDataContext _context;

        public PgQuestionSubscriptionRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionSubscription>> GetAllAsync()
        {
            return await _context.QuestionSubscriptions
                .ToListAsync();
        }

        public async Task<QuestionSubscription> GetAsync(int id)
        {
            return await _context.QuestionSubscriptions
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<QuestionSubscription>> GetByUserId(string userId)
        {
            return await _context.QuestionSubscriptions
                .Where(s => userId == s.userId)
                .ToListAsync();
        }

        public async Task<ICollection<QuestionSubscription>> GetByQuestionId(int id)
        {
            return await _context.QuestionSubscriptions
                .Where(s => id == s.questionId)
                .ToListAsync();
        }

        public async Task<ICollection<QuestionSubscription>> GetByUserIdAndQuestionIdAsync(string userId, int questionId)
        {
            return await _context.QuestionSubscriptions
                .Where(subscription => subscription.userId == userId && subscription.questionId == questionId)
                .ToListAsync();
        }

        public async Task<QuestionSubscription> GetSingleByUserIdAndQuestionIdAsync(string userId, int questionId)
        {
            return (await GetByUserIdAndQuestionIdAsync(userId, questionId))
                .FirstOrDefault(null);
        }

        public async Task CreateAsync(QuestionSubscription questionsubscription)
        {
            _context.QuestionSubscriptions.Add(questionsubscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuestionSubscription questionsubscription)
        {
            //FIXME: this method doesn't belong here...
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            QuestionSubscription questionsubscription = await _context.QuestionSubscriptions.FindAsync(id);
            if (questionsubscription == null)
            {
                throw new NullReferenceException();
            }

            _context.QuestionSubscriptions.Remove(questionsubscription);
            await _context.SaveChangesAsync();
        }
    }
}
