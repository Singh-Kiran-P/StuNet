using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Api.Models;

namespace Server.Api.DataBase
{
    public interface IDataContext
    {
        DbSet<Answer> Answers { get; set; }
        DbSet<User> Users { get; set;}
        DbSet<Topic> Topics { get; set;}
        DbSet<Question> Questions { get; set;}

        DbSet<IdentityUserRole<string>> Roles { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Professor> Professors { get; set; }
        DbSet<FieldOfStudy> FieldOfStudies { get; set; }
        DbSet<Course> Courses { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
