using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;
using System.Text.RegularExpressions;
using Server.Api.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VmsApi.Services;
using Server.Api.Enum;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        //private readonly IUserRepository _userRepository;
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        public AuthController(IFieldOfStudyRepository fieldOfStudyRepository, IMapper mapper, UserManager<User> userManager, ITokenGenerator tokenGenerator)
        {
            //_userRepository = userRepository;
            _fieldOfStudyRepository = fieldOfStudyRepository;

            _mapper = mapper;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterJWTUser(RegisterUserDto dto)
        {
            try
            {
                User _user = null;
                string role = "";

                if(dto.Password.Length < 6 || dto.ConfirmPassword.Length < 6) {return BadRequest("Password length should be at least 6");}

                var hash = PasswordHelper.generateHashAndSalt(dto.Password).Item1;

                //email check en aanmaken account
                Regex regStudent = new Regex(@"\w+@student.uhasselt.be");
                Regex regProf = new Regex(@"\w+@uhasselt.be");
                if (regStudent.IsMatch(dto.Email))
                {
                    //fieldOfStudy processing
                    FieldOfStudy fos = await _fieldOfStudyRepository.getByFullNameAsync(dto.fieldOfStudy);
                    if (fos == null) { return BadRequest("Field Of Study does not exist"); }
                    Student newStudent = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString(), FieldOfStudyId = fos.id};

                    _user = newStudent;
                    role = RolesEnum.student_NORM;

                }
                else if (regProf.IsMatch(dto.Email))
                {
                    Professor prof = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString() };
                    _user = prof;
                    role = RolesEnum.prof_NORM;
                } else {
                    return BadRequest("Please use an Uhasselt email");
                }


                var result = await _userManager.CreateAsync(_user, dto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                await _userManager.AddToRoleAsync(_user, role);

                return StatusCode(201);
            }
            catch (System.Exception)
            {
                return BadRequest("Error while creating account contact support");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginJWT(LoginUserDto dto)
        {
            try {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
                {
                    var loginJWTDto = await _tokenGenerator.GetTokenAsync(user);

                    return Ok(loginJWTDto);
                }

                return Unauthorized("Invalid Authentication");

            } catch (System.Exception) {
				return Unauthorized("Invalid Authentication");
			}
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> Refresh(RefreshCred refreshCred)
        {
			// var username = principal.Identity.Name;
			// var savedRefreshToken = GetRefreshToken(username); //retrieve the refresh token from a data store
			// if (savedRefreshToken != refreshToken)
			//     throw new SecurityTokenException("Invalid refresh token");

			// var newJwtToken = GenerateToken(principal.Claims);
			// var newRefreshToken = GenerateRefreshToken();
			// DeleteRefreshToken(username, refreshToken);
			// SaveRefreshToken(username, newRefreshToken);

			// return new ObjectResult(new {
			//     token = newJwtToken,
			//     refreshToken = newRefreshToken
			// });
			return Ok();
		}
    }
}
