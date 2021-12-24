using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IAnswerRepository : IRestfulRepository<Answer>
    {
        Task<IEnumerable<Answer>> GetByQuestionId(int questionId);
    }

    public class PgAnswerRepository : IAnswerRepository
    {
        private readonly IDataContext _context;

        public PgAnswerRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _context.Answers
                // .Include(a => a.userId)
                .Include(a => a.question)
                // .Include(a => a.question.user)
                .Include(a => a.question.course)
                .Include(a => a.question.topics)
                .ToListAsync();
        }

        public async Task<Answer> GetAsync(int id)
        {
            return await _context.Answers
                .Where(a => a.id == id)
                // .Include(a => a.userId)
                .Include(a => a.question)
                // .Include(a => a.question.user)
                .Include(a => a.question.course)
                .Include(a => a.question.topics)
                .FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int answerId)
        {
            var answerToRemove = await _context.Answers.FindAsync(answerId);
            if (answerToRemove == null)
                throw new NullReferenceException();

            _context.Answers.Remove(answerToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Answer answer)
        {
            var answerToUpdate = await _context.Answers.FindAsync(answer.id);
            if (answerToUpdate == null)
            {
                throw new NullReferenceException();
            }
            answerToUpdate.title = answer.title;
            answerToUpdate.body = answer.body;
            answerToUpdate.userId = answer.userId;
            answerToUpdate.question = answer.question;
            answerToUpdate.time = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Answer>> GetByQuestionId(int questionId)
        {
            return await _context.Answers
                // .Include(a => a.userId)
                .Include(a => a.question)
                // .Include(a => a.question.user)
                .Include(a => a.question.course)
                .Include(a => a.question.topics)
                .Where(a => a.question.id == questionId)
                .ToListAsync();
        }
    }
}
