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
            createCourseDto dto = new(){
                Name = randomName(),
                Number = randomInt().ToString(),
                topicNames = Enumerable.Range(1, count).Select(_ => randomName()).ToList()
            };
            var controller = new CourseController(_courseRepositoryStub.Object, _topicRepositoryStub.Object);
            
        //When
            Course course = ((await controller.createCourse(dto)).Result as OkObjectResult).Value as Course;

        //Then
			dto.Should().BeEquivalentTo(
				course,
				options => options.ComparingByMembers<createQuestionDto>().ExcludingMissingMembers()
			);
			course.Id.Should().NotBe(null);
            course.topics.Should().OnlyContain(topic => topic.course == course);
			course.topics.Should().NotBeNullOrEmpty();
			course.topics.Should().AllBeOfType<Topic>();
			course.topics.Should().HaveCount(count);
			// course.dateTime.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 0, 0, 500)); // 500ms
        }

        [Fact]
        public async Task getCourse_WithValidId_CourseDto()
        {
        //Given
            Course course = new()
            {
                Name = "random" + random.Next().ToString(),
                Number = random.Next().ToString(),
                topics = new List<Topic>{new Topic(){ name = randomName(), id = randomInt() }}
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
            dto.Topics.Should().NotBeNullOrEmpty();
        }
    }
}