using System;
using AutoMapper;
using Server.Api.Dtos;
using Server.Api.Enum;
using Server.Api.Models;
using Server.Api.Helpers;
using Server.Api.Services;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _mailSender;
        private readonly ITokenManager _tokenManager;
        private readonly UserManager<User> _userManager;

        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        public AuthController(IFieldOfStudyRepository fieldOfStudyRepository, IMapper mapper, UserManager<User> userManager, ITokenManager tokenGenerator, IEmailSender mailSender)
        {
            _mapper = mapper;
            _mailSender = mailSender;
            _userManager = userManager;
            _tokenManager = tokenGenerator;
            _fieldOfStudyRepository = fieldOfStudyRepository;

        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterJWTUser(RegisterUserDto dto)
        {
            User user = null;
            string role = "";
            if (dto.Password.Length < 6) return BadRequest("Password length should be at least 6");
            var hash = PasswordHelper.generateHashAndSalt(dto.Password).Item1;
            Regex regStudent = new Regex(@"\w+@student\.uhasselt\.be");
            Regex regProf = new Regex(@"\w+@uhasselt\.be");
            if (regStudent.IsMatch(dto.Email)) {
                FieldOfStudy fos = await _fieldOfStudyRepository.GetAsync(dto.fieldOfStudy);
                if (fos == null) return BadRequest("Field Of Study does not exist");
                Student newStudent = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString(), FieldOfStudyId = fos.id };
                role = RolesEnum.student_NORM;
                user = newStudent;
            }
            else if (regProf.IsMatch(dto.Email)) {
                Professor prof = new() { UserName = dto.Email, Email = dto.Email, PasswordHash = hash.ToString() };
                role = RolesEnum.prof_NORM;
                user = prof;
            }
            else return BadRequest("Please use an UHasselt email");
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, role);
            try { SendConfirmationEmail(user); }
            catch (Exception) { return BadRequest("Error sending confirmation email"); }
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginJWT(LoginUserDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null) return Unauthorized("Invalid email address");
                if (!user.EmailConfirmed) return Unauthorized("Please confirm your email address before logging in!");
                if (!(await _userManager.CheckPasswordAsync(user, dto.Password))) return Unauthorized("Invalid password");
                var token = await _tokenManager.GetTokenAsync(user);
                return Ok(token);
            }
            catch { return Unauthorized("Invalid Authentication"); }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return base.Content("<div><p>This user does not exist</p></div>", "text/html");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded) return base.Content("<div><p>Your email has been confirmed, you can now log in to StuNet.</p></div>", "text/html");
            else return base.Content("<div><p>Email confirmation failed, please contact stunetuhasselt@outlook.com</p></div>", "text/html");
        }

        private async void SendConfirmationEmail(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { token, email = user.Email }, Request.Scheme);
            await _mailSender.SendEmail(user.Email, "Confirmation Email", EmailTemplate.ConfirmEmail, new {
               link = confirmationLink
            });
        }

        [HttpGet("validateToken")]
        public IActionResult ValidateToken([FromQuery] string token)
        {
            if (_tokenManager.ValidateToken(token)) return Ok();
            else return Unauthorized();
        }
    }
}
