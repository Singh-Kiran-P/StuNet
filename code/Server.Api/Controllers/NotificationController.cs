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
using Microsoft.AspNetCore.SignalR;
using ChatSample.Hubs;


namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IQuestionNotificationRepository _questionNotificationRepository;
        // private readonly ICourseNotificationRepository _courseNotificationRepository;

		public CourseSubscriptionController(IQuestionNotificationRepository repository)
        {
            _questionNotificationRepository = repository;
		}
        
        private async Task<IEnumerable<NotificationDto>> _getSubscriptions()
        {
            IEnumerable<QuestionNotification> notifications = await _questionNotificationRepository.getAllAsync();
            IEnumerable<NotificationDto> getDtos = subscriptions.Select(subscription => NotificationDto.convert(notifications));
            return getDtos;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> getCourseSubscriptions()
        {
            IEnumerable<NotificationDto> getDtos = await _getSubscriptions();
            return Ok(getDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetSubscription(int id)
        {
            QuestionNotification notification = await _questionNotificationRepository.getAsync(id);
            if (subscription == null)
                return NotFound();

            return Ok(NotificationDto.convert(notification));
        }

        [HttpGet("ByUserAndCourseId/{courseId}")]
        public async Task<ActionResult<getByIdsCourseSubscriptionDto>> GetCourseSubscriptionByUserAndCourseId(int courseId)
        {
            IEnumerable<CourseSubscription> notifications = await _questionNotificationRepository.getAllAsync();
            ClaimsPrincipal currentUser = HttpContext.User;
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);

            IEnumerable<getByIdsCourseSubscriptionDto> userSubscriptionDtos = subscriptions
                .Where(subscription => subscription.courseId == courseId && subscription.userId == user.Id)
                .Select(subscription => getByIdsCourseSubscriptionDto.convert(subscription));
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
				await _hubContext.Groups.AddToGroupAsync(UserHandler.ConnectedIds[user.Id], "Course " + subscription.courseId.ToString());
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
				CourseSubscription sub = await _courseSubscriptionRepository.getAsync(id);
				ClaimsPrincipal currentUser = HttpContext.User;
				if (currentUser.HasClaim(c => c.Type == "username"))
				{
					string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
					User user = await _userManager.FindByEmailAsync(userEmail);

					await _hubContext.Groups.RemoveFromGroupAsync(UserHandler.ConnectedIds[user.Id], "Course " + sub.courseId.ToString());
					await _courseSubscriptionRepository.deleteAsync(id);
				} else {
                    return Unauthorized();
                }
			}
            catch (System.Exception)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
