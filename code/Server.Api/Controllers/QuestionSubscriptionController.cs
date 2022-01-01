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
    public class QuestionSubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository<QuestionSubscription> _questionSubscriptionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public QuestionSubscriptionController(ISubscriptionRepository<QuestionSubscription> subscriptionRepository, IQuestionRepository questionRepository, UserManager<User> userManager, IHubContext<ChatHub> hubContext)
        {
            _questionSubscriptionRepository = subscriptionRepository;
            _questionRepository = questionRepository;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        private async Task<IEnumerable<GetQuestionSubscriptionDto>> _GetQuestionSubscriptions()
        {
            IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.GetAllAsync();
            IEnumerable<GetQuestionSubscriptionDto> getDtos = subscriptions.Select(subscription => GetQuestionSubscriptionDto.Convert(subscription));
            return getDtos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetQuestionSubscriptionDto>>> GetQuestionSubscriptions()
        {
            IEnumerable<GetQuestionSubscriptionDto> getDtos = await _GetQuestionSubscriptions();
            return Ok(getDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetQuestionSubscriptionDto>> GetQuestionSubscription(int id)
        {
            QuestionSubscription subscription = await _questionSubscriptionRepository.GetAsync(id);
            if (subscription == null)
                return NotFound();

            return Ok(GetQuestionSubscriptionDto.Convert(subscription));
        }

        [HttpGet("ByUserAndQuestionId/{questionId}")] //FIXME: Make route lower case
        public async Task<ActionResult<GetByIdsQuestionSubscriptionDto>> GetQuestionSubscriptionByUserAndQuestionId(int questionId)
        {
            IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.GetAllAsync();
            ClaimsPrincipal currentUser = HttpContext.User;
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);

            IEnumerable<GetByIdsQuestionSubscriptionDto> userSubscriptionDtos = subscriptions
                .Where(subscription => subscription.subscribedItemId == questionId && subscription.userId == user.Id)
                .Select(subscription => GetByIdsQuestionSubscriptionDto.Convert(subscription));
            return Ok(userSubscriptionDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CreateQuestionSubscriptionDto>> CreateQuestionSubscription(CreateQuestionSubscriptionDto dto)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                User user = await _userManager.FindByEmailAsync(userEmail);

                QuestionSubscription subscription = new()
                {
                    dateTime = DateTime.UtcNow,
                    userId = user.Id,
                    subscribedItem = await _questionRepository.GetAsync(dto.questionId),
                };

                await _hubContext.Groups.AddToGroupAsync(UserHandler.ConnectedIds[user.Id], "Question " + subscription.subscribedItemId.ToString());
                await _questionSubscriptionRepository.CreateAsync(subscription);
                return Ok(subscription);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestionSubscription(int id)
        {
            try
            {
                QuestionSubscription sub = await _questionSubscriptionRepository.GetAsync(id);
                ClaimsPrincipal currentUser = HttpContext.User;
                if (currentUser.HasClaim(c => c.Type == "username"))
                {
                    string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                    User user = await _userManager.FindByEmailAsync(userEmail);

                    await _hubContext.Groups.RemoveFromGroupAsync(UserHandler.ConnectedIds[user.Id], "Question " + sub.subscribedItemId.ToString());
                    await _questionSubscriptionRepository.DeleteAsync(id);
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
            return NoContent();
        }
    }
}
