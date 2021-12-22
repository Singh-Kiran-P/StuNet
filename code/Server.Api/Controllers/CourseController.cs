using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FuzzySharp;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Services;
namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;

        public CourseController(ICourseRepository repository, ITopicRepository topicRepository)
        {
            _courseRepository = repository;
            _topicRepository = topicRepository;
        }

        private async Task<IEnumerable<GetAllCourseDto>> _getCourseAsync()
        {
            IEnumerable<Course> courses = await _courseRepository.getAllAsync();
            IEnumerable<GetAllCourseDto> getDtos = courses.Select(course =>
               new GetAllCourseDto()
               {
                   id = course.id,
                   name = course.name,
                   number = course.number,
                   description = course.description,
                   courseEmail = course.courseEmail,
                   profEmail = course.profEmail,
                   topics = course.topics.Select(topic =>
                       new getOnlyTopicDto() { name = topic.name, id = topic.id }
                        ).ToList(),
               }
            );
            return getDtos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllCourseDto>>> getCourses()
        {
            IEnumerable<GetAllCourseDto> getDtos = await _getCourseAsync();
            return Ok(getDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseDto>> GetCourse(int id)
        {
            Course course = await _courseRepository.getAsync(id);
            if (course == null)
                return NotFound();

            GetCourseDto getDto = new()
            {
                id = course.id,
                name = course.name,
                number = course.number,
                description = course.description,
                courseEmail = course.courseEmail,
                profEmail = course.profEmail,
                topics = course.topics.Select(topic =>
                    new getOnlyTopicDto() { id = topic.id, name = topic.name }
                ).ToList(),
                channels = course.channels.Select(channel =>
                    new getOnlyChannelDto() { id = channel.id, name = channel.name }
                ).ToList()
            };

            return Ok(getDto);
        }

        [HttpGet("search/")]
        public async Task<ActionResult<GetCourseDto>> searchByName([FromQuery] string name)
        {
            IEnumerable<GetAllCourseDto> getDtos = await _getCourseAsync();

            IEnumerable<GetAllCourseDto> searchResults = StringMatcher.FuzzyMatchObject(getDtos, name);

            if (searchResults == null || !searchResults.Any())
                return NoContent();
            else
                return Ok(searchResults);
        }

        [HttpPost]
        public async Task<ActionResult<Course>> createCourse(createCourseDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                Course course = new()
                {
                    name = dto.name,
                    number = dto.number,
                    description = dto.description,
                    courseEmail = dto.courseEmail,
                    profEmail = userEmail
                };
                await _courseRepository.createAsync(course);
                return Ok(course);
            } else {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseRepository.deleteAsync(id);
            }
            catch (System.Exception)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, CourseDto courseDto)
        {
            Course course = new()
            {
                id = id,
                name = courseDto.name,
                number = courseDto.number,
                description = courseDto.description,
                courseEmail = courseDto.courseEmail,
                profEmail = courseDto.profEmail
            };

            await _courseRepository.updateAsync(course);
            return NoContent();
        }
    }
}
