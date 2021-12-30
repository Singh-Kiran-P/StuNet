using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgQuestionSubscriptionRepository : IQuestionSubscriptionRepository
    {
        private readonly IDataContext _context;

        public PgQuestionSubscriptionRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<QuestionSubscription>> getByUserId(string userId)
        {
            return await _context.QuestionSubscriptions.Where(s => userId == s.userId).ToListAsync();
        }
        public async Task<ICollection<QuestionSubscription>> getByUserIdAndQuestionIdAsync(string userId, int questionId)
        {
            return await _context.QuestionSubscriptions
                .Where(subscription => subscription.userId == userId && subscription.questionId == questionId).ToListAsync()
                ;
        }

        public async Task<QuestionSubscription> getSingleByUserIdAndQuestionIdAsync(string userId, int questionId)
        {
            return (await getByUserIdAndQuestionIdAsync(userId, questionId)).FirstOrDefault(null);
        }

        public async Task<IEnumerable<QuestionSubscription>> getAllAsync()
        {
            return await _context.QuestionSubscriptions
                .ToListAsync();
        }

        public async Task<QuestionSubscription> getAsync(int id)
        {
            return await _context.QuestionSubscriptions
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task updateAsync(QuestionSubscription questionsubscription)
        {
            //FIXME: this method doesn't belong here...
            await _context.SaveChangesAsync();
        }

        public async Task createAsync(QuestionSubscription questionsubscription)
        {
            _context.QuestionSubscriptions.Add(questionsubscription);
            await _context.SaveChangesAsync();
        }

        public async Task deleteAsync(int id)
        {
            QuestionSubscription questionsubscription = await _context.QuestionSubscriptions.FindAsync(id);
            if (questionsubscription == null)
                throw new NullReferenceException();

            _context.QuestionSubscriptions.Remove(questionsubscription);
            await _context.SaveChangesAsync();
        }
    }
}