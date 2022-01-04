using System;
using System.Linq;
using Server.Api.Models;
using Server.Api.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Api.Repositories
{
    public interface ITopicRepository : IRestfulRepository<Topic>
    {

    }

    public class PgTopicRepository : ITopicRepository
    {
        private readonly IDataContext _context;

        public PgTopicRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await _context.Topics
                .Include(t => t.questions)
                .ThenInclude(q => q.topics)
                .Include(t => t.course)
                .ToListAsync();
        }

        public async Task<Topic> GetAsync(int id)
        {
            return await _context.Topics
                .Where(t => t.id == id)
                .Include(t => t.questions)
                .ThenInclude(q => q.topics)
                .Include(t => t.course)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Topic topic)
        {
            var topicToUpdate = await _context.Topics.FindAsync(topic.id);
            if (topicToUpdate == null) throw new NullReferenceException();
            topicToUpdate.name = topic.name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int topicId)
        {
            var topicToRemove = await _context.Topics.FindAsync(topicId);
            if (topicToRemove == null) throw new NullReferenceException();
            _context.Topics.Remove(topicToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
