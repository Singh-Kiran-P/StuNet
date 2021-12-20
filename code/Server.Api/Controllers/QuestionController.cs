using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly UserManager<User> _userManager;
        public QuestionController(IQuestionRepository questionRepository, ITopicRepository topicRepository, ICourseRepository courseRepository, UserManager<User> userManager)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
            _userManager = userManager;
        }

        // private static questionDto toDto(Question question, User user)
        // {
        //     return new questionDto
        //     {
        //         id = question.id,
        //         user = question.user,
        //         course = new getOnlyCourseDto
        //         {
        //             id = question.course.id,
        //             name = question.course.name,
        //             number = question.course.number,
        //         },
        //         title = question.title,
        //         body = question.body,
        //         topics = question.topics.Select(topic => new getOnlyTopicDto
        //         {
        //             id = topic.id,
        //             name = topic.name
        //         }).ToList(),
        //         time = question.dateTime
        //     };
        // }

        

        //[Authorize(Roles = "student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<questionDto>>> GetQuestions()
        {
            // var questions = await _questionRepository.getAllAsync();
            // return Ok(questions.Select(question => questionDto.convert(question)));
            try
            {
                var questions = await _questionRepository.getAllAsync();
                List<questionDto> res = new List<questionDto>();
                foreach (var q in questions) {
                    User user = await _userManager.FindByIdAsync(q.userId);
                    res.Add(questionDto.convert(q, user));
                }
                return Ok(res);
            }
            catch { return BadRequest("Error finding all questions"); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<questionDto>> GetQuestion(int id)
        {
            // var question = await _questionRepository.getAsync(id);
            // if (question == null)
            //     return NotFound();
            // return Ok(toDto(question));
            try
            {
                var question = await _questionRepository.getAsync(id);
                if (question == null)
                    return NotFound();
                User user = await _userManager.FindByIdAsync(question.userId);
            
                return Ok(questionDto.convert(question, user));
            }
            catch { return BadRequest("Error finding question"); }
        }

        [HttpGet("getFile/{id}")]
        public async Task<ActionResult<questionDto>> GetFile(int id)
        {
            // var question = await _questionRepository.getAsync(id);
            // if (question == null)
            //     return NotFound();
            // return Ok(toDto(question));
            try
            {
                var question = await _questionRepository.getAsync(id);
                if (question == null)
                    return NotFound();
                User user = await _userManager.FindByIdAsync(question.userId);
            
                return Ok(questionDto.convert(question, user));
            }
            catch { return BadRequest("Error finding question"); }
        }
    
        // [HttpPost("UploadFile")]
        // public async Task<ActionResult> UploadFile([FromForm] fileDto dto){
        //     System.Console.WriteLine("In UPLOAD FILE");
        //     foreach( var title in dto.titles){
        //         System.Console.WriteLine(title);
        //     }
        //     string fileName;
        //     foreach( var formFile in dto.files){
        //         var extension = "."+formFile.FileName.Split('.')[formFile.FileName.Split('.').Length -1];
        //         fileName = DateTime.Now.Ticks +"_"+ formFile.FileName;//+ extension; // Security reasons
        //         if (formFile.Length > 0){
        //             var filePath = Path.Combine(Directory.GetCurrentDirectory(),"Upload");
                    
        //             if(!Directory.Exists(filePath))
        //             {
        //                 Directory.CreateDirectory(filePath);
        //             }
        //             var path = Path.Combine(Directory.GetCurrentDirectory(),"Upload", fileName);
        //             using (var stream = new FileStream(path, FileMode.Create)){
        //                 await formFile.CopyToAsync(stream);
        //             }
        //         }
        //     }
        //     return NoContent();
        // }
        private async Task<List<String>> uploadFile(ICollection<IFormFile> files, String questionId){
            string fileName;
            List<String> paths = new List<string>();
            foreach( var formFile in files){
                var extension = "."+formFile.FileName.Split('.')[formFile.FileName.Split('.').Length -1];
                fileName = DateTime.Now.Ticks +"_"+ formFile.FileName;//+ extension; // Security reasons
                if (formFile.Length > 0){
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),"Upload\\Questions", questionId);
                    System.Console.WriteLine(filePath);
                    
                    if(!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"Upload\\Questions", questionId, fileName);
                    var pathUniversal = Path.Combine("Upload\\Questions", questionId, fileName);
                    using (var stream = new FileStream(path, FileMode.Create)){
                        await formFile.CopyToAsync(stream);
                    }
                    paths.Add(pathUniversal);
                }
            }
            return paths;
        }

        //[Authorize(Roles = "student")]
        [HttpPost]
        public async Task<ActionResult<questionDto>> CreateQuestion([FromForm] createQuestionDto dto)
        {
            System.Console.WriteLine("IN HERE");
            System.Console.WriteLine(dto.ToString());
            bool uploadFileBool = true;
            if(dto.files == null){
                dto.files = new List<IFormFile>();
                uploadFileBool = false;
            }
            if(dto.topicIds == null){
                dto.topicIds = new List<int>();
            }
            System.Console.WriteLine(dto.ToString());
            ClaimsPrincipal currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                System.Console.WriteLine("IN USER");
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                User user = await _userManager.FindByEmailAsync(userEmail);
                Course c = _courseRepository.getAsync(dto.courseId).Result;

                ICollection<Topic> topics = new List<Topic>();
                topics = dto.topicIds.Select(id => _topicRepository.getAsync(id)) //TODO: Dit is een probleem als 1 van de topics niet bestaat, er wordt niet null teruggegeven maar een lijst met een null in en dit gaat niet in de db; voorlopige oplossing zie lijn 78
                                                .Select(task => task.Result)
                                                .ToList();
                
                if (c==null) {return BadRequest("Course does not exist");}
                if (topics.Contains(null)) {return BadRequest("One of the topics does not exist");}
                
                Question question = new()
                {
                    title = dto.title,
                    userId = user.Id, //TODO
                    course = c,
                    body = dto.body,
                    filepaths = new List<String>(),
                    topics = topics,
                    dateTime = DateTime.Now
                };
                await _questionRepository.createAsync(question);
                if(uploadFileBool)
                {
                    question.filepaths = uploadFile(dto.files,question.id.ToString()).Result;
                    await _questionRepository.updateAsync(question);
                }
                return Ok(questionDto.convert(question, user));
            }
            else
            {
                return  Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            var existingQuestion = await _questionRepository.getAsync(id);
            if (existingQuestion is null)
            {
                return NotFound();
            }

            await _questionRepository.deleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, createQuestionDto dto)
        {

            var existingQuestion = await _questionRepository.getAsync(id);
            if (existingQuestion is null)
            {
                return NotFound();
            }

            Question updatedQuestion = new()
            {
                title = dto.title,
                // user = updateQuestionDto.user,
                course = _courseRepository.getAsync(dto.courseId).Result,
                body = dto.body,
                // files = updateQuestionDto.files
                topics = dto.topicIds.Select(id => _topicRepository.getAsync(id))
                                                .Select(task => task.Result)
                                                .ToList(),
                dateTime = DateTime.Now
            };

            await _questionRepository.updateAsync(updatedQuestion);
            return NoContent();
        }
    }
}
