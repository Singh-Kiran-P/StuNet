// @Kiran

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Server.Api.Dtos;
using Server.Api.Enum;
using Server.Api.Helpers;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Services;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITokenManager _tokenManager;

        //private readonly IUserRepository _userRepository;
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        public AuthController(IFieldOfStudyRepository fieldOfStudyRepository, IMapper mapper, UserManager<User> userManager, ITokenManager tokenGenerator)
        {
            //_userRepository = userRepository;
            _fieldOfStudyRepository = fieldOfStudyRepository;
            _mapper = mapper;
            _userManager = userManager;
            _tokenManager = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterJWTUser(RegisterUserDto dto)
        {
            try
            {
                User _user = null;
                string role = "";

                if (dto.Password.Length < 6 || dto.ConfirmPassword.Length < 6) { return BadRequest("Password length should be at least 6"); }

                var hash = PasswordHelper.generateHashAndSalt(dto.Password).Item1;

                //email check en aanmaken account
                Regex regStudent = new Regex(@"\w+@student.uhasselt.be");
                Regex regProf = new Regex(@"\w+@uhasselt.be");
                if (regStudent.IsMatch(dto.Email))
                {
                    //fieldOfStudy processing
                    FieldOfStudy fos = await _fieldOfStudyRepository.getByFullNameAsync(dto.fieldOfStudy);
                    if (fos == null) { return BadRequest("Field Of Study does not exist"); }
                    Student newStudent = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString(), FieldOfStudyId = fos.id };

                    _user = newStudent;
                    role = RolesEnum.student_NORM;

                }
                else if (regProf.IsMatch(dto.Email))
                {
                    Professor prof = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString() };
                    _user = prof;
                    role = RolesEnum.prof_NORM;
                }
                else
                {
                    return BadRequest("Please use an Uhasselt email");
                }

                var result = await _userManager.CreateAsync(_user, dto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                await _userManager.AddToRoleAsync(_user, role);

                //send email confirmation link
                try { sendConfirmationEmail(_user); } catch (Exception) { return BadRequest("Error sending confirmation email"); }

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
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (!user.EmailConfirmed) { return Unauthorized("Please confirm your email adres before logging in!"); }
                if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
                {
                    var token = await _tokenManager.GetTokenAsync(user);

                    return Ok(token);
                }

                return Unauthorized("Invalid password or email");
            }
            catch (System.Exception)
            {
                return Unauthorized("Invalid Authentication");
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return base.Content("<div><p>This user does not exist</p></div>", "text/html");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return base.Content("<div><p>Thank you for confirming your email.</p></div>", "text/html");
            }
            else
            {
                return base.Content("<div><p>Email confirmation failed please contact stunet@gmail.com</p></div>", "text/html");
            }
        }

        private async void sendConfirmationEmail(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { token, email = user.Email }, Request.Scheme);
            EmailSender emailHelper = new EmailSender();
            emailHelper.SendEmail(user.Email, "Confirmation Email", confirmationLink);
        }

        [HttpGet("validateToken")]
        public IActionResult ValidateToken([FromQuery] string token)
        {
            if (_tokenManager.ValidateToken(token))
                return Ok();
            else
                return Unauthorized();
        }
    }
}
