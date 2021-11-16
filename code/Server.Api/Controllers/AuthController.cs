using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;
using System.Text.RegularExpressions;
using Server.Api.Helpers;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        public AuthController(IUserRepository userRepository, IFieldOfStudyRepository fieldOfStudyRepository)
        {
            _userRepository = userRepository;
            _fieldOfStudyRepository = fieldOfStudyRepository;
        }
    
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserDto dto)
        {
            User _user = null;

            //password check
            if(dto.password1 != dto.password2) {return BadRequest("Passwords do not match");}

            //Generate password salt and hash
            (string hashedPass, string salt) = PasswordHelper.generateHashAndSalt(dto.password1);

            //email check en aanmaken account
            Regex regStudent = new Regex(@"\w+@student.uhasselt.be");
            Regex regProf = new Regex(@"\w+@uhasselt.be");
            if (regStudent.IsMatch(dto.email)) {
                //fieldOfStudy processing
                FieldOfStudy fos = await _fieldOfStudyRepository.getByFullNameAsync(dto.fieldOfStudy);
                if(fos == null) {return BadRequest("Field Of Study does not exist");}
                Student newStudent = new() { email = dto.email, password = hashedPass, salt = salt, fieldOfStudy = fos};
                _user = newStudent;
            }
            else if (regProf.IsMatch(dto.email)) {
                Professor prof = new(){email = dto.email, password = hashedPass, salt = salt};
                _user = prof;
            }
            else{return Unauthorized("Please use an uhasselt e-mail to create an account");}
    
            await _userRepository.createAsync(_user);
            return Ok();
        }
        
        //Checkt voorlopig alleen of user een juist passwoord ingeeft. TODO: JWT token?
        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(LoginUserDto dto)
        {
            User user = await _userRepository.getByEmailAsync(dto.email);
            if(user != null){
                if(PasswordHelper.AreEqual(dto.password, user.password, user.salt)) {return Ok("Logged in");}
            }
            return Unauthorized("Login failed");
        }
    }
}