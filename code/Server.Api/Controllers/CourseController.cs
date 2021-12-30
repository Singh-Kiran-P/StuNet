// @Kiran @Tijl @Melih
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using FuzzySharp;
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
        private readonly ISubscriptionRepository<CourseSubscription> _subscriptionRepository;

        public CourseController(ICourseRepository repository, ITopicRepository topicRepository, ISubscriptionRepository<CourseSubscription> subscriptionRepository)
        {
            _courseRepository = repository;
            _topicRepository = topicRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        private async Task<IEnumerable<GetAllCourseDto>> _GetCourseAsync()
        {
            IEnumerable<Course> courses = await _courseRepository.GetAllAsync();
            return courses.Select(c => GetAllCourseDto.Convert(c));
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
                IEnumerable<Course> subscribedCourses = subscriptions.Select(sub => sub.subscribedItem);

                return Ok(subscribedCourses.Select(c => GetAllCourseDto.Convert(c)));
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

            return Ok(GetCourseDto.Convert(course));
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
