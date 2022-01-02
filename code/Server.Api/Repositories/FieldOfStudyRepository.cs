using System;
using Server.Api.Models;
using Server.Api.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Api.Repositories
{
    public interface IFieldOfStudyRepository : IRestfulRepository<FieldOfStudy>
    {
        Task<FieldOfStudy> GetByFullNameAsync(string fullName);
    }

    public class PgFieldOfStudyRepository : IFieldOfStudyRepository
    {
        private readonly IDataContext _context;

        public PgFieldOfStudyRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FieldOfStudy>> GetAllAsync()
        {
            return await _context.FieldOfStudies.ToListAsync();
        }

        public async Task<FieldOfStudy> GetAsync(int id)
        {
            return await _context.FieldOfStudies.FindAsync(id);
        }

        public async Task<FieldOfStudy> GetByFullNameAsync(string fullName)
        {
            return await _context.FieldOfStudies.SingleOrDefaultAsync(fos => fos.fullName == fullName);
        }

        public async Task CreateAsync(FieldOfStudy fieldOfStudy)
        {
            _context.FieldOfStudies.Add(fieldOfStudy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int fieldOfStudyId)
        {
            var fieldOfStudyToRemove = await _context.FieldOfStudies.FindAsync(fieldOfStudyId);
            if (fieldOfStudyToRemove == null) throw new NullReferenceException();
            _context.FieldOfStudies.Remove(fieldOfStudyToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FieldOfStudy fieldOfStudy)
        {
            var fieldOfStudyToUpdate = await _context.FieldOfStudies.FindAsync(fieldOfStudy.id);
            if (fieldOfStudyToUpdate == null) throw new NullReferenceException();
            fieldOfStudyToUpdate.name = fieldOfStudy.name;
            fieldOfStudyToUpdate.fullName = fieldOfStudy.fullName;
            fieldOfStudyToUpdate.isBachelor = fieldOfStudy.isBachelor;
            await _context.SaveChangesAsync();
        }
    }
}
