using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Api.Models;

namespace Server.Api.DataBase
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
             
        }
        public DbSet<User> Users { get; set; }
        public DbSet<FieldOfStudy> FieldOfStudies { get; set; }

    }
}