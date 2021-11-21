using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;
using System.Linq;

namespace Server.Api.Repositories
{
    public class PgTopicRepository : ITopicRepository
    {
        private readonly IDataContext _context;
        public PgTopicRepository(IDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Topic>> getAllAsync()
        {
            return await _context.Topics
            .Include(t => t.questions)
            .Include(t => t.course)
            .ToListAsync();
        }
        public async Task createAsync(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
        }
        public async Task deleteAsync(int topicId)
        {
            var topicToRemove = await _context.Topics.FindAsync(topicId);
            if (topicToRemove == null)
                throw new NullReferenceException();
            
            _context.Topics.Remove(topicToRemove);
            await _context.SaveChangesAsync();
        }
        public async Task<Topic> getAsync(int id)
        {
			return await _context.Topics
			.Include(t => t.questions)
			.Include(t => t.course)
			.Where(t => t.id == id).FirstOrDefaultAsync();

		}

        public async Task updateAsync(Topic topic)
        {
            var topicToUpdate = await _context.Topics.FindAsync(topic.id);
            if (topicToUpdate == null)
                throw new NullReferenceException();
            topicToUpdate.name = topic.name;
            await _context.SaveChangesAsync();
        }
    }
}