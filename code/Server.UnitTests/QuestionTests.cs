using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Repositories;
using Server.Api.Controllers;
using Server.Api.Models;
using Server.Api.Dtos;

namespace Server.UnitTests
{
    public class QuestionTests
    {
        [Fact]
        public async Task GetQuestion_InvalidId_ReturnsNotFound(){

            // Arrange
			var questionRepositoryStub = new Mock<IQuestionRepository>();
			questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

			var controller = new QuestionController(questionRepositoryStub.Object, null, null);

			// Act
			var result = await controller.GetQuestion(0);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}
    }
}
