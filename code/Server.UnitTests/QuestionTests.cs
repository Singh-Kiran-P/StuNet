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

    public class QuestionTests
    {
        private readonly Mock<IQuestionRepository> _questionRepositoryStub = new();
        private readonly Mock<ITopicRepository> _topicRepositoryStub = new();
        private readonly Mock<ICourseRepository> _courseRepositoryStub = new();
        private readonly Mock<IHubContext<ChatHub>> _hubContextStub = new();
		private readonly Mock<INotificationRepository<QuestionNotification>> _notificationRepositoryStub = new();
		private readonly Mock<ICourseSubscriptionRepository> _courseSubscriptionRepositoryStub = new();
		private readonly Mock<IQuestionSubscriptionRepository> _questionSubscriptionRepositoryStub = new();
		private readonly Random rand = new();

        private Mock<UserManager<User>> GetMockUserManager() {
            var userStoreMock = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        private Question createRandomQuestion() {

        	DateTime start = new DateTime(1995, 1, 1);
        	int range = (DateTime.Today - start).Days;

        	return new()
        	{
        		id = rand.Next(),
        		userId = null,
        		course = new Course
        		{
        			id = rand.Next(),
        			name = rand.Next().ToString(),
        			number = rand.Next().ToString()
        		},
        		title = rand.Next().ToString(),
        		body = rand.Next().ToString(),
        		topics = Enumerable.Range(1, 10).Select(_ => new Topic {
        			id = rand.Next(),
        			name = rand.Next().ToString()
        		}).ToList(),
        		time = start.AddDays(rand.Next(range))
        	};
        }

        [Fact]
        public async Task GetQuestion_InvalidId_ReturnsNotFound() {

            // Arrange
            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

        	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, GetMockUserManager().Object, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);

            // Act
            var result = await controller.GetQuestion(rand.Next());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetQuestion_validId_ReturnsQuestion()
        {

            // Arrange
            Question question = createRandomQuestion();

            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(question);

        	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, GetMockUserManager().Object, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);

            // Act
            var result = await controller.GetQuestion(rand.Next());

            // Assert
            ((result.Result as OkObjectResult).Value as GetQuestionDto).Should()
            .BeEquivalentTo(question, options => options.ComparingByMembers<GetQuestionDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task CreateQuestion_FromCreateQuestionDto_ReturnsCreatedItem()
        {

            Topic randomTopic = new()
            {
                id = rand.Next(),
                name = rand.Next().ToString(),
                course = null,
                questions = null,
            };

        	Course randomCourse = new()
        	{
        		id = rand.Next(),
        		name = rand.Next().ToString(),
        		number = rand.Next().ToString(),
        	};

			User randomUser = new()
			{
				Id = rand.Next().ToString()
			};
			UserHandler.ConnectedIds[randomUser.Id] = "";

			CreateQuestionDto questionToCreate = new()
        	{
        		courseId = rand.Next(),
        		title = rand.Next().ToString(),
        		body = rand.Next().ToString(),
        		topicIds = Enumerable.Range(1, 10).Select(_ => rand.Next()).ToList<int>()
        	};

            _topicRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(randomTopic);

        	_courseRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
        		.ReturnsAsync(randomCourse);

			var MockManager = GetMockUserManager();
			MockManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
        		.ReturnsAsync(randomUser);

            _courseSubscriptionRepositoryStub.Setup(repo => repo.getByCourseId(It.IsAny<int>()))
                .ReturnsAsync(new CourseSubscription[0]);

			_hubContextStub.Setup(c => c.Groups.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).
				Returns(Task.CompletedTask);

			_hubContextStub.Setup(c => c.Clients.Group(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), CancellationToken.None))
				.Returns(Task.CompletedTask);

			var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("username", "") }, "TestAuthType"));


			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, MockManager.Object, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);
            controller.ControllerContext.HttpContext = httpContext;

            var result = await controller.CreateQuestion(questionToCreate);


        	var createdQuestion = (result.Result as OkObjectResult).Value as GetQuestionDto;
        	questionToCreate.Should().BeEquivalentTo(
        		createdQuestion,
        		options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
        	);

        	createdQuestion.id.Should().NotBe(null);
        	createdQuestion.topics.Should().NotBeNullOrEmpty();
        	foreach (GetPartialTopicDto t in createdQuestion.topics)
        	{
        		t.Should().BeEquivalentTo(
        			randomTopic,
        			options => options.ComparingByMembers<GetPartialTopicDto>().ExcludingMissingMembers()
        		);
        	}
        	createdQuestion.time.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 0, 500)); // 500ms
        }

        [Fact]
        public async Task UpdateQuestion_InvalidId_ReturnsNotFound()
        {
            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

            var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, GetMockUserManager().Object, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);

            var result = await controller.UpdateQuestion(rand.Next(), null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateQuestion_WithExistingItem_ReturnsNoContent()
        {

            CreateQuestionDto randomQuestion = new()
            {
                courseId = rand.Next(),
                title = rand.Next().ToString(),
                body = rand.Next().ToString(),
                topicIds = Enumerable.Range(1, 10).OrderBy(_ => rand.Next()).ToList<int>()
            };

            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)createRandomQuestion());

        	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, GetMockUserManager().Object, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);

            var result = await controller.UpdateQuestion(rand.Next(), randomQuestion);

            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task DeleteQuestion_InvalidId_ReturnsNotFound()
        {
            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

            var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, GetMockUserManager().Object, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);

            var result = await controller.DeleteQuestion(rand.Next());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteQuestion_WithExistingItem_ReturnsNoContent()
        {

            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)createRandomQuestion());

        	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, GetMockUserManager().Object, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);

            var result = await controller.DeleteQuestion(rand.Next());

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
