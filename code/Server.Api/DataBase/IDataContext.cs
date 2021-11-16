using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.Models;

namespace Server.Api.DataBase
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set;}
        DbSet<Question> Questions { get; set;}
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}