using Moq;
using Xunit;
using System;
using System.Linq;
using Server.Api.Dtos;
using ChatSample.Hubs;
using FluentAssertions;
using System.Threading;
using Server.Api.Models;
using Server.Api.Controllers;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Server.UnitTests
{

    public class QuestionTests : UnitTest
    {

        private QuestionController CreateController(UserManager<User> userManager = null)
        {
            return new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object, userManager, _hubContextStub.Object, _questionNotificationRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object, _emailSenderStub.Object);
        }

        [Fact]
        public async Task GetQuestion_InvalidId_ReturnsNotFound()
        {
            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

        	var controller = CreateController();

            var result = await controller.GetQuestion(rand.Next());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetQuestion_validId_ReturnsQuestion()
        {
            // Arrange
            Question question = createRandomQuestion();
			User randomUser = new() {
				Id = rand.Next().ToString()
			};

            var MockManager = GetMockUserManager();
			MockManager.Setup(repo => repo.FindByIdAsync(It.IsAny<string>()))
        		.ReturnsAsync(randomUser);

            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(question);

        	var controller = CreateController(MockManager.Object);

            var result = await controller.GetQuestion(rand.Next());

            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<GetQuestionDto>().And
            .BeEquivalentTo(question, options => options.ComparingByMembers<GetQuestionDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task CreateQuestion_FromCreateQuestionDto_ReturnsCreatedItem()
        {
			Topic randomTopic = createRandomTopic();
            _topicRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(randomTopic);

			Course randomCourse = createRandomCourse();
        	_courseRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
        		.ReturnsAsync(randomCourse);

			User randomUser = new() {
				Id = rand.Next().ToString()
			};
			UserHandler.ConnectedIds[randomUser.Id] = "";
			var MockManager = GetMockUserManager();
			MockManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
        		.ReturnsAsync(randomUser);

			CreateQuestionDto questionToCreate = new() {
        		courseId = rand.Next(),
        		body = rand.Next().ToString(),
        		title = rand.Next().ToString(),
        		topicIds = Enumerable.Range(1, 10).Select(_ => rand.Next()).ToList<int>()
        	};

            _courseSubscriptionRepositoryStub.Setup(repo => repo.GetBySubscribedId(It.IsAny<int>()))
                .ReturnsAsync(new CourseSubscription[0]);

			_hubContextStub.Setup(c => c.Groups.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).
				Returns(Task.CompletedTask);

			_hubContextStub.Setup(c => c.Clients.Group(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), CancellationToken.None))
				.Returns(Task.CompletedTask);

			var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("username", "") }, "TestAuthType"));

			var controller = CreateController(MockManager.Object);
			controller.ControllerContext.HttpContext = httpContext;

            var result = await controller.CreateQuestion(questionToCreate);

        	result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<GetQuestionDto>();
            var createdQuestion = (result.Result as OkObjectResult).Value as GetQuestionDto;

            createdQuestion.Should().BeEquivalentTo(
        		questionToCreate,
        		options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
        	);
            createdQuestion.id.Should().NotBe(null);
        	createdQuestion.topics.Should().NotBeNullOrEmpty();
        	foreach (GetPartialTopicDto t in createdQuestion.topics) {
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

            var controller = CreateController();

            var result = await controller.UpdateQuestion(rand.Next(), null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateQuestion_WithExistingItem_ReturnsNoContent()
        {

            Question oldQuestion = createRandomQuestion();
            CreateQuestionDto newQuestion = new() {
                courseId = rand.Next(),
                body = rand.Next().ToString(),
                title = rand.Next().ToString(),
                topicIds = Enumerable.Range(1, 10).OrderBy(_ => rand.Next()).ToList<int>()
            };

            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(oldQuestion);

        	var controller = CreateController();

            var result = await controller.UpdateQuestion(rand.Next(), newQuestion);

            result.Should().BeOfType<NoContentResult>();
            oldQuestion.Should().BeEquivalentTo(
                oldQuestion,
                options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
            );
        }

        [Fact]
        public async Task DeleteQuestion_InvalidId_ReturnsNotFound()
        {
            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

            var controller = CreateController();

            var result = await controller.DeleteQuestion(rand.Next());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteQuestion_WithExistingItem_ReturnsNoContent()
        {

            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)createRandomQuestion());

        	var controller = CreateController();

            var result = await controller.DeleteQuestion(rand.Next());

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
