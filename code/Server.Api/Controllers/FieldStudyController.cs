using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;


namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FieldOfStudyController: ControllerBase
    {
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        public FieldOfStudyController(IFieldOfStudyRepository fieldOfStudyRepository)
        {
            _fieldOfStudyRepository = fieldOfStudyRepository;
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldOfStudy>>> GetFieldOfStudies()
        {
            var fieldOfStudies = await _fieldOfStudyRepository.getAllAsync();
            return Ok(fieldOfStudies);
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<FieldOfStudy>> GetFieldOfStudy(int id)
        {
            var fieldOfStudy = await _fieldOfStudyRepository.getAsync(id);
            if(fieldOfStudy == null)
                return NotFound();
    
            return Ok(fieldOfStudy);
        }
    
        [HttpPost]
        public async Task<ActionResult> CreateFieldOfStudy(string fullNamsdfe ,string fullNdsame, string? fullName = null)
        {
            string _fullname = "";


            // if (createFieldOfStudyDto.fullName != null) {
            //     _fullname = createFieldOfStudyDto.fullName;
            // }
            // else{
            //     if(createFieldOfStudyDto.isBachelor){
            //         _fullname = createFieldOfStudyDto.name+"-"+"BACH"+"-"+createFieldOfStudyDto.year;
            //     } else {
            //         _fullname = createFieldOfStudyDto.name+"-"+"MASTER"+"-"+createFieldOfStudyDto.year;
            //     }
            // }

            // FieldOfStudy fieldOfStudy = new()
            //     {
            //         fullName = _fullname,
            //         name = createFieldOfStudyDto.name,
            //         isBachelor = createFieldOfStudyDto.isBachelor,
            //         year = createFieldOfStudyDto.year,
            //     };            
    
            // await _fieldOfStudyRepository.createAsync(fieldOfStudy);
            return Ok();
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFieldOfStudy(int id)
        {
            await _fieldOfStudyRepository.deleteAsync(id);
            return Ok();
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFieldOfStudy(int id, FieldOfStudyDto updateFieldOfStudyDto)
        {
            FieldOfStudy fieldOfStudy = new()
            {
                id = id,
                fullName = updateFieldOfStudyDto.fullName,
                name = updateFieldOfStudyDto.name,
                isBachelor = updateFieldOfStudyDto.isBachelor,
                year = updateFieldOfStudyDto.year,
            };
    
            await _fieldOfStudyRepository.updateAsync(fieldOfStudy);
            return Ok();
        }
    }
}