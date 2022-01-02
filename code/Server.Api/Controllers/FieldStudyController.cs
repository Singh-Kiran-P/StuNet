using Server.Api.Dtos;
using Server.Api.Models;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FieldOfStudyController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;

        public FieldOfStudyController(IFieldOfStudyRepository fieldOfStudyRepository, UserManager<User> userManager)
        {
            _userManager = userManager;
            _fieldOfStudyRepository = fieldOfStudyRepository;
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
            if (fieldOfStudy == null) return NotFound();
            return Ok(fieldOfStudy);
        }

        [Authorize(Roles = "student,prof")]
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

        [Authorize(Roles = "prof")]
        [HttpPost]
        public async Task<ActionResult> CreateFieldOfStudy(CreateFieldOfStudyDto createFieldOfStudyDto)
        {
            FieldOfStudy fieldOfStudy = new() {
                name = createFieldOfStudyDto.name,
                fullName = createFieldOfStudyDto.fullName,
                isBachelor = createFieldOfStudyDto.isBachelor
            };

            await _fieldOfStudyRepository.CreateAsync(fieldOfStudy);
            return Ok();
        }

        [Authorize(Roles = "prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFieldOfStudy(int id)
        {
            var existing = await _fieldOfStudyRepository.GetAsync(id);
            if (existing == null) return NotFound();
            await _fieldOfStudyRepository.DeleteAsync(id);
            return Ok();
        }

        [Authorize(Roles = "prof")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFieldOfStudy(int id, FieldOfStudyDto updateFieldOfStudyDto)
        {
            var existing = await _fieldOfStudyRepository.GetAsync(id);
            if (existing == null) return NotFound();
            FieldOfStudy fieldOfStudy = new() {
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
