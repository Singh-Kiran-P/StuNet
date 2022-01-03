using Moq;
using Xunit;
using System;
using System.Linq;
using Server.Api.Dtos;
using FluentAssertions;
using System.Threading;
using Server.Api.Models;
using Server.Api.Helpers;
using Server.Api.Services;
using System.Security.Claims;
using Server.Api.Controllers;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Server.UnitTests
{
    public class AuthTests : UnitTest
    {

        private AuthController createController(UserManager<User> userManager = null)
        {
            var configuration = new ConfigurationBuilder()
                .Build();

            Mock<JwtTokenManager> tokenManager = new(userManager, configuration);
            return new AuthController(_FOSRepositoryStub.Object, null, userManager, tokenManager.Object, _emailSenderStub.Object);
        }

        [Theory]
        [InlineData("@student.uhasselt.be")]
        [InlineData("@uhasselt.be")]
        public async Task RegisterJWTUser_withValidEmail_Returns201(string domain)
        {
            string password = rand.Next().ToString().PadLeft(6);
            RegisterUserDto registerDto = new() {
                Password = password,
                ConfirmPassword = password,
                fieldOfStudy = rand.Next(),
                Email = rand.Next().ToString() + domain
            };

            _FOSRepositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(createRandomFieldOfStudy());

            var MockManager = GetMockUserManager();
            MockManager.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<User, string>((u, _) => u.Should().BeEquivalentTo(
                    registerDto,
                    options => options.ComparingByMembers<RegisterUserDto>().ExcludingMissingMembers()
                ));

            MockManager.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(rand.Next().ToString());

            var controller = createController(MockManager.Object);

            var result = await controller.RegisterJWTUser(registerDto);

            result.Should().BeOfType<NoContentResult>();
        }

        [Theory]
        [InlineData("student@student.uhasselt.be", "", "Password length should be at least 6")] // valid mail, invalid password
        [InlineData("student", "lengthOfAtleast6", "Please use an UHasselt email")] // invalid mail, valid password
        [InlineData(".:;@[/|\\]@uhasselt.be", "lengthOfAtleast6", "Please use an UHasselt email")] // valid mail domain, valid password, illegal mailaddress characters
        public async Task RegisterJWTUser_withInvalidProperties_ReturnsBadRequest(string email, string password, string expectedError)
        {
            RegisterUserDto registerDto = new() {
                Email = email,
                Password = password,
                ConfirmPassword = password,
                fieldOfStudy = rand.Next()
            };

            _FOSRepositoryStub.Setup(repo => repo.GetByFullNameAsync(It.IsAny<string>()))
                .ReturnsAsync(createRandomFieldOfStudy());

            var MockManager = GetMockUserManager();
            MockManager.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            MockManager.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(rand.Next().ToString());

            var controller = createController(MockManager.Object);

            var result = await controller.RegisterJWTUser(registerDto);

            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be(expectedError);
        }
    }
}
