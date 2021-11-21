using System;
using Xunit;
using Moq;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Repositories;
using Server.Api.Controllers;
using Server.Api.Models;
using Server.Api.Dtos;
using System.Linq;

namespace Server.UnitTests {
    public class QuestionTests {
		private readonly Mock<IQuestionRepository> _questionRepositoryStub = new();
		private readonly Mock<ITopicRepository> _topicRepositoryStub = new();
		private readonly Mock<ICourseRepository> __courseRepositoryStub = new();
		private readonly Random rand = new();

		private Question createRandomQuestion() {

			DateTime start = new DateTime(1995, 1, 1);
			int range = (DateTime.Today - start).Days;           

			return new()
			{
				id = rand.Next(),
				user = null,
				course = new Course
				{
					Id = rand.Next(),
					Name = rand.Next().ToString(),
					Number = rand.Next().ToString()
				},
				title = rand.Next().ToString(),
				body = rand.Next().ToString(),
				topics = null,
				dateTime = start.AddDays(rand.Next(range))
			};
		}

		[Fact]
        public async Task GetQuestion_InvalidId_ReturnsNotFound() {

            // Arrange
			_questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, __courseRepositoryStub.Object);

			// Act
			var result = await controller.GetQuestion(rand.Next());

			// Assert
			result.Result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task GetQuestion_validId_ReturnsQuestion() {

			// Arrange
			Question question = createRandomQuestion();

			_questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
				.ReturnsAsync(question);

			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, __courseRepositoryStub.Object);

			// Act
			var result = await controller.GetQuestion(rand.Next());

			// Assert
			((result.Result as OkObjectResult).Value as Question).Should().BeEquivalentTo(question);
		}

		[Fact]
		public async Task CreateQuestion_FromCreateQuestionDto_ReturnsCreatedItem() {

			Topic randomTopic = new() {
				id = rand.Next(),
				name = rand.Next().ToString(),
				course = null,
				questions = null,
			};

			createQuestionDto questionToCreate = new()
			{
				courseId = rand.Next(),
				title = rand.Next().ToString(),
				body = rand.Next().ToString(),
				topicIds = Enumerable.Range(1, 10).OrderBy(_ => rand.Next()).ToList<int>()
			};

			_topicRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
				.ReturnsAsync(randomTopic);

			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, __courseRepositoryStub.Object);


			var result = await controller.CreateQuestion(questionToCreate);

			var createdQuestion = (result.Result as OkObjectResult).Value as Question;
			questionToCreate.Should().BeEquivalentTo(
				createdQuestion,
				options => options.ComparingByMembers<createQuestionDto>().ExcludingMissingMembers()
			);

			createdQuestion.id.Should().NotBe(null);
			createdQuestion.topics.Should().NotBeNullOrEmpty();
			createdQuestion.topics.Should().OnlyContain(t => t == randomTopic);
			createdQuestion.dateTime.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 0, 0, 500)); // 500ms
		}

		[Fact]
		public async Task UpdateQuestion_InvalidId_ReturnsNotFound() {
			_questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, __courseRepositoryStub.Object);

			var result = await controller.UpdateQuestion(rand.Next(), null);

			result.Should().BeOfType<NotFoundResult>();
		}
		
		[Fact]
		public async Task UpdateQuestion_WithExistingItem_ReturnsNoContent() {

			createQuestionDto randomQuestion = new()
			{
				courseId = rand.Next(),
				title = rand.Next().ToString(),
				body = rand.Next().ToString(),
				topicIds = Enumerable.Range(1, 10).OrderBy(_ => rand.Next()).ToList<int>()
			};

			_questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)createRandomQuestion());

			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, __courseRepositoryStub.Object);

			var result = await controller.UpdateQuestion(rand.Next(), randomQuestion);

			result.Should().BeOfType<NoContentResult>();
		}

		
		[Fact]
		public async Task DeleteQuestion_InvalidId_ReturnsNotFound() {
			_questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)null);

			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, __courseRepositoryStub.Object);

			var result = await controller.DeleteQuestion(rand.Next());

			result.Should().BeOfType<NotFoundResult>();
		}
		
		[Fact]
		public async Task DeleteQuestion_WithExistingItem_ReturnsNoContent() {

			_questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync((Question)createRandomQuestion());

			var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, __courseRepositoryStub.Object);

			var result = await controller.DeleteQuestion(rand.Next());

			result.Should().BeOfType<NoContentResult>();
		}
    }
}
