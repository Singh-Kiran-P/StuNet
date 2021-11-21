using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Dtos;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;

        public CourseController(ICourseRepository repository, ITopicRepository topicRepository)
        {
            _courseRepository = repository;
            _topicRepository = topicRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> getCourses()
        {
            var courses = await _courseRepository.getAllAsync();
            return Ok(courses);
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetCourse(int id)
        {
            Course course = await _courseRepository.getAsync(id);
            if (course == null)
                return NotFound();
    
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult> createCourse(createCourseDto dto)
        {
            Course course = new()
            {
                Name = dto.Name,
                Number = dto.Number,
                topics = new List<Topic>(),
                // Channels = dto.channels.Select(name => _channelRepository.createAsync(name))
				// 								.Select(task => task.Result)
				// 								.ToList(),
                // topics = dto.topicNames.Select(name => _topicRepository.createAsync(
                //     new Topic(
                //     {
                //         name = name
                //     })))
				// 	.Select(task => task.Result
            	// 	.ToList(),
            };
            
            await _courseRepository.createAsync(course);
            course = _courseRepository.getAsync(course.Id).Result;
            
            foreach(var topicName in dto.topicNames){
                Topic topic = new(){
                    name = topicName,
                    course = course
                };
                course.topics.Add(topic);
                await _topicRepository.createAsync(topic);
            }
            await _courseRepository.updateAsync(course);
            Console.WriteLine("CREATE COURSE FINISHED");
            return Ok();
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try {
                await _courseRepository.deleteAsync(id);
            }
            catch (System.Exception) {
                return NotFound();
            }
            return Ok();
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, CourseDto courseDto)
        {
            Course course = new()
            {
                Id = id,
                Name = courseDto.Name,
                Number = courseDto.Number                
            };
    
            await _courseRepository.updateAsync(course);
            return Ok();
        }
    }
}