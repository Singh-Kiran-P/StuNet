// @Kiran @Senn

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.DataBase;
using Server.Api.Models;

namespace Server.Api.Repositories
{
    public class PgFieldOfStudyRepository : IFieldOfStudyRepository
    {
        private readonly IDataContext _context;
        public PgFieldOfStudyRepository(IDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FieldOfStudy>> getAllAsync()
        {
            return await _context.FieldOfStudies.ToListAsync();
        }
        public async Task createAsync(FieldOfStudy fieldOfStudy)
        {
            _context.FieldOfStudies.Add(fieldOfStudy);
            await _context.SaveChangesAsync();
        }
        public async Task deleteAsync(int fieldOfStudyId)
        {
            var fieldOfStudyToRemove = await _context.FieldOfStudies.FindAsync(fieldOfStudyId);
            if (fieldOfStudyToRemove == null)
                throw new NullReferenceException();

            _context.FieldOfStudies.Remove(fieldOfStudyToRemove);
            await _context.SaveChangesAsync();
        }
        public async Task<FieldOfStudy> getAsync(int id)
        {
            return await _context.FieldOfStudies.FindAsync(id);
        }

        public async Task updateAsync(FieldOfStudy fieldOfStudy)
        {
            var fieldOfStudyToUpdate = await _context.FieldOfStudies.FindAsync(fieldOfStudy.id);
            if (fieldOfStudyToUpdate == null)
                throw new NullReferenceException();
            fieldOfStudyToUpdate.fullName = fieldOfStudy.fullName;
            fieldOfStudyToUpdate.name = fieldOfStudy.name;
            fieldOfStudyToUpdate.isBachelor = fieldOfStudy.isBachelor;
            await _context.SaveChangesAsync();
        }

        public async Task<FieldOfStudy> getByFullNameAsync(string fullName)
        {
            return await _context.FieldOfStudies.SingleOrDefaultAsync(fos => fos.fullName == fullName);
        }
    }
}
