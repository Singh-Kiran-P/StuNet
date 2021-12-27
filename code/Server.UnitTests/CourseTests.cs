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

        private CourseController createController() {
            return new CourseController(_courseRepositoryStub.Object, _topicRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object);
        }

        [Fact]
        public async Task createCourse_WithValidCourseDto_Ok()
        {
            //Given
            createCourseDto dto = new()
            {
                name = rand.Next().ToString(),
                number = rand.Next().ToString(),
                description = rand.Next().ToString()
            };
			var controller = createController();

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
			Course course = createRandomCourse();
			_courseRepositoryStub.Setup(repo => repo.getAsync(It.IsAny<int>()))
                .ReturnsAsync(course);
            var controller = createController();
            //When
            GetCourseDto dto = ((await controller.GetCourse(rand.Next())).Result as OkObjectResult).Value as GetCourseDto;
            //Then
            dto.Should().BeEquivalentTo(
                course,
                options => options.ComparingByMembers<createQuestionDto>().ExcludingMissingMembers()
            );
            dto.topics.Should().NotBeNullOrEmpty();
        }
    }
}
