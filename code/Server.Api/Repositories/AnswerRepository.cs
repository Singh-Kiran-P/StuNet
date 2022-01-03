using System;
using System.Linq;
using Server.Api.Models;
using Server.Api.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Api.Repositories
{
    public interface IAnswerRepository : IRestfulRepository<Answer>
    {
        Task<IEnumerable<Answer>> GetByQuestionId(int questionId);

        Task<IEnumerable<Answer>> GetGivenByUserId(string userId);
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
                .Include(a => a.question)
                .Include(a => a.question.course)
                .Include(a => a.question.topics)
                .ToListAsync();
        }

        public async Task<Answer> GetAsync(int id)
        {
            return await _context.Answers
                .Where(a => a.id == id)
                .Include(a => a.question)
                .Include(a => a.question.course)
                .Include(a => a.question.topics)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Answer>> GetGivenByUserId(string userId)
        {
            return await _context.Answers
                .Where(q => q.userId == userId)
                .Include(a => a.question)
                .Include(a => a.question.course)
                .Include(a => a.question.topics)
                .ToListAsync();
        }

        public async Task CreateAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Answer answer)
        {
            var answerToUpdate = await _context.Answers.FindAsync(answer.id);
            if (answerToUpdate == null) throw new NullReferenceException();
            answerToUpdate.time = answer.time;
            answerToUpdate.body = answer.body;
            answerToUpdate.title = answer.title;
            answerToUpdate.userId = answer.userId;
            answerToUpdate.question = answer.question;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Answer>> GetByQuestionId(int questionId)
        {
            return await _context.Answers
                .Include(a => a.question)
                .Include(a => a.question.course)
                .Include(a => a.question.topics)
                .Where(a => a.question.id == questionId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int answerId)
        {
            var answerToRemove = await _context.Answers.FindAsync(answerId);
            if (answerToRemove == null) throw new NullReferenceException();
            _context.Answers.Remove(answerToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
