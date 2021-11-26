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
using Server.Api.Enum;
using Server.Api.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            User _user = null;
            string role = "";
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

            //send email confirmation link
            try {sendConfirmationEmail(_user);}
            catch (Exception){return BadRequest("Error sending confirmation email");}

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginJWT(LoginUserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if(!user.EmailConfirmed) {return Unauthorized("Please confirm your email adres before logging in!");}
            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                if (!user.EmailConfirmed) { return Unauthorized("Please confirm your email before proceeding"); }
                var token = await _tokenGenerator.GetTokenAsync(user);
                return Ok(token);
            }

            return Unauthorized("Invalid Authentication");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return base.Content("<div><p>This user does not exist</p></div>", "text/html");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded){
                return base.Content("<div><p>Thank you for confirming your email.</p></div>", "text/html");
            } else {
                return base.Content("<div><p>Email confirmation failed please contact stunet@gmail.com</p></div>", "text/html");
            }
        }

        private async void sendConfirmationEmail(User user) {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { token, email = user.Email }, Request.Scheme);
            EmailSender emailHelper = new EmailSender();
            await emailHelper.SendEmailAsync(user.Email, "Confirmation Email", confirmationLink);
        }
    }
}