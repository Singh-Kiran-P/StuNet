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
        public DbSet<CourseSubscription> CourseSubscriptions { get; set; } 
        public DbSet<QuestionSubscription> QuestionSubscriptions { get; set; } 
        public DbSet<CourseNotification> CourseNotifications { get; set; } 

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

            createFieldOfStudy(modelBuilder);
            createUsers(modelBuilder);
            createCourse(modelBuilder);
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
			// modelBuilder.Entity<Topic>()
			//     .HasOne(t => t.course)
			//     .WithMany(c => c.topics)
			//     .HasForeignKey(t => t.courseId);

			// modelBuilder.Entity<Question>()
			// 	.HasOne(q => q.course)
			// 	.WithMany(c => c.questions)
			// 	.HasForeignKey(q => q.courseId);

			// modelBuilder.Entity<TextChannel>()
			//     .HasOne(c => c.course)
			//     .WithMany(c => c.channels)
			//     .HasForeignKey(c => c.courseId);

			var courses = new List<Course> {
				new Course() {
					id = 1,
					name = "Course 1",
					number = "Course 1 number",
				},
				new Course() {
					id = 2,
					name = "Course 2",
					number = "Course 2 number",
				}
			};

			var topics = new List<Topic> {
				new Topic {
					id = 1,
					name = "Topic 1",
					courseId = courses[0].id,
				},
				new Topic {
					id = 2,
					name = "Topic 2",
					courseId = courses[0].id,
				},
				new Topic {
					id = 3,
					name = "Topic 3",
					courseId = courses[0].id,
				},
				new Topic {
					id = 4,
					name = "Topic 4",
					courseId = courses[1].id,
				},
				new Topic {
					id = 5,
					name = "Topic 5",
					courseId = courses[1].id,
				}
			};

			var questions = new List<Question> {
				new Question {
					id = 1,
					userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
					courseId = courses[0].id,
					title = "Question 1, all topics for course",
					body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque dui libero, egestas gravida nisl vitae, luctus ornare felis. Donec eget orci vitae mauris ornare tempor ornare eu felis. Donec commodo nec orci eu lobortis. Nam nec feugiat nibh, quis rhoncus quam. Vivamus sit amet lobortis mi. Nulla bibendum orci ac finibus ultrices. Sed pellentesque quam ac metus elementum, sed gravida sem mollis. Proin facilisis id nisl ut varius. Interdum et malesuada fames ac ante ipsum primis in faucibus. Duis a ipsum fermentum, feugiat lectus eu, sollicitudin quam.",
					// topics = topics.Where(t => t.courseId == courses[0].id).ToList(),
					time = DateTime.UtcNow.AddDays(-1)
				},
				new Question {
					id = 2,
					userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
					courseId = courses[0].id,
					title = "Question 2, single topic",
					body = "Etiam erat dui, cursus vel pulvinar ac, accumsan varius diam. Nullam dignissim efficitur eros, eu semper dui viverra in. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Integer dignissim lectus in convallis aliquam. Aliquam consectetur ligula eget felis ultrices eleifend. Proin eu purus lectus. Phasellus pharetra suscipit tempor. Nullam mollis maximus quam, quis maximus mauris semper tincidunt. Nam venenatis, lorem eu pellentesque posuere, sem elit tempor velit, eu tincidunt felis tellus sed odio. Phasellus tristique maximus sem vitae ultrices. Cras ut pharetra nisl, sed varius tellus.",
					// topics = new List<Topic> {topics[0]},
					time = DateTime.UtcNow.AddMonths(-1)
				},
				new Question {
					id = 3,
					userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
					courseId = courses[1].id,
					title = "Question 3, no topics",
					body = "In bibendum dictum mauris, vitae posuere mi fringilla at. Cras dapibus vestibulum risus eu pretium. In varius sed metus id consequat. Suspendisse at interdum leo, eu pharetra nibh. Sed in sollicitudin nunc, a dapibus elit. Phasellus posuere velit a lacinia mattis. Quisque volutpat magna metus, vel dictum dui porttitor id. Cras eu sapien pulvinar, imperdiet leo vel, tincidunt lorem. Nulla facilisi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus quis odio quam. Fusce nec libero eget arcu gravida faucibus. Sed vulputate porttitor ligula, non posuere lectus iaculis eu. Phasellus rhoncus risus laoreet, laoreet felis eu, bibendum ligula.",
					// topics = new List<Topic>(),
					time = DateTime.UtcNow.AddYears(-1)
				},

			};

			modelBuilder.Entity<Question>()
				.HasMany(q => q.topics)
				.WithMany(t => t.questions)
				.UsingEntity("QuestionTopic", typeof(Dictionary<string, object>),
					r => r.HasOne(typeof(Topic)).WithMany().HasForeignKey("topicId"),
                    l => l.HasOne(typeof(Question)).WithMany().HasForeignKey("questionId"),
                    je => { je.ToTable("questiontopics").HasData(
                            new { questionId = 1, topicId = 1 },
                            new { questionId = 1, topicId = 2 },
                            new { questionId = 1, topicId = 3 },
                            new { questionId = 2, topicId = 1 }
                        );
                    }
                );

			var answers = new List<Answer> {
				new Answer {
					id = 1,
					userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
					questionId = questions[0].id,
					title = "Answer 1",
					body = "answer",
					time = DateTime.UtcNow.AddHours(-5)
				},
                new Answer {
					id = 2,
					userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
					questionId = questions[1].id,
					title = "Answer 2",
					body = "answer",
					time = DateTime.UtcNow.AddHours(-2)
				},
                new Answer {
					id = 3,
					userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
					questionId = questions[1].id,
					title = "Answer 3",
					body = "answer",
					time = DateTime.UtcNow.AddDays(-18)
				},
                new Answer {
					id = 4,
					userId = "c1dae7b7-8094-4e40-b277-82768c5d08d7",
					questionId = questions[2].id,
					title = "Answer 4",
					body = "answer",
					time = DateTime.UtcNow.AddMonths(-11)
				}
			};

			var channel = new TextChannel
			{
				id = 1,
				name = "testChannel",
                courseId = courses[0].id,
			};

			var messages = new List<Message> {
				new Message {
					id = 1,
					channelId = channel.id,
					userMail = "student@student.uhasselt.be",
					body = "Message",
					time = DateTime.UtcNow.AddDays(-1)
				},
				new Message {
					id = 2,
					channelId = channel.id,
					userMail = "prof@uhasselt.be",
					body = "Reply",
					time = DateTime.UtcNow.AddHours(-23)
				}
			};


			modelBuilder.Entity<Course>().HasData(courses);
            modelBuilder.Entity<Topic>().HasData(topics);
            modelBuilder.Entity<Question>().HasData(questions);
            modelBuilder.Entity<Answer>().HasData(answers);
            modelBuilder.Entity<TextChannel>().HasData(channel);
            modelBuilder.Entity<Message>().HasData(messages);
        }
    }
}
