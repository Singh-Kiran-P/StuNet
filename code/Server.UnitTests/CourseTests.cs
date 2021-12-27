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

        [Fact]
        public async Task CreateCourse_WithValidCourseDto_Ok()
        {
            //Given
            CreateCourseDto dto = new()
            {
                name = rand.Next().ToString(),
                number = rand.Next().ToString(),
                description = rand.Next().ToString()
            };
			var controller = CreateController();

			//When
			Course course = ((await controller.CreateCourse(dto)).Result as OkObjectResult).Value as Course;

            //Then
            dto.Should().BeEquivalentTo(
                course,
                options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
            );
            course.id.Should().NotBe(null);
        }

        [Fact]
        public async Task GetCourse_WithValidId_CourseDto()
        {
			//Given
			Course course = createRandomCourse();
			_courseRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(course);
            var controller = CreateController();
            //When
            GetCourseDto dto = ((await controller.GetCourse(rand.Next())).Result as OkObjectResult).Value as GetCourseDto;
            //Then
            dto.Should().BeEquivalentTo(
                course,
                options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
            );
            dto.topics.Should().NotBeNullOrEmpty();
        }
    }
}
