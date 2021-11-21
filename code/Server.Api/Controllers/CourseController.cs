using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository repository)
        {
            _courseRepository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> getCourses()
        {
            var courses = await _courseRepository.getAllAsync();
            return Ok(courses);
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetCourse(int id)
        {
            Course course = await _courseRepository.getAsync(id);
            if (course == null)
                return NotFound();
    
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult> createCourse(CourseDto dto)
        {
            Course course = new()
            {
                Name = dto.Name,
                Number = dto.Number,
            };

            await _courseRepository.createAsync(course);
            return Ok();
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try {
                await _courseRepository.deleteAsync(id);
            }
            catch (System.Exception) {
                return NotFound();
            }
            return Ok();
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, CourseDto courseDto)
        {
            Course course = new()
            {
                Id = id,
                Name = courseDto.Name,
                Number = courseDto.Number                
            };
    
            await _courseRepository.updateAsync(course);
            return Ok();
        }
    }
}