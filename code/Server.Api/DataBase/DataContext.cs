// https://henriquesd.medium.com/entity-framework-core-relationships-with-fluent-api-8f741c57b881
// https://www.learnentityframeworkcore.com/configuration/one-to-many-relationship-configuration
// https://code-maze.com/migrations-and-seed-data-efcore/
// https://dejanstojanovic.net/aspnet/2020/july/seeding-data-with-entity-framework-core-using-migrations/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Api.Models;
using Server.Api.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;




namespace Server.Api.DataBase
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityUserRole<string>> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<FieldOfStudy> FieldOfStudies { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TextChannel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }

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
			// modelBuilder.Entity<Answer>()
            // .HasOne(a => a.user)
            // .WithMany(u => u.answers)
            // .HasForeignKey(a => a.userId);

            createCourse(modelBuilder);
            createFieldOfStudy(modelBuilder);
            createUsers(modelBuilder);
        }


        private void createUsers(ModelBuilder modelBuilder)
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
            var defaultStudent = new Student()
            {
                FieldOfStudyId = 1,
                Email = "student@student.uhasselt.be",
                NormalizedEmail = "STUDENT@STUDENT.UHASSELT.BE",
                UserName = "student@student.uhasselt.be",
                NormalizedUserName = "STUDENT@STUDENT.UHASSELT.BE",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "GSXIEJ7H7DWJTSRP24CU5DWFJV4WNFAI",
                Id = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
                PasswordHash = "AQAAAAEAACcQAAAAEM3NAKXyohZdXCFtacPu/m8XMK+7VbOGSSePxwzsA+RcDlg1m9p/5RWvBSJtrgNrjQ==", //abc123
                ConcurrencyStamp = "de4df913-7e5b-4406-b710-ea134f7b4a43",
            };
            var defaultProf = new Professor()
            {
                Email = "prof@uhasselt.be",
                NormalizedEmail = "PROF@UHASSELT.BE",
                UserName = "prof@uhasselt.be",
                NormalizedUserName = "PROF@UHASSELT.BE",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "TZEZOM5SNMCQIPT4UPQPHWDJZVAZDIQ2",
                Id = "7d2b412e-7de8-4341-90f4-49b741e83466",
                PasswordHash = "AQAAAAEAACcQAAAAEJCOeIX31jPsOvnmQsfwN+7lRjUAAGFJ8ALpjqPTTXjIT9AbdDcr5vJkgK2gsVqK7A==", //abc123
                ConcurrencyStamp = "83fe0b78-26c9-4c24-a39c-140ec82493b8",
            };

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                defaultUser
             );
            modelBuilder.Entity<Student>().HasData(
                defaultStudent
            );
            modelBuilder.Entity<Professor>().HasData(
                defaultProf
            );
        }

        private void createFieldOfStudy(ModelBuilder modelBuilder){
            var fos1 = new FieldOfStudy() {
                id = 1,
                fullName = "INF-BACH-1",
                name = "INF",
                isBachelor = true,
                year = 1,
            };
            modelBuilder.Entity<FieldOfStudy>().HasData(fos1);
        }

        private void createCourse(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>()
                 .HasOne(t => t.course)
                 .WithMany(c => c.topics)
                 .HasForeignKey(t => t.courseId);

            modelBuilder.Entity<TextChannel>()
                 .HasOne(c => c.course)
                 .WithMany(c => c.channels)
                 .HasForeignKey(c => c.courseId);

            var course1 = new Course()
            {
                id = 1,
                name = "testCourse",
                number = "testCourse",
            };
            var topic1 = new Topic
            {
                id = 1,
                name = "testTopic",
                courseId = course1.id,
            };
			var channel1 = new TextChannel
			{
				id = 1,
				name = "testChannel",
                courseId = course1.id,
                messages = new List<Message>()
			};

			modelBuilder.Entity<Course>().HasData(course1);
            modelBuilder.Entity<Topic>().HasData(topic1);
            modelBuilder.Entity<TextChannel>().HasData(channel1);
        }
    }
}
