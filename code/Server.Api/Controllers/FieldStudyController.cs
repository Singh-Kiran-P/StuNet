using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;
using Microsoft.AspNetCore.Identity;

using System;
using System.ComponentModel;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldOfStudy>>> GetFieldOfStudies()
        {
            var fieldOfStudies = await _fieldOfStudyRepository.GetAllAsync();
            return Ok(fieldOfStudies);
        }

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

        [HttpGet("user")]
        public async Task<ActionResult<FieldOfStudy>> GetUserFieldOfStudy([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email) as Student;
            if (user == null) return NotFound();
            var id = user.FieldOfStudyId;
            var fieldOfStudy = await _fieldOfStudyRepository.GetAsync(id);
            if (fieldOfStudy == null) return NotFound();
            return Ok(fieldOfStudy);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFieldOfStudy(CreateFieldOfStudyDto createFieldOfStudyDto)
        {
            FieldOfStudy fieldOfStudy = new()
            {
                name = createFieldOfStudyDto.name,
                fullName = createFieldOfStudyDto.fullName,
                isBachelor = createFieldOfStudyDto.isBachelor
            };
            await _fieldOfStudyRepository.CreateAsync(fieldOfStudy);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFieldOfStudy(int id)
        {
            await _fieldOfStudyRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFieldOfStudy(int id, FieldOfStudyDto updateFieldOfStudyDto)
        {
            FieldOfStudy fieldOfStudy = new()
            {
                id = id,
                name = updateFieldOfStudyDto.name,
                fullName = updateFieldOfStudyDto.fullName,
                isBachelor = updateFieldOfStudyDto.isBachelor
            };
            await _fieldOfStudyRepository.UpdateAsync(fieldOfStudy);
            return Ok();
        }
    }
}
