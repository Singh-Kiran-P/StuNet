using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;
using Microsoft.AspNetCore.Identity;

using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FieldOfStudyController : ControllerBase
    {
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        private readonly UserManager<User> _userManager;

        public FieldOfStudyController(IFieldOfStudyRepository fieldOfStudyRepository, UserManager<User> userManager)
        {
            _fieldOfStudyRepository = fieldOfStudyRepository;
            _userManager = userManager;
        }
        [Authorize(Roles = "student,prof")]

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldOfStudy>>> GetFieldOfStudies()
        {
            var fieldOfStudies = await _fieldOfStudyRepository.GetAllAsync();
            return Ok(fieldOfStudies);
        }
        [Authorize(Roles = "student,prof")]

        [HttpGet("{id}")]
        public async Task<ActionResult<FieldOfStudy>> GetFieldOfStudy(int id)
        {
            var fieldOfStudy = await _fieldOfStudyRepository.GetAsync(id);
            if (fieldOfStudy == null)
            {
                return NotFound();
            }
            return Ok(fieldOfStudy);
        }
        [Authorize(Roles = "student,prof")]

        [HttpGet("user")]
        public async Task<ActionResult<FieldOfStudy>> GetUserFieldOfStudy([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            var id = 1; // TODO DOESNT FUCKING WORK: user.FieldOfStudyId;
            var fieldOfStudy = await _fieldOfStudyRepository.GetAsync(id);
            if (fieldOfStudy == null) return NotFound();
            return Ok(fieldOfStudy);
        }
        [Authorize(Roles = "prof")]

        [HttpPost]
        public async Task<ActionResult> CreateFieldOfStudy(CreateFieldOfStudyDto createFieldOfStudyDto)
        {
            string _fullname = createFieldOfStudyDto.name
                + "-"
                + (createFieldOfStudyDto.isBachelor ? "BACH" : "MASTER");
            FieldOfStudy fieldOfStudy = new()
            {
                fullName = _fullname,
                name = createFieldOfStudyDto.name,
                isBachelor = createFieldOfStudyDto.isBachelor
            };
            await _fieldOfStudyRepository.CreateAsync(fieldOfStudy);
            return Ok();
        }

        [Authorize(Roles = "prof")]

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFieldOfStudy(int id)
        {
            await _fieldOfStudyRepository.DeleteAsync(id);
            return Ok();
        }
        [Authorize(Roles = "prof")]

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFieldOfStudy(int id, FieldOfStudyDto updateFieldOfStudyDto)
        {
            FieldOfStudy fieldOfStudy = new()
            {
                id = id,
                fullName = updateFieldOfStudyDto.fullName,
                name = updateFieldOfStudyDto.name,
                isBachelor = updateFieldOfStudyDto.isBachelor
            };
            await _fieldOfStudyRepository.UpdateAsync(fieldOfStudy);
            return Ok();
        }
    }
}
