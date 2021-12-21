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
    public class CourseTests
    {
        public readonly Random random = new();

        private readonly Mock<ITopicRepository> _topicRepositoryStub = new();
        private readonly Mock<ICourseRepository> _courseRepositoryStub = new();

        private string randomName()
        {
            return "random" + random.Next().ToString();
        }

        private int randomInt()
        {
            return random.Next();
        }

        [Fact]
        public async Task createCourse_WithValidCourseDto_Ok()
        {
            //Given
            int count = random.Next(1, 50);
            createCourseDto dto = new()
            {
                name = randomName(),
                number = randomInt().ToString(),
                description = randomName()
            };
            var controller = new CourseController(_courseRepositoryStub.Object, _topicRepositoryStub.Object);

            //When
            Course course = ((await controller.createCourse(dto)).Result as OkObjectResult).Value as Course;

            //Then
            dto.Should().BeEquivalentTo(
                course,
                options => options.ComparingByMembers<createQuestionDto>().ExcludingMissingMembers()
            );
            course.id.Should().NotBe(null);
        }

        [Fact]
        public async Task getCourse_WithValidId_CourseDto()
        {
            //Given
            Course course = new()
            {
                name = "random" + random.Next().ToString(),
                number = random.Next().ToString(),
                description = random.Next().ToString(),
                topics = new List<Topic> { new Topic() { name = randomName(), id = randomInt() } },
                channels = new List<TextChannel> { new TextChannel() { name = randomName(), id = randomInt() } }
            };
            _courseRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync(course);
            var controller = new CourseController(_courseRepositoryStub.Object, _topicRepositoryStub.Object);
            //When
            GetCourseDto dto = ((await controller.GetCourse(randomInt())).Result as OkObjectResult).Value as GetCourseDto;
            //Then
            dto.Should().BeEquivalentTo(
                course,
                options => options.ComparingByMembers<createQuestionDto>().ExcludingMissingMembers()
            );
            dto.topics.Should().NotBeNullOrEmpty();
        }
    }
}
