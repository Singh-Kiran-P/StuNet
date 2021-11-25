using System;
using System.Linq;
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

        public async Task<IEnumerable<Course>> getAllAsync()
        {
            return await _context.Courses
                .Include(c => c.topics)
                .ToListAsync();
        }

        public async Task<Course> getAsync(int id)
        {
			return await _context.Courses
                .Include(c => c.topics)
                .Include(c => c.questions)
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();
		}

        public async Task updateAsync(Course course)
        {
            Course courseToUpdate = await _context.Courses.FindAsync(course.id);
            if (courseToUpdate == null)
                throw new NullReferenceException();
            courseToUpdate.name = course.name;
            courseToUpdate.number = course.number;
            courseToUpdate.topics = course.topics;
            await _context.SaveChangesAsync();
        }

        public async Task createAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task deleteAsync(int id)
        {
            Course course = await _context.Courses.FindAsync(id);
            if (course == null)
                throw new NullReferenceException();
            
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}