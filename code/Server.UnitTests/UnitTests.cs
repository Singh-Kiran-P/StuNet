using System;
using Xunit;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Repositories;
using Server.Api.Controllers;
using Server.Api.Models;
using Server.Api.Dtos;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ChatSample.Hubs;

namespace Server.UnitTests
{
	public class UnitTest {
        protected readonly Mock<IFieldOfStudyRepository> _FOSRepositoryStub = new();
		protected readonly Mock<IAnswerRepository> _answerRepositoryStub = new();
		protected readonly Mock<IQuestionRepository> _questionRepositoryStub = new();
        protected readonly Mock<ITopicRepository> _topicRepositoryStub = new();
        protected readonly Mock<ICourseRepository> _courseRepositoryStub = new();
        protected readonly Mock<IHubContext<ChatHub>> _hubContextStub = new();
        protected readonly Mock<IEmailSender> _emailSenderStub = new();
		protected readonly Mock<INotificationRepository<QuestionNotification>> _questionNotificationRepositoryStub = new();
		protected readonly Mock<INotificationRepository<AnswerNotification>> _answerNotificationRepositoryStub = new();
		protected readonly Mock<ISubscriptionRepository<CourseSubscription>> _courseSubscriptionRepositoryStub = new();
		protected readonly Mock<ISubscriptionRepository<QuestionSubscription>> _questionSubscriptionRepositoryStub = new();
		protected static Random rand = new();

		public static DateTime randomPassedDate() {
			DateTime start = new DateTime(1995, 1, 1);
			return start.AddDays(rand.Next((DateTime.Today - start).Days));
		}

		public static Course createRandomCourse() {
			return new()
            {
				id = rand.Next(),
                name = rand.Next().ToString(),
                number = rand.Next().ToString(),
                description = rand.Next().ToString(),
                topics = Enumerable.Range(1, 10).Select(_ => new Topic() { name = rand.Next().ToString(), id = rand.Next() } ).ToList(),
                channels = Enumerable.Range(1, 10).Select(_ => new TextChannel() { name = rand.Next().ToString(), id = rand.Next() } ).ToList()
            };
		}

		public static Topic createRandomTopic() {
			int courseId = rand.Next();
			return new()
			{
				id = rand.Next(),
				name = rand.Next().ToString(),
				courseId = courseId,
				course = new Course
				{
					id = courseId,
					name = rand.Next().ToString(),
					number = rand.Next().ToString()
				},
			};
		}


		public static Question createRandomQuestion() {
			int courseId = rand.Next();
			return new()
			{
				id = rand.Next(),
				userId = rand.Next().ToString(),
				courseId = courseId,
				course = new Course
				{
					id = courseId,
					name = rand.Next().ToString(),
					number = rand.Next().ToString()
				},
				title = rand.Next().ToString(),
				body = rand.Next().ToString(),
				topics = Enumerable.Range(1, 10).Select(_ => new Topic {
					id = rand.Next(),
					name = rand.Next().ToString()
				}).ToList(),
				time = randomPassedDate()
			};
		}

		public static Answer createRandomAnswer() {
			Question question = createRandomQuestion();
			return new()
			{
				id = rand.Next(),
				userId = rand.Next().ToString(),
				questionId = question.id,
				question = question,
				title = rand.Next().ToString(),
				body = rand.Next().ToString(),
				time = randomPassedDate(),
				isAccepted = rand.Next(2) == 0,
			};
		}

		public static FieldOfStudy createRandomFieldOfStudy() {
			return new()
			{
				id = rand.Next(),
				fullName = rand.Next().ToString(),
				name = rand.Next().ToString(),
				isBachelor = rand.Next(2) == 0
			};
		}

		public static Mock<UserManager<User>> GetMockUserManager() {
            var userStoreMock = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
	}

}
