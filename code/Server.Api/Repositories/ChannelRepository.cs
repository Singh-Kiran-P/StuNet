using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface IChannelRepository : IRestfulRepository<TextChannel>
    {
        Task<ICollection<TextChannel>> getByCourseIdAsync(int courseId);
    }
    
    public class pgChannelRepository : IChannelRepository
    {
        private readonly IDataContext _context;

        public pgChannelRepository(IDataContext context)
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

        public async Task CreateAsync(TextChannel channel)
        {
            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int channelId)
        {
            var channelToRemove = await _context.Channels.FindAsync(channelId);
            if (channelToRemove == null)
            {
                throw new NullReferenceException();
            }

            _context.Channels.Remove(channelToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<TextChannel> GetAsync(int id)
        {
            return await _context.Channels
                .Where(c => c.id == id)
                .Include(c => c.course)
                .Include(c => c.messages)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<TextChannel>> getByCourseIdAsync(int courseId)
        {
            return await _context.Channels
                .Where(c => c.course.id == courseId)
                .Include(c => c.course)
                .ToListAsync();
        }

        public async Task UpdateAsync(TextChannel channel)
        {
            var channelToUpdate = await _context.Channels.FindAsync(channel.id);
            if (channelToUpdate == null)
            {
                throw new NullReferenceException();
            }
            channelToUpdate.name = channel.name;
            channelToUpdate.course = channel.course;
            channelToUpdate.messages = channel.messages;
            await _context.SaveChangesAsync();
        }
    }
}
