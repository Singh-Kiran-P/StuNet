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

        private readonly IUserRepository _userRepository;
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        public AuthController(IUserRepository userRepository, IFieldOfStudyRepository fieldOfStudyRepository, IMapper mapper, UserManager<User> userManager, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _fieldOfStudyRepository = fieldOfStudyRepository;

            _mapper = mapper;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;


        }

        [HttpPost("registerJWT")]
        public async Task<ActionResult> RegisterJWTUser([FromBody] RegisterUserDto dto)
        {
            User _user = null;
            string role = "";
            var hash = PasswordHelper.generateHashAndSalt(dto.Password).Item1;
            // User user = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString() };

            //Generate password salt and hash
            // (string hashedPass, string salt) = PasswordHelper.generateHashAndSalt(dto.Password);

            //email check en aanmaken account
            Regex regStudent = new Regex(@"\w+@student.uhasselt.be");
            Regex regProf = new Regex(@"\w+@uhasselt.be");
            if (regStudent.IsMatch(dto.Email))
            {
                //fieldOfStudy processing
                FieldOfStudy fos = await _fieldOfStudyRepository.getByFullNameAsync(dto.fieldOfStudy);
                if (fos == null) { return BadRequest("Field Of Study does not exist"); }
                Student newStudent = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString(), fieldOfStudy = fos };

                _user = newStudent;
                role = RolesEnum.student_NORM;

            }
            else if (regProf.IsMatch(dto.Email))
            {
                Professor prof = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString() };
                _user = prof;
                role = RolesEnum.prof_NORM;
            }


            var result = await _userManager.CreateAsync(_user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(_user, role);

            return StatusCode(201);
        }

        [HttpPost("loginJWT")]
        public async Task<IActionResult> LoginJWT(LoginUserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var token = await _tokenGenerator.GetTokenAsync(user);

                return Ok(token);
            }

            return Unauthorized("Invalid Authentication");
        }


        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserDto dto)
        {
            // User _user = null;

            // //password check
            // if (dto.password1 != dto.password2) { return BadRequest("Passwords do not match"); }

            // //Generate password salt and hash
            // (string hashedPass, string salt) = PasswordHelper.generateHashAndSalt(dto.password1);

            // //email check en aanmaken account
            // Regex regStudent = new Regex(@"\w+@student.uhasselt.be");
            // Regex regProf = new Regex(@"\w+@uhasselt.be");
            // if (regStudent.IsMatch(dto.email))
            // {
            //     //fieldOfStudy processing
            //     FieldOfStudy fos = await _fieldOfStudyRepository.getByFullNameAsync(dto.fieldOfStudy);
            //     if (fos == null) { return BadRequest("Field Of Study does not exist"); }
            //     Student newStudent = new() { email = dto.email, password = hashedPass, salt = salt, fieldOfStudy = fos };
            //     _user = newStudent;
            // }
            // else if (regProf.IsMatch(dto.email))
            // {
            //     Professor prof = new() { email = dto.email, password = hashedPass, salt = salt };
            //     _user = prof;
            // }
            // else { return Unauthorized("Please use an uhasselt e-mail to create an account"); }

            // await _userRepository.createAsync(_user);
            return Ok();
        }

        //Checkt voorlopig alleen of user een juist passwoord ingeeft. TODO: JWT token?
        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(LoginUserDto dto)
        {
            // User user = await _userRepository.getByEmailAsync(dto.Email);
            // if (user != null)
            // {
            //     if (PasswordHelper.AreEqual(dto.Password, user.password, user.salt)) { return Ok("Logged in"); }
            // }
            return Unauthorized("Login failed");
        }
    }
}