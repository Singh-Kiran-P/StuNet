

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgCourseRepository : ICourseRepository
    {
        private readonly IDataContext _context;

        public PgCourseRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task createAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public Task deleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Course>> getAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public Task<Course> getAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task updateAsync(Course course)
        {
            throw new System.NotImplementedException();
        }
    }
}