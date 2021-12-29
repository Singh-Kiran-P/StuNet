using System;
using Xunit;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Repositories;
using Server.Api.Controllers;
using Server.Api.Models;
using Server.Api.Dtos;
using Server.Api.Helpers;
using Server.Api.Services;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Server.UnitTests 
{
    public class AuthTests : UnitTest
    {

        private AuthController createController(UserManager<User> userManager = null)
        {
            var configuration = new ConfigurationBuilder()
                // .AddInMemoryCollection(new Dictionary<string, string>
                //     {
                //         {"Key1", "Value1"},
                //         {"Nested:Key1", "NestedValue1"},
                //         {"Nested:Key2", "NestedValue2"}
                //     })
                .Build();

            Mock<JwtTokenManager> tokenManager = new(userManager, configuration);
            return new AuthController(_FOSRepositoryStub.Object, null, userManager, tokenManager.Object);
        }

        [Theory]
        [InlineData("@student.uhasselt.be")]
        [InlineData("@uhasselt.be")]
        public async Task RegisterJWTUser_withValidEmail_Returns201(string domain)
        {
            // Given
            string password = rand.Next().ToString().PadLeft(6);
            RegisterUserDto registerDto = new()
            {
                Email = rand.Next().ToString() + domain,
                Password = password,
                ConfirmPassword = password,
                fieldOfStudy = rand.Next().ToString()
            };

            _FOSRepositoryStub.Setup(repo => repo.GetByFullNameAsync(It.IsAny<string>()))
                .ReturnsAsync(createRandomFieldOfStudy());

            var MockManager = GetMockUserManager();
            MockManager.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<User, string>((u, _) => u.Should().BeEquivalentTo(
                    registerDto,
                    options => options.ComparingByMembers<RegisterUserDto>().ExcludingMissingMembers()));

            MockManager.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(rand.Next().ToString());
        
            var controller = createController(MockManager.Object);

            // When
            var result = await controller.RegisterJWTUser(registerDto);
        
            // Then
            result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(201);
        }

        [Theory]
        [InlineData("student@student.uhasselt.be", "", "Password length should be at least 6")] // valid mail, invalid password
        [InlineData("student", "lengthOfAtleast6", "Please use an Uhasselt email")] // invalid mail, valid password
        [InlineData(".:;@[/|\\]@uhasselt.be", "lengthOfAtleast6", "Please use an Uhasselt email")] // valid mail domain, valid password, ilegal mailaddress characters
        public async Task RegisterJWTUser_withInvalidProperties_ReturnsBadRequest(string email, string password, string expectedError)
        {
            // Given
            RegisterUserDto registerDto = new()
            {
                Email = rand.Next().ToString(),
                Password = password,
                ConfirmPassword = password,
                fieldOfStudy = rand.Next().ToString()
            };

            _FOSRepositoryStub.Setup(repo => repo.GetByFullNameAsync(It.IsAny<string>()))
                .ReturnsAsync(createRandomFieldOfStudy());

            var MockManager = GetMockUserManager();
            MockManager.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            MockManager.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(rand.Next().ToString());
        
            var controller = createController(MockManager.Object);
        
            // When
            var result = await controller.RegisterJWTUser(registerDto);
        
            // Then
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be(expectedError);
        }
    }
}