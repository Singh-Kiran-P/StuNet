using System.Threading;
using Server.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Server.Api.DataBase
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Topic> Topics { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<TextChannel> Channels { get; set; }
        DbSet<Professor> Professors { get; set; }
        DbSet<FieldOfStudy> FieldOfStudies { get; set; }
        DbSet<IdentityUserRole<string>> Roles { get; set; }
        DbSet<CourseSubscription> CourseSubscriptions { get; set; } 
        DbSet<AnswerNotification> AnswerNotifications { get; set; } 
        DbSet<QuestionSubscription> QuestionSubscriptions { get; set; } 
        DbSet<QuestionNotification> QuestionNotifications { get; set; }
        
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
