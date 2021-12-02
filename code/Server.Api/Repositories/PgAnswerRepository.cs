using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;
using System.Linq;

namespace Server.Api.Repositories
{
    public class PgAnswerRepository : IAnswerRepository
    {
        private readonly IDataContext _context;
        public PgAnswerRepository(IDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Answer>> getAllAsync()
        {
			return await _context.Answers
            .Include(q => q.userId)          
            .Include(q => q.question)          
            .Include(q => q.question.user)          
            .Include(q => q.question.course)          
            .ToListAsync();
		}
        public async Task<Answer> getAsync(int id)
        {
			return await _context.Answers
            .Include(q => q.userId)          
            .Include(q => q.question)          
            .Include(q => q.question.user)          
            .Include(q => q.question.course)
            .Where(q => q.id == id).FirstOrDefaultAsync();
		}
        public async Task createAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }
        public async Task deleteAsync(int answerId)
        {
            var answerToRemove = await _context.Answers.FindAsync(answerId);
            if (answerToRemove == null)
                throw new NullReferenceException();
            
            _context.Answers.Remove(answerToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task updateAsync(Answer answer)
        {
            var answerToUpdate = await _context.Answers.FindAsync(answer.id);
            if (answerToUpdate == null)
                throw new NullReferenceException();
            answerToUpdate.title = answer.title;
            answerToUpdate.body = answer.body;
			answerToUpdate.userId = answer.userId;
			answerToUpdate.question = answer.question;
			answerToUpdate.dateTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Answer> getByQuestionId(int questionId)
        {
            // var answers = _context.Answers.Select(x => x).Where(answer => answer.question.id == questionId);
            return _context.Answers.ToList().Where(answer => answer.question.id == questionId);
            // return answers.ToList();
        }
    }
}