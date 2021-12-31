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
using Microsoft.AspNetCore.SignalR;
using ChatSample.Hubs;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseSubscriptionController : ControllerBase
    {
        private readonly ICourseSubscriptionRepository _courseSubscriptionRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public CourseSubscriptionController(ICourseSubscriptionRepository repository, UserManager<User> userManager, IHubContext<ChatHub> hubContext)
        {
            _courseSubscriptionRepository = repository;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        private async Task<IEnumerable<GetCourseSubscriptionDto>> _GetCourseSubscriptions()
        {
            IEnumerable<CourseSubscription> subscriptions = await _courseSubscriptionRepository.GetAllAsync();
            IEnumerable<GetCourseSubscriptionDto> getDtos = subscriptions.Select(subscription => GetCourseSubscriptionDto.Convert(subscription));
            return getDtos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCourseSubscriptionDto>>> GetCourseSubscriptions()
        {
            IEnumerable<GetCourseSubscriptionDto> getDtos = await _GetCourseSubscriptions();
            return Ok(getDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseSubscriptionDto>> GetCourseSubscription(int id)
        {
            CourseSubscription subscription = await _courseSubscriptionRepository.GetAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            GetCourseSubscriptionDto getDto = new()
            {
                dateTime = subscription.dateTime,
                userId = subscription.userId,
                courseId = subscription.courseId,
            };
            return Ok(getDto);
        }

        [HttpGet("ByUserAndCourseId/{courseId}")] //FIXME: Make route lower case
        public async Task<ActionResult<GetByIdsCourseSubscriptionDto>> GetCourseSubscriptionByUserAndCourseId(int courseId)
        {
            IEnumerable<CourseSubscription> subscriptions = await _courseSubscriptionRepository.GetAllAsync();
            ClaimsPrincipal currentUser = HttpContext.User;
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);

            IEnumerable<GetByIdsCourseSubscriptionDto> userSubscriptionDtos = subscriptions
                .Where(subscription => subscription.courseId == courseId && subscription.userId == user.Id)
                .Select(subscription => GetByIdsCourseSubscriptionDto.Convert(subscription));
            return Ok(userSubscriptionDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CreateCourseSubscriptionDto>> CreateCourseSubscription(CreateCourseSubscriptionDto dto)
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
                await _hubContext.Groups.AddToGroupAsync(UserHandler.ConnectedIds[user.Id], "Course " + subscription.courseId.ToString());
                await _courseSubscriptionRepository.CreateAsync(subscription);
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
                CourseSubscription sub = await _courseSubscriptionRepository.GetAsync(id);
                ClaimsPrincipal currentUser = HttpContext.User;
                if (currentUser.HasClaim(c => c.Type == "username"))
                {
                    string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                    User user = await _userManager.FindByEmailAsync(userEmail);

                    await _hubContext.Groups.RemoveFromGroupAsync(UserHandler.ConnectedIds[user.Id], "Course " + sub.courseId.ToString());
                    await _courseSubscriptionRepository.DeleteAsync(id);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (System.Exception)
            {
                return NotFound();
            }
            return NoContent(); //FIXME: this trailing return doesn't make sense, right?
        }
    }
}
