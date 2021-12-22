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
        DbSet<TextChannel> Channels { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<CourseSubscription> CourseSubscriptions { get; set; } 
        DbSet<QuestionSubscription> QuestionSubscriptions { get; set; } 
        DbSet<AnswerNotification> AnswerNotifications { get; set; } 
        DbSet<QuestionNotification> QuestionNotifications { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
