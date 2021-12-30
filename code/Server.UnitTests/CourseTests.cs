using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Repositories;
using Server.Api.Controllers;
using Server.Api.Models;
using Server.Api.Dtos;

namespace Server.UnitTests
{
    public class CourseTests : UnitTest
    {

        private CourseController CreateController() {
            return new CourseController(_courseRepositoryStub.Object, _topicRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object);
        }

        // TODO inloggen bij tests???

        /* [Fact]
        public async Task createCourse_WithValidCourseDto_Ok()
        {
            //Given
            CreateCourseDto dto = new()
            {
                name = randomName(),
                number = randomInt().ToString(),
                description = randomName(),
                courseEmail = "random" + random.Next().ToString() + "@gmail.com",
                profEmail = "random" + random.Next().ToString() + "@uhasselt.be",
            };
			var controller = CreateController();

			//When
			var result = await controller.CreateCourse(dto);

            //Then
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<Course>();
            var createdCourse = (result.Result as OkObjectResult).Value as Course;

            createdCourse.Should().BeEquivalentTo(
                dto,
                options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
            );
            createdCourse.id.Should().NotBe(null);
        }

        [Fact]
        public async Task GetCourse_WithValidId_CourseDto()
        {
            //Given
            Course course = new()
            {
                name = "random" + random.Next().ToString(),
                number = random.Next().ToString(),
                description = random.Next().ToString(),
                courseEmail = "random" + random.Next().ToString() + "@gmail.com",
                profEmail = "random" + random.Next().ToString() + "@uhasselt.be",
                topics = new List<Topic> { new Topic() { name = randomName(), id = randomInt() } },
                channels = new List<TextChannel> { new TextChannel() { name = randomName(), id = randomInt() } }
            };
            _courseRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync(course);
            var controller = CreateController();

            //When
			var result = await controller.GetCourse(rand.Next());

            //Then
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<GetCourseDto>();
            var createdCourse = (result.Result as OkObjectResult).Value as GetCourseDto;

            createdCourse.Should().BeEquivalentTo(
                course,
                options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
            );
            dto.topics.Should().NotBeNullOrEmpty();
        } */
    }
}
