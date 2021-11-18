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
        private readonly ICourseRepository _repository;

        public CourseController(ICourseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> getCourses()
        {
            var courses = await _repository.getAllAsync();
            return Ok(courses);
        }

        // [HttpGet("{id}")]
        // public async Task

        [HttpPost]
        public async Task<ActionResult> createCourse(CourseDto dto)
        {
            Course course = new()
            {
                Name = dto.Name,
                Number = dto.Number,
            };

            await _repository.createAsync(course);
            return Ok();
        }

    }
}