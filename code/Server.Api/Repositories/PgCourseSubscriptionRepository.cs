using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgCourseSubscriptionRepository : ICourseSubscriptionRepository
    {
        private readonly IDataContext _context;

        public PgCourseSubscriptionRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<CourseSubscription>> getByUserIdAndCourseIdAsync(int userId, int courseId)
        {
            return await _context.CourseSubscriptions
                .Where(s => s.userId == userId && s.courseId == courseId).ToListAsync()
                ;
        }

        public async Task<CourseSubscription> getSingleByUserIdAndCourseIdAsync(int userId, int courseId)
        {
            return (await getByUserIdAndCourseIdAsync(userId, courseId)).FirstOrDefault(null);
        }

        public async Task<IEnumerable<CourseSubscription>> getAllAsync()
        {
            //TODO: test function
            return await _context.CourseSubscriptions
                .ToListAsync();
        }

        public async Task<CourseSubscription> getAsync(int id)
        {
            //TODO: this function shouldn't be here
            return await _context.CourseSubscriptions
                // .Where(c => c.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task updateAsync(CourseSubscription coursesubscription)
        {
            //TODO: test function
            await _context.SaveChangesAsync();
        }

        public async Task createAsync(CourseSubscription coursesubscription)
        {
            //TODO: test function
            _context.CourseSubscriptions.Add(coursesubscription);
            await _context.SaveChangesAsync();
        }

        public async Task deleteAsync(int id)
        {
            //TODO: test function
            CourseSubscription coursesubscription = await _context.CourseSubscriptions.FindAsync(id);
            if (coursesubscription == null)
                throw new NullReferenceException();

            _context.CourseSubscriptions.Remove(coursesubscription);
            await _context.SaveChangesAsync();
        }
    }
}
