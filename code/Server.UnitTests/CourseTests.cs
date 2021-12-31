using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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
            string profEmail = rand.Next().ToString();
            CreateCourseDto dto = new()
            {
                name = rand.Next().ToString(),
                number = rand.Next().ToString(),
                description = rand.Next().ToString(),
                courseEmail = rand.Next().ToString(),
                profEmail = profEmail
            };
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("username", profEmail) }, "TestAuthType"));


			var controller = CreateController();
			controller.ControllerContext.HttpContext = httpContext;

			//When
			var result = await controller.CreateCourse(dto);

            //Then
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<Course>();
            var createdCourse = (result.Result as OkObjectResult).Value as Course;

            createdCourse.Should().BeEquivalentTo(
                dto,
                options => options.ComparingByMembers<CreateCourseDto>().ExcludingMissingMembers()
            );
            createdCourse.id.Should().NotBe(null);
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
			var result = await controller.GetCourse(rand.Next());

            //Then
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<GetCourseDto>();
            var createdCourse = (result.Result as OkObjectResult).Value as GetCourseDto;

            createdCourse.Should().BeEquivalentTo(
                course,
                options => options.ComparingByMembers<CreateQuestionDto>().ExcludingMissingMembers()
            );
            createdCourse.topics.Should().NotBeNullOrEmpty();
        }
    }
}
