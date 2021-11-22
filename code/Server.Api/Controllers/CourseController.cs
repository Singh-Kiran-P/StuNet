using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCourseDto>>> getCourses()
        {
            IEnumerable<Course> courses = await _courseRepository.getAllAsync();
            IEnumerable<GetCourseDto> getDtos = courses.Select(course =>
                new GetCourseDto()
                {
                    name = course.name,
                    number = course.number,
                    topics = course.topics.Select(topic =>
                        new getOnlyTopicDto(){ name = topic.name, id = topic.id }
                    ).ToList(),
                }
            );
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
                name = course.name,
                number = course.number,
                topics = course.topics.Select(topic =>
                    new getOnlyTopicDto(){ name = topic.name, id = topic.id }
                ).ToList(),
            };
    
            return Ok(getDto);
        }

        [HttpPost]
        public async Task<ActionResult<Course>> createCourse(createCourseDto dto)
        {
            Course course = new()
            {
                name = dto.name,
                number = dto.number,
                topics = dto.topicNames.Select(name => new Topic(){ name = name }).ToList(),
            };
            await _courseRepository.createAsync(course);

            foreach (var topic in course.topics) {
                topic.course = course;
                await _topicRepository.updateAsync(topic);
            }
            return Ok(course);
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
            return NoContent();
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, CourseDto courseDto)
        {
            Course course = new()
            {
                id = id,
                name = courseDto.name,
                number = courseDto.number                
            };
    
            await _courseRepository.updateAsync(course);
            return NoContent();
        }
    }
}