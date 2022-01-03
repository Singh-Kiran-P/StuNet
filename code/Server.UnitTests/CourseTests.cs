using Moq;
using Xunit;
using System;
using System.Linq;
using Server.Api.Dtos;
using FluentAssertions;
using Server.Api.Models;
using Server.Api.Controllers;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Server.UnitTests
{
    public class CourseTests : UnitTest
    {

        private CourseController CreateController() {
            var MockManager = GetMockUserManager();
            return new CourseController(_courseRepositoryStub.Object, _topicRepositoryStub.Object, _courseSubscriptionRepositoryStub.Object, MockManager.Object);
        }

        [Fact]
        public async Task CreateCourse_WithValidCourseDto_Ok()
        {
            string profEmail = rand.Next().ToString();
            CreateCourseDto dto = new() {
                profEmail = profEmail,
                name = rand.Next().ToString(),
                number = rand.Next().ToString(),
                description = rand.Next().ToString(),
                courseEmail = rand.Next().ToString()
            };

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("username", profEmail) }, "TestAuthType"));

			var controller = CreateController();
			controller.ControllerContext.HttpContext = httpContext;

			var result = await controller.CreateCourse(dto);

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
			Course course = createRandomCourse();
			_courseRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(course);
            var controller = CreateController();

			var result = await controller.GetCourse(rand.Next());

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
