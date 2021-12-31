using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Services;
using System.Security.Claims;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ICourseSubscriptionRepository _subscriptionRepository;

        public CourseController(ICourseRepository repository, ITopicRepository topicRepository, ICourseSubscriptionRepository subscriptionRepository)
        {
            _courseRepository = repository;
            _topicRepository = topicRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        private async Task<IEnumerable<GetAllCourseDto>> _GetCourseAsync()
        {
            IEnumerable<Course> courses = await _courseRepository.GetAllAsync();
            IEnumerable<GetAllCourseDto> getDtos = courses.Select(course =>
               new GetAllCourseDto()
               {
                   id = course.id,
                   name = course.name,
                   number = course.number,
                   description = course.description,
                   topics = course.topics.Select(topic => new GetPartialTopicDto() { name = topic.name, id = topic.id }).ToList()
               }
            );
            return getDtos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllCourseDto>>> GetCourses()
        {
            IEnumerable<GetAllCourseDto> getDtos = await _GetCourseAsync();
            return Ok(getDtos);
        }

        [HttpGet("subscribed")]
        public async Task<ActionResult<IEnumerable<GetAllCourseDto>>> GetSubscribedCourses()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "userref"))
            {
                string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
                IEnumerable<CourseSubscription> subscriptions = await _subscriptionRepository.GetByUserId(userId);
                IEnumerable<int> subscribedCourseIds = subscriptions.Select(sub => sub.courseId);
                IEnumerable<GetAllCourseDto> courses = await _GetCourseAsync();

                return Ok(courses.Where(course => subscribedCourseIds.Contains(course.id)));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseDto>> GetCourse(int id)
        {
            Course course = await _courseRepository.GetAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            GetCourseDto getDto = new()
            {
                id = course.id,
                name = course.name,
                number = course.number,
                description = course.description,
                topics = course.topics.Select(topic => new GetPartialTopicDto(){ id = topic.id, name = topic.name }).ToList(),
                channels = course.channels.Select(channel => new GetPartialChannelDto(){ id = channel.id, name = channel.name }).ToList()
            };
            return Ok(getDto);
        }


        [HttpGet("search/")]
        public async Task<ActionResult<GetCourseDto>> SearchByName([FromQuery] string name)
        {
            IEnumerable<GetAllCourseDto> getDtos = await _GetCourseAsync();
            IEnumerable<GetAllCourseDto> searchResults = StringMatcher.FuzzyMatchObject(getDtos, name);

            if (searchResults == null || !searchResults.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(searchResults);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(CreateCourseDto dto)
        {
            Course course = new()
            {
                name = dto.name,
                number = dto.number,
                description = dto.description
            };
            await _courseRepository.CreateAsync(course);
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseRepository.DeleteAsync(id);
            }
            catch (System.Exception)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, GetPartialCourseDto courseDto)
        {
            Course course = new()
            {
                id = id,
                name = courseDto.name,
                number = courseDto.number,
                description = courseDto.description
            };
            await _courseRepository.UpdateAsync(course);
            return NoContent();
        }
    }
}
