// @Tijl @Melih
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseSubscriptionController : ControllerBase
    {
        private readonly ICourseSubscriptionRepository _courseSubscriptionRepository;
        private readonly UserManager<User> _userManager;

        public CourseSubscriptionController(ICourseSubscriptionRepository repository, UserManager<User> userManager)
        {
            _courseSubscriptionRepository = repository;
            _userManager = userManager;
        }
        
        private async Task<IEnumerable<getCourseSubscriptionDto>> _getCourseSubscriptions()
        {
            IEnumerable<CourseSubscription> subscriptions = await _courseSubscriptionRepository.getAllAsync();
            IEnumerable<getCourseSubscriptionDto> getDtos = subscriptions.Select(subscription => getCourseSubscriptionDto.convert(subscription));
            return getDtos;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<getCourseSubscriptionDto>>> getCourseSubscriptions()
        {
            IEnumerable<getCourseSubscriptionDto> getDtos = await _getCourseSubscriptions();
            return Ok(getDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<getCourseSubscriptionDto>> GetCourseSubscription(int id)
        {
            CourseSubscription subscription = await _courseSubscriptionRepository.getAsync(id);
            if (subscription == null)
                return NotFound();

            getCourseSubscriptionDto getDto = new()
            {
                dateTime = subscription.dateTime,
                userId = subscription.userId,
                courseId = subscription.courseId,
            };

            return Ok(getDto);
        }

        [HttpGet("ByUserAndCourseId/{courseId}")]
        public async Task<ActionResult<getCourseSubscriptionDto>> GetCourseSubscriptionByUserAndCourseId(int courseId)
        {
            IEnumerable<getCourseSubscriptionDto> dtos = await _getCourseSubscriptions();
            ClaimsPrincipal currentUser = HttpContext.User;
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);

            IEnumerable<getCourseSubscriptionDto> userSubscriptionDtos = dtos.Where(dto => dto.courseId == courseId && dto.userId == user.Id);
            return Ok(userSubscriptionDtos);
        }

        [HttpPost]
        public async Task<ActionResult<createCourseSubscriptionDto>> createCourseSubscription(createCourseSubscriptionDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                User user = await _userManager.FindByEmailAsync(userEmail);
                
                CourseSubscription subscription = new()
                {
                    dateTime = DateTime.UtcNow,
                    userId = user.Id,
                    courseId = dto.courseId,
                };
                await _courseSubscriptionRepository.createAsync(subscription);
                return Ok(subscription);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourseSubscription(int id)
        {
            try
            {
                await _courseSubscriptionRepository.deleteAsync(id);
            }
            catch (System.Exception)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
