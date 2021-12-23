using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public interface ICourseSubscriptionRepository : IRestfulRepository<CourseSubscription>
    {
        Task<ICollection<CourseSubscription>> getByUserId(string userId);
        Task<ICollection<CourseSubscription>> getByCourseId(int id);
        Task<ICollection<CourseSubscription>> getByUserIdAndCourseIdAsync(string userId, int courseId);
        Task<CourseSubscription> getSingleByUserIdAndCourseIdAsync(string userId, int courseId);
    }

    public class PgCourseSubscriptionRepository : ICourseSubscriptionRepository
    {
        private readonly IDataContext _context;

        public PgCourseSubscriptionRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<CourseSubscription>> getByUserId(string userId)
        {
            return await _context.CourseSubscriptions.Where(s => userId == s.userId).ToListAsync();
        }

        public async Task<ICollection<CourseSubscription>> getByCourseId(int id)
        {
            return await _context.CourseSubscriptions.Where(s => id == s.courseId).ToListAsync();
        }

        public async Task<ICollection<CourseSubscription>> getByUserIdAndCourseIdAsync(string userId, int courseId)
        {
            return await _context.CourseSubscriptions
                .Where(s => s.userId == userId && s.courseId == courseId).ToListAsync();
        }

        public async Task<CourseSubscription> getSingleByUserIdAndCourseIdAsync(string userId, int courseId)
        {
            return (await getByUserIdAndCourseIdAsync(userId, courseId)).FirstOrDefault(null);
        }

        public async Task<IEnumerable<CourseSubscription>> getAllAsync()
        {
            return await _context.CourseSubscriptions
                .ToListAsync();
        }

        public async Task<CourseSubscription> getAsync(int id)
        {
            return await _context.CourseSubscriptions
                .Where(s => s.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task updateAsync(CourseSubscription coursesubscription)
        {
            //FIXME: this method doesn't belong here...
            await _context.SaveChangesAsync();
        }

        public async Task createAsync(CourseSubscription coursesubscription)
        {
            _context.CourseSubscriptions.Add(coursesubscription);
            await _context.SaveChangesAsync();
        }

        public async Task deleteAsync(int id)
        {
            CourseSubscription coursesubscription = await _context.CourseSubscriptions.FindAsync(id);
            if (coursesubscription == null)
            {
                throw new NullReferenceException();
            }
            _context.CourseSubscriptions.Remove(coursesubscription);
            await _context.SaveChangesAsync();
        }
    }
}
