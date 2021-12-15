using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;
using System.Linq;

namespace Server.Api.Repositories
{
    public class pgMessageRepository
    {
        private readonly IDataContext _context;
        public pgMessageRepository(IDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Message>> getAllAsync(int channelId)
        {
            return await _context.Messages
			.Where(m => m.channelId == channelId)
            .ToListAsync();
        }
        public async Task createAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}