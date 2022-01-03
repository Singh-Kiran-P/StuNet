using Moq;
using Xunit;
using System;
using System.Linq;
using Server.Api.Dtos;
using ChatSample.Hubs;
using FluentAssertions;
using System.Threading;
using Server.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Controllers;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Server.UnitTests
{
    public class AnswerTests : UnitTest 
    {
        private AnswerController createController(UserManager<User> userManager = null) {
            return new AnswerController(_answerRepositoryStub.Object, userManager, _questionRepositoryStub.Object, _hubContextStub.Object, _answerNotificationRepositoryStub.Object, _questionSubscriptionRepositoryStub.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SetAnswerAccepted_withValidAnswerAndPermissions_ReturnsNoContent(bool setAccepted)
        {
            string userId = rand.Next().ToString();
            Answer randomAnswer = createRandomAnswer();
            randomAnswer.question.userId = userId;

            _answerRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(randomAnswer);

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userref", userId) }, "TestAuthType"));
        
            var controller = createController();
            controller.ControllerContext.HttpContext = httpContext;

            var result = await controller.SetAnswerAccepted(rand.Next(), setAccepted);

            result.Should().BeOfType<NoContentResult>();
            randomAnswer.isAccepted.Should().Be(setAccepted);

        }

        [Fact]
        public async Task createAnswer_fromCreateAnswerDto_ReturnsGetAnswerDto()
        {
            string userId = rand.Next().ToString();
            Question randomQuestion = createRandomQuestion();

            CreateAnswerDto answerToCreate = new()  {
                body = rand.Next().ToString(),
                title = rand.Next().ToString(),
                questionId = randomQuestion.id
            };

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userref", userId) }, "TestAuthType")); 

            User randomUser = new() {
				Id = userId,
                Email = rand.Next().ToString()
			};

            var MockManager = GetMockUserManager();
            MockManager.Setup(repo => repo.FindByIdAsync(It.IsAny<string>()))
        		.ReturnsAsync(randomUser);

            _questionRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(randomQuestion);

            _questionSubscriptionRepositoryStub.Setup(repo => repo.GetBySubscribedId(It.IsAny<int>()))
                .ReturnsAsync(new QuestionSubscription[0]);

            _hubContextStub.Setup(c => c.Clients.Group(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), CancellationToken.None))
				.Returns(Task.CompletedTask);

            var controller = createController(MockManager.Object);
            controller.ControllerContext.HttpContext = httpContext;

            var result = await controller.CreateAnswer(answerToCreate);

            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<GetAnswerDto>();;
            var createdAnswer = (result.Result as OkObjectResult).Value as GetAnswerDto;

            answerToCreate.Should().BeEquivalentTo(
                createdAnswer,
                options => options.ComparingByMembers<CreateAnswerDto>().ExcludingMissingMembers()
            );
            createdAnswer.question.Should().BeEquivalentTo(
                randomQuestion,
                options => options.ComparingByMembers<GetQuestionDto>().ExcludingMissingMembers()
            );

            createdAnswer.time.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 0, 500));
        }
    }
}
