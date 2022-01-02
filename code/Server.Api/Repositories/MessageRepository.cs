using System.Linq;
using Server.Api.Models;
using Server.Api.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Api.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllAsync(int channelId);
        Task CreateAsync(Message message);
    }

    public class PgMessageRepository : IMessageRepository
    {
        private readonly IDataContext _context;

        public PgMessageRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllAsync(int channelId)
        {
            return await _context.Messages
                .Where(m => m.channelId == channelId)
                .ToListAsync();
        }

        public async Task CreateAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
