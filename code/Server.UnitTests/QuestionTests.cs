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

    public class QuestionTests : UnitTest
    {

        private QuestionController createController(UserManager<User> userManager = null) 
        {
            return new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, userManager, _hubContextStub.Object, _notificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);
        }

        [Fact]
        public async Task GetQuestion_InvalidId_ReturnsNotFound() 
        {
            // Arrange
            _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

        	var controller = createController();

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
			User randomUser = new()
			{
				Id = rand.Next().ToString()
			};

            var MockManager = GetMockUserManager();
			MockManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
        		.ReturnsAsync(randomUser);

            _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync(question);

        	var controller = createController(MockManager.Object);

            // Act
            var result = await controller.GetQuestion(rand.Next());

            // Assert
            ((result.Result as OkObjectResult).Value as questionDto).Should()
            .BeEquivalentTo(question, options => options.ComparingByMembers<questionDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task CreateQuestion_FromCreateQuestionDto_ReturnsCreatedItem()
        {
			Topic randomTopic = createRandomTopic();
            _topicRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync(randomTopic);

			Course randomCourse = createRandomCourse();
        	_courseRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        		.ReturnsAsync(randomCourse);

			User randomUser = new()
			{
				Id = rand.Next().ToString()
			};
			UserHandler.ConnectedIds[randomUser.Id] = "";
			var MockManager = GetMockUserManager();
			MockManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
        		.ReturnsAsync(randomUser);

			createQuestionDto questionToCreate = new()
        	{
        		courseId = rand.Next(),
        		title = rand.Next().ToString(),
        		body = rand.Next().ToString(),
        		topicIds = Enumerable.Range(1, 10).Select(_ => rand.Next()).ToList<int>()
        	};

            _courseSubscriptionRepositoryStub.Setup(repo => repo.getByCourseId(It.IsAny<int>()))
                .ReturnsAsync(new CourseSubscription[0]);

			_hubContextStub.Setup(c => c.Groups.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).
				Returns(Task.CompletedTask);

			_hubContextStub.Setup(c => c.Clients.Group(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), CancellationToken.None))
				.Returns(Task.CompletedTask);

			var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("username", "") }, "TestAuthType"));


			var controller = createController(MockManager.Object);
			controller.ControllerContext.HttpContext = httpContext;

            var result = await controller.CreateQuestion(questionToCreate);


        	var createdQuestion = (result.Result as OkObjectResult).Value as questionDto;
        	questionToCreate.Should().BeEquivalentTo(
        		createdQuestion,
        		options => options.ComparingByMembers<createQuestionDto>().ExcludingMissingMembers()
        	);

        	createdQuestion.id.Should().NotBe(null);
        	createdQuestion.topics.Should().NotBeNullOrEmpty();
        	foreach (getOnlyTopicDto t in createdQuestion.topics)
        	{
        		t.Should().BeEquivalentTo(
        			randomTopic,
        			options => options.ComparingByMembers<getOnlyTopicDto>().ExcludingMissingMembers()
        		);
        	}
        	createdQuestion.time.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 0, 500)); // 500ms
        }

        [Fact]
        public async Task UpdateQuestion_InvalidId_ReturnsNotFound()
        {
            _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

            var controller = createController();

            var result = await controller.UpdateQuestion(rand.Next(), null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateQuestion_WithExistingItem_ReturnsNoContent()
        {

            createQuestionDto randomQuestion = new()
            {
                courseId = rand.Next(),
                title = rand.Next().ToString(),
                body = rand.Next().ToString(),
                topicIds = Enumerable.Range(1, 10).OrderBy(_ => rand.Next()).ToList<int>()
            };

            _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)createRandomQuestion());

        	var controller = createController();

            var result = await controller.UpdateQuestion(rand.Next(), randomQuestion);

            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task DeleteQuestion_InvalidId_ReturnsNotFound()
        {
            _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

            var controller = createController();

            var result = await controller.DeleteQuestion(rand.Next());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteQuestion_WithExistingItem_ReturnsNoContent()
        {

            _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)createRandomQuestion());

        	var controller = createController();

            var result = await controller.DeleteQuestion(rand.Next());

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
