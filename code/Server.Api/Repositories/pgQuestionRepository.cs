using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;
using System.Linq;

namespace Server.Api.Repositories
{
    public class PgQuestionRepository : IQuestionRepository
    {
        private readonly IDataContext _context;
        public PgQuestionRepository(IDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Question>> getAllAsync()
        {
			return await _context.Questions
            .Include(q => q.topics)
            .Include(q => q.user)          
            .Include(q => q.course)          
            .ToListAsync();
		}
        public async Task<Question> getAsync(int id)
        {
			return await _context.Questions
            .Include(q => q.topics)
            .Include(q => q.user)          
            .Include(q => q.course)
            .Where(q => q.id == id).FirstOrDefaultAsync();
		}
        public async Task createAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }
        public async Task deleteAsync(int questionId)
        {
            var questionToRemove = await _context.Questions.FindAsync(questionId);
            if (questionToRemove == null)
                throw new NullReferenceException();
            
            _context.Questions.Remove(questionToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task updateAsync(Question question)
        {
            var questionToUpdate = await _context.Questions.FindAsync(question.id);
            if (questionToUpdate == null)
                throw new NullReferenceException();
            questionToUpdate.title = question.title;
            questionToUpdate.body = question.body;
			questionToUpdate.topics = question.topics;
			questionToUpdate.dateTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}