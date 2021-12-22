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
        private readonly IAnswerNotificationRepository _answerNotificationRepository;

		public NotificationController(IQuestionNotificationRepository questionNotificationRepository, IAnswerNotificationRepository answerNotificationRepository)
        {
            _questionNotificationRepository = questionNotificationRepository;
			_answerNotificationRepository = answerNotificationRepository;
		}

        private async Task<IEnumerable<QuestionNotification>> _getQuestionNotifications(string userId) 
        {
            return await _questionNotificationRepository.getByUserId(userId);

        }

        private async Task<IEnumerable<AnswerNotification>> _getAnswerNotifications(string userId) 
        {
            return await _answerNotificationRepository.getByUserId(userId);
		}
        
        [HttpGet]
        public async Task<ActionResult<(IEnumerable<NotificationDto>, IEnumerable<NotificationDto>)>> getNotifications()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
			if (currentUser.HasClaim(c => c.Type == "userref"))
			{
				string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
				IEnumerable<QuestionNotification> qNotifs = await _getQuestionNotifications(userId);
				IEnumerable<AnswerNotification> cNotifs = await _getAnswerNotifications(userId);

				return Ok((qNotifs.Select(n => NotificationDto.convert(n)), cNotifs.Select(n => NotificationDto.convert(n))));
			} else {
				return Unauthorized();
			}
		}

        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeleteCourseNotification(int id)
        // {
        //     try
        //     {
        //         await _courseNotificationRepository.deleteAsync(id);
		// 	}
        //     catch (System.Exception)
        //     {
        //         return NotFound();
        //     }
        //     return NoContent();
        // }
    }
}
