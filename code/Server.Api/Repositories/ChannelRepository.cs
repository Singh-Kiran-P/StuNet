using System;
using System.Linq;
using Server.Api.Models;
using Server.Api.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Api.Repositories
{
    public interface IChannelRepository : IRestfulRepository<TextChannel>
    {
        Task<ICollection<TextChannel>> GetByCourseIdAsync(int courseId);
    }
    
    public class PgChannelRepository : IChannelRepository
    {
        private readonly IDataContext _context;

        public PgChannelRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TextChannel>> GetAllAsync()
        {
            return await _context.Channels
                .Include(c => c.course)
                .Include(c => c.messages)
                .ToListAsync();
        }

        public async Task<TextChannel> GetAsync(int id)
        {
            return await _context.Channels
                .Where(c => c.id == id)
                .Include(c => c.course)
                .Include(c => c.messages)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<TextChannel>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Channels
                .Where(c => c.course.id == courseId)
                .Include(c => c.course)
                .ToListAsync();
        }

        public async Task CreateAsync(TextChannel channel)
        {
            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TextChannel channel)
        {
            var channelToUpdate = await _context.Channels.FindAsync(channel.id);
            if (channelToUpdate == null) throw new NullReferenceException();
            channelToUpdate.name = channel.name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int channelId)
        {
            var channelToRemove = await _context.Channels.FindAsync(channelId);
            if (channelToRemove == null) throw new NullReferenceException();
            _context.Channels.Remove(channelToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
