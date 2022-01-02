using System.Linq;
using Server.Api.Dtos;
using Server.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository<AnswerNotification> _answerNotificationRepository;
        private readonly INotificationRepository<QuestionNotification> _questionNotificationRepository;

        public NotificationController(INotificationRepository<QuestionNotification> questionNotificationRepository, INotificationRepository<AnswerNotification> answerNotificationRepository)
        {
            _answerNotificationRepository = answerNotificationRepository;
            _questionNotificationRepository = questionNotificationRepository;
        }

        private async Task<IEnumerable<QuestionNotification>> _GetQuestionNotifications(string userId)
        {
            return await _questionNotificationRepository.GetByUserId(userId);
        }

        private async Task<IEnumerable<AnswerNotification>> _GetAnswerNotifications(string userId)
        {
            return await _answerNotificationRepository.GetByUserId(userId);
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet]
        public async Task<ActionResult<(IEnumerable<GetQuestionNotificationDto>, IEnumerable<GetAnswerNotificationDto>)>> GetNotifications()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "userref")) return Unauthorized();
            string userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
            IEnumerable<QuestionNotification> qNotifs = await _GetQuestionNotifications(userId);
            IEnumerable<AnswerNotification> aNotifs = await _GetAnswerNotifications(userId);
            return Ok((qNotifs.Select(n => GetQuestionNotificationDto.Convert(n)), aNotifs.Select(n => GetAnswerNotificationDto.Convert(n))));
        }
    }
}
