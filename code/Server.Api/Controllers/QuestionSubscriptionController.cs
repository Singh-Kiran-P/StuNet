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
    public class QuestionSubscriptionController : ControllerBase
    {
        private readonly IQuestionSubscriptionRepository _questionSubscriptionRepository;
        private readonly UserManager<User> _userManager;

        public QuestionSubscriptionController(IQuestionSubscriptionRepository repository, UserManager<User> userManager)
        {
            _questionSubscriptionRepository = repository;
            _userManager = userManager;
        }
        
        private async Task<IEnumerable<getQuestionSubscriptionDto>> _getQuestionSubscriptions()
        {
            IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.getAllAsync();
            IEnumerable<getQuestionSubscriptionDto> getDtos = subscriptions.Select(subscription => getQuestionSubscriptionDto.convert(subscription));
            return getDtos;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<getQuestionSubscriptionDto>>> getQuestionSubscriptions()
        {
            IEnumerable<getQuestionSubscriptionDto> getDtos = await _getQuestionSubscriptions();
            return Ok(getDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<getQuestionSubscriptionDto>> GetQuestionSubscription(int id)
        {
            QuestionSubscription subscription = await _questionSubscriptionRepository.getAsync(id);
            if (subscription == null)
                return NotFound();

            getQuestionSubscriptionDto getDto = new()
            {
                dateTime = subscription.dateTime,
                userId = subscription.userId,
                questionId = subscription.questionId,
            };

            return Ok(getDto);
        }

        [HttpGet("ByUserAndQuestionId/{questionId}")]
        public async Task<ActionResult<getByIdsQuestionSubscriptionDto>> GetQuestionSubscriptionByUserAndQuestionId(int questionId)
        {
            IEnumerable<QuestionSubscription> subscriptions = await _questionSubscriptionRepository.getAllAsync();
            ClaimsPrincipal currentUser = HttpContext.User;
            string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            User user = await _userManager.FindByEmailAsync(userEmail);

            IEnumerable<getByIdsQuestionSubscriptionDto> userSubscriptionDtos = subscriptions
                .Where(subscription => subscription.questionId == questionId && subscription.userId == user.Id)
                .Select(subscription => getByIdsQuestionSubscriptionDto.convert(subscription));
            return Ok(userSubscriptionDtos);
        }

        [HttpPost]
        public async Task<ActionResult<createQuestionSubscriptionDto>> createQuestionSubscription(createQuestionSubscriptionDto dto)
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
                    questionId = dto.questionId,
                };
                await _questionSubscriptionRepository.createAsync(subscription);
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
                await _questionSubscriptionRepository.deleteAsync(id);
            }
            catch (System.Exception)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
