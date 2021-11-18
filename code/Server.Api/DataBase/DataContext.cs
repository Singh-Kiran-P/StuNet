using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Api.Models;

namespace Server.Api.DataBase
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityUserRole<string>> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<FieldOfStudy> FieldOfStudies { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });

            var defaultUser = new User()
            {

                Email = "xxxx@example.com",
                NormalizedEmail = "XXXX@EXAMPLE.COM",
                UserName = "Owner",
                NormalizedUserName = "OWNER",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "96bd606a-2d23-4cc7-a929-e380d181d0e6",
                Id = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                PasswordHash = "AQAAAAEAACcQAAAAEGpaVsQ7M5pomvpiM9HvM4/i8tDIyNp2hU9LY8dCggz4FdHwnqhRFwyW5/zNJnN4aw==",
                ConcurrencyStamp = "687a9571-1098-4e18-bedc-f7075207a573",
            };

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                defaultUser
             );

        }

    }
}