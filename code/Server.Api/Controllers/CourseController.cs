using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public CourseController(ICourseRepository repository, ITopicRepository topicRepository, ISubscriptionRepository<CourseSubscription> subscriptionRepository,UserManager<User> userManager)
        {
            _courseRepository = repository;
            _topicRepository = topicRepository;
            _subscriptionRepository = subscriptionRepository;
            _userManager = userManager;
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

        [HttpGet("getSubscribedByEmail")]
        public async Task<ActionResult<IEnumerable<GetAllCourseDto>>> GetSubscribedCoursesByEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user != null) 
            {
                IEnumerable<CourseSubscription> subscriptions = await _subscriptionRepository.GetByUserId(user.Id);
                IEnumerable<Course> subscribedCourses = subscriptions.Select(sub => sub.subscribedItem);
                return Ok(subscribedCourses.Select(c => GetAllCourseDto.Convert(c)));
            }
            else
            {
                return NotFound();
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
                await _courseRepository.CreateAsync(course);
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
                description = courseDto.description,
                courseEmail = courseDto.courseEmail,
                profEmail = courseDto.profEmail
            };
            await _courseRepository.UpdateAsync(course);
            return NoContent();
        }
    }
}
