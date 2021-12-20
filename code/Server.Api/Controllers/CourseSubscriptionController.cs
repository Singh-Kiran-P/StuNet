// @Tijl @Melih
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public CourseSubscriptionController(ICourseSubscriptionRepository repository)
        {
            _courseSubscriptionRepository = repository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<getCourseSubscriptionDto>>> getCourseSubscriptions()
        {
            IEnumerable<CourseSubscription> getDtos = await _courseSubscriptionRepository.getAllAsync();
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

        [HttpPost]
        public async Task<ActionResult<createCourseSubscriptionDto>> createCourseSubscription(createCourseSubscriptionDto dto)
        {
            CourseSubscription subscription = new()
            {
                dateTime = DateTime.UtcNow,
                userId = dto.userId,
                courseId = dto.courseId,
            };
            await _courseSubscriptionRepository.createAsync(subscription);
            return Ok(subscription);
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
