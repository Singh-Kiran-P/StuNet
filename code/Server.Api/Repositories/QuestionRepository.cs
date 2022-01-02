using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IQuestionRepository : IRestfulRepository<Question>
    {
        Task<IEnumerable<Question>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<Question>> GetAskedByUserId(string userId);
    }

    public class PgQuestionRepository : IQuestionRepository
    {
        private readonly IDataContext _context;

        public PgQuestionRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _context.Questions
                .Include(q => q.topics)
                // .Include(q => q.user)
                .Include(q => q.course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetAskedByUserId(string userId)
        {
            return await _context.Questions
            .Where(q => q.userId == userId)
            .Include(q => q.topics)
            .Include(q => q.course)
            .ToListAsync();
        }

        public async Task<Question> GetAsync(int id)
        {
            return await _context.Questions
                .Where(q => q.id == id)
                .Include(q => q.topics)
                // .Include(q => q.user)
                .Include(q => q.course)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Question>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Questions
                .Include(q => q.course)
                .Where(q => q.course.id == courseId)
                .Include(q => q.topics)
                // .Include(q => q.user)
                .ToListAsync();
        }

        public async Task CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Question question)
        {
            var questionToUpdate = await _context.Questions.FindAsync(question.id);
            if (questionToUpdate == null)
            {
                throw new NullReferenceException();
            }
            questionToUpdate.title = question.title;
            questionToUpdate.body = question.body;
            questionToUpdate.topics = question.topics;
            questionToUpdate.time = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int questionId)
        {
            var questionToRemove = await _context.Questions.FindAsync(questionId);
            if (questionToRemove == null)
            {
                throw new NullReferenceException();
            }

            _context.Questions.Remove(questionToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
