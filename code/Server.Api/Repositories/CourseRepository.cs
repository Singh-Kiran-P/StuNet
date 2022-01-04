using System;
using System.Linq;
using Server.Api.Models;
using Server.Api.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Api.Repositories
{
    public interface ICourseRepository : IRestfulRepository<Course>
    {
        Task<Course> GetByNameAsync(string name);
        Task<Course> GetByCourseEmailAsync(string courseMail);
        Task<IEnumerable<Course>> GetAllByProfEmailAsync(string email);

    }

    public class PgCourseRepository : ICourseRepository
    {
        private readonly IDataContext _context;

        public PgCourseRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                .Include(c => c.topics)
                .Include(c => c.channels).ThenInclude(c => c.messages)
                .ToListAsync();
        }

        public async Task<Course> GetAsync(int id)
        {
            return await _context.Courses
                .Where(c => c.id == id)
                .Include(c => c.topics)
                .Include(c => c.channels).ThenInclude(c => c.messages)
                .FirstOrDefaultAsync();
        }

        public async Task<Course> GetByNameAsync(string name)
        {
            return await _context.Courses
                .Where(c => c.name == name)
                .Include(c => c.topics)
                .Include(c => c.channels).ThenInclude(c => c.messages)
                .FirstOrDefaultAsync();
        }

        public async Task<Course> GetByCourseEmailAsync(string courseMail)
        {
            return await _context.Courses
                .Where(c => c.courseEmail == courseMail)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Course>> GetAllByProfEmailAsync(string profEmail)
        {
            return await _context.Courses
                .Where(c => c.profEmail == profEmail)
                .ToListAsync();
        }

        public async Task CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            Course courseToUpdate = await _context.Courses.FindAsync(course.id);
            if (courseToUpdate == null) throw new NullReferenceException();
            courseToUpdate.name = course.name;
            courseToUpdate.number = course.number;
            courseToUpdate.profEmail = course.profEmail;
            courseToUpdate.courseEmail = course.courseEmail;
            courseToUpdate.description = course.description;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Course course = await _context.Courses.FindAsync(id);
            if (course == null) throw new NullReferenceException();
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
