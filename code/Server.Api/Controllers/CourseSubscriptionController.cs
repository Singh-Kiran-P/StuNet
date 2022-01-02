using System;
using System.Linq;
using ChatSample.Hubs;
using Server.Api.Dtos;
using Server.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseSubscriptionController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ICourseRepository _courseRepository;
        private readonly ISubscriptionRepository<CourseSubscription> _courseSubscriptionRepository;

        public CourseSubscriptionController(ISubscriptionRepository<CourseSubscription> subscriptionRepository, ICourseRepository courseRepository, UserManager<User> userManager, IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _courseRepository = courseRepository;
            _courseSubscriptionRepository = subscriptionRepository;
        }

        [Authorize(Roles = "student,prof")]
        private async Task<IEnumerable<GetCourseSubscriptionDto>> _GetCourseSubscriptions()
        {
            IEnumerable<CourseSubscription> subscriptions = await _courseSubscriptionRepository.GetAllAsync();
            IEnumerable<GetCourseSubscriptionDto> getDtos = subscriptions.Select(subscription => GetCourseSubscriptionDto.Convert(subscription));
            return getDtos;
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCourseSubscriptionDto>>> GetCourseSubscriptions()
        {
            IEnumerable<GetCourseSubscriptionDto> getDtos = await _GetCourseSubscriptions();
            return Ok(getDtos);
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseSubscriptionDto>> GetCourseSubscription(int id)
        {
            CourseSubscription subscription = await _courseSubscriptionRepository.GetAsync(id);
            if (subscription == null) return NotFound();
            return Ok(GetCourseSubscriptionDto.Convert(subscription));
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("ByUserAndCourseId/{courseId}")]
        public async Task<ActionResult<GetByIdsCourseSubscriptionDto>> GetCourseSubscriptionByUserAndCourseId(int courseId)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "username")) return Unauthorized();
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);
            IEnumerable<CourseSubscription> subscriptions = await _courseSubscriptionRepository.GetAllAsync();
            IEnumerable<GetByIdsCourseSubscriptionDto> userSubscriptionDtos = subscriptions
                .Where(subscription => subscription.subscribedItemId == courseId && subscription.userId == user.Id)
                .Select(subscription => GetByIdsCourseSubscriptionDto.Convert(subscription));
            return Ok(userSubscriptionDtos);
        }

        [Authorize(Roles = "student,prof")]
        [HttpPost]
        public async Task<ActionResult<CreateCourseSubscriptionDto>> CreateCourseSubscription(CreateCourseSubscriptionDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "username")) return Unauthorized();
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);
            CourseSubscription subscription = new() {
                userId = user.Id,
                dateTime = DateTime.UtcNow,
                subscribedItemId = dto.courseId,
                subscribedItem = await _courseRepository.GetAsync(dto.courseId)
            };

            await _hubContext.Groups.AddToGroupAsync(UserHandler.ConnectedIds[user.Id], "Course " + subscription.subscribedItemId.ToString());
            await _courseSubscriptionRepository.CreateAsync(subscription);
            return Ok(subscription);
        }

        [Authorize(Roles = "student,prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourseSubscription(int id)
        {
            var existing = await _courseSubscriptionRepository.GetAsync(id);
            if (existing == null) return NotFound();
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "username")) return Unauthorized();
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);
            await _hubContext.Groups.RemoveFromGroupAsync(UserHandler.ConnectedIds[user.Id], "Course " + existing.subscribedItemId.ToString());
            await _courseSubscriptionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
