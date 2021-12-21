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
		private readonly Mock<ICourseRepository> _courseRepositoryStub = new();
		private readonly Random rand = new();

		// private Question createRandomQuestion() {

		// 	DateTime start = new DateTime(1995, 1, 1);
		// 	int range = (DateTime.Today - start).Days;           

		// 	return new()
		// 	{
		// 		id = rand.Next(),
		// 		userId = null,
		// 		course = new Course
		// 		{
		// 			id = rand.Next(),
		// 			name = rand.Next().ToString(),
		// 			number = rand.Next().ToString()
		// 		},
		// 		title = rand.Next().ToString(),
		// 		body = rand.Next().ToString(),
		// 		topics = Enumerable.Range(1, 10).Select(_ => new Topic {
		// 			id = rand.Next(),
		// 			name = rand.Next().ToString()
		// 		}).ToList(),
		// 		time = start.AddDays(rand.Next(range))
		// 	};
		// }

		// [Fact]
        // public async Task GetQuestion_InvalidId_ReturnsNotFound() {

        //     // Arrange
        //     _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        //         .ReturnsAsync((Question)null);

		// 	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object);

        //     // Act
        //     var result = await controller.GetQuestion(rand.Next());

        //     // Assert
        //     result.Result.Should().BeOfType<NotFoundResult>();
        // }

        // [Fact]
        // public async Task GetQuestion_validId_ReturnsQuestion()
        // {

        //     // Arrange
        //     Question question = createRandomQuestion();

        //     _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        //         .ReturnsAsync(question);

		// 	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object);

        //     // Act
        //     var result = await controller.GetQuestion(rand.Next());

        //     // Assert
        //     ((result.Result as OkObjectResult).Value as questionDto).Should()
        //     .BeEquivalentTo(question, options => options.ComparingByMembers<questionDto>().ExcludingMissingMembers());
        // }

        // [Fact]
        // public async Task CreateQuestion_FromCreateQuestionDto_ReturnsCreatedItem()
        // {

        //     Topic randomTopic = new()
        //     {
        //         id = rand.Next(),
        //         name = rand.Next().ToString(),
        //         course = null,
        //         questions = null,
        //     };

		// 	Course randomCourse = new()
		// 	{
		// 		id = rand.Next(),
		// 		name = rand.Next().ToString(),
		// 		number = rand.Next().ToString(),
		// 	};

		// 	createQuestionDto questionToCreate = new()
		// 	{
		// 		courseId = rand.Next(),
		// 		title = rand.Next().ToString(),
		// 		body = rand.Next().ToString(),
		// 		topicIds = Enumerable.Range(1, 10).Select(_ => rand.Next()).ToList<int>()
		// 	};

        //     _topicRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        //         .ReturnsAsync(randomTopic);

		// 	_courseRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
		// 		.ReturnsAsync(randomCourse);

		// 	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object);


        //     var result = await controller.CreateQuestion(questionToCreate);


		// 	var createdQuestion = (result.Result as OkObjectResult).Value as questionDto;
		// 	questionToCreate.Should().BeEquivalentTo(
		// 		createdQuestion,
		// 		options => options.ComparingByMembers<createQuestionDto>().ExcludingMissingMembers()
		// 	);

		// 	createdQuestion.id.Should().NotBe(null);
		// 	createdQuestion.topics.Should().NotBeNullOrEmpty();
		// 	foreach (getOnlyTopicDto t in createdQuestion.topics)
		// 	{
		// 		t.Should().BeEquivalentTo(
		// 			randomTopic,
		// 			options => options.ComparingByMembers<getOnlyTopicDto>().ExcludingMissingMembers()
		// 		);	
		// 	}
		// 	createdQuestion.time.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 0, 0, 500)); // 500ms
		// }

        // [Fact]
        // public async Task UpdateQuestion_InvalidId_ReturnsNotFound()
        // {
        //     _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        //         .ReturnsAsync((Question)null);

        //     var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object);

        //     var result = await controller.UpdateQuestion(rand.Next(), null);

        //     result.Should().BeOfType<NotFoundResult>();
        // }

        // [Fact]
        // public async Task UpdateQuestion_WithExistingItem_ReturnsNoContent()
        // {

        //     createQuestionDto randomQuestion = new()
        //     {
        //         courseId = rand.Next(),
        //         title = rand.Next().ToString(),
        //         body = rand.Next().ToString(),
        //         topicIds = Enumerable.Range(1, 10).OrderBy(_ => rand.Next()).ToList<int>()
        //     };

        //     _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        //         .ReturnsAsync((Question)createRandomQuestion());

		// 	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object);

        //     var result = await controller.UpdateQuestion(rand.Next(), randomQuestion);

        //     result.Should().BeOfType<NoContentResult>();
        // }


        // [Fact]
        // public async Task DeleteQuestion_InvalidId_ReturnsNotFound()
        // {
        //     _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        //         .ReturnsAsync((Question)null);

        //     var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object);

        //     var result = await controller.DeleteQuestion(rand.Next());

        //     result.Should().BeOfType<NotFoundResult>();
        // }

        // [Fact]
        // public async Task DeleteQuestion_WithExistingItem_ReturnsNoContent()
        // {

        //     _questionRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
        //         .ReturnsAsync((Question)createRandomQuestion());

		// 	var controller = new QuestionController(_questionRepositoryStub.Object, _topicRepositoryStub.Object, _courseRepositoryStub.Object);

        //     var result = await controller.DeleteQuestion(rand.Next());

        //     result.Should().BeOfType<NoContentResult>();
        // }
    }
}
