// @Tijl @Melih
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository<QuestionNotification> _questionNotificationRepository;
        private readonly INotificationRepository<AnswerNotification> _answerNotificationRepository;

        public NotificationController(INotificationRepository<QuestionNotification> questionNotificationRepository, INotificationRepository<AnswerNotification> answerNotificationRepository)
        {
            _questionNotificationRepository = questionNotificationRepository;
            _answerNotificationRepository = answerNotificationRepository;
        }

        private async Task<IEnumerable<QuestionNotification>> _GetQuestionNotifications(string userId)
        {
            return await _questionNotificationRepository.GetByUserId(userId);
        }

        private async Task<IEnumerable<AnswerNotification>> _GetAnswerNotifications(string userId)
        {
            return await _answerNotificationRepository.GetByUserId(userId);
        }

        [HttpGet]
        public async Task<ActionResult<(IEnumerable<GetNotificationDto>, IEnumerable<GetNotificationDto>)>> GetNotifications()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "userref"))
            {
                string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
                IEnumerable<QuestionNotification> qNotifs = await _GetQuestionNotifications(userId);
                IEnumerable<AnswerNotification> cNotifs = await _GetAnswerNotifications(userId);

                return Ok((qNotifs.Select(n => GetNotificationDto.Convert(n)), cNotifs.Select(n => GetNotificationDto.Convert(n))));
            }
            else
            {
                return Unauthorized();
            }
        }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeleteCourseNotification(int id)
        // {
        //     try
        //     {
        //         await _courseNotificationRepository.deleteAsync(id);
        //     }
        //     catch (System.Exception)
        //     {
        //         return NotFound();
        //     }
        //     return NoContent();
        // }
    }
}
