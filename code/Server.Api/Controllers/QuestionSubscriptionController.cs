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
    public class QuestionSubscriptionController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IQuestionRepository _questionRepository;
        private readonly ISubscriptionRepository<QuestionSubscription> _questionSubscriptionRepository;

        public QuestionSubscriptionController(ISubscriptionRepository<QuestionSubscription> subscriptionRepository, IQuestionRepository questionRepository, UserManager<User> userManager, IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _questionRepository = questionRepository;
            _questionSubscriptionRepository = subscriptionRepository;
        }

        private async Task<IEnumerable<GetQuestionSubscriptionDto>> _GetQuestionSubscriptions()
        {
            IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.GetAllAsync();
            IEnumerable<GetQuestionSubscriptionDto> getDtos = subscriptions.Select(subscription => GetQuestionSubscriptionDto.Convert(subscription));
            return getDtos;
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetQuestionSubscriptionDto>>> GetQuestionSubscriptions()
        {
            IEnumerable<GetQuestionSubscriptionDto> getDtos = await _GetQuestionSubscriptions();
            return Ok(getDtos);
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetQuestionSubscriptionDto>> GetQuestionSubscription(int id)
        {
            QuestionSubscription subscription = await _questionSubscriptionRepository.GetAsync(id);
            if (subscription == null) return NotFound();
            return Ok(GetQuestionSubscriptionDto.Convert(subscription));
        }

        [Authorize(Roles = "student,prof")]
        [HttpGet("ByUserAndQuestionId/{questionId}")]
        public async Task<ActionResult<GetByIdsQuestionSubscriptionDto>> GetQuestionSubscriptionByUserAndQuestionId(int questionId)
        {
            IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.GetAllAsync();
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "username")) return Unauthorized();
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);
            IEnumerable<GetByIdsQuestionSubscriptionDto> userSubscriptionDtos = subscriptions
                .Where(subscription => subscription.subscribedItemId == questionId && subscription.userId == user.Id)
                .Select(subscription => GetByIdsQuestionSubscriptionDto.Convert(subscription));
            return Ok(userSubscriptionDtos);
        }

        [Authorize(Roles = "student,prof")]
        [HttpPost]
        public async Task<ActionResult<CreateQuestionSubscriptionDto>> CreateQuestionSubscription(CreateQuestionSubscriptionDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "username")) return Unauthorized();
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);
            QuestionSubscription subscription = new() {
                userId = user.Id,
                dateTime = DateTime.UtcNow,
                subscribedItemId = dto.questionId,
                subscribedItem = await _questionRepository.GetAsync(dto.questionId),
            };

            await _hubContext.Groups.AddToGroupAsync(UserHandler.ConnectedIds[user.Id], "Question " + subscription.subscribedItemId.ToString());
            await _questionSubscriptionRepository.CreateAsync(subscription);
            return Ok(subscription);
        }

        [Authorize(Roles = "student,prof")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestionSubscription(int id)
        {
            var existing = await _questionSubscriptionRepository.GetAsync(id);
            if (existing == null) return NotFound();
            ClaimsPrincipal currentUser = HttpContext.User;
            if (!currentUser.HasClaim(c => c.Type == "username")) return Unauthorized();
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);
            await _hubContext.Groups.RemoveFromGroupAsync(UserHandler.ConnectedIds[user.Id], "Question " + existing.subscribedItemId.ToString());
            await _questionSubscriptionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
