// @Kiran @Senn

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
    public class UserController : ControllerBase
    {
        //     private readonly IUserRepository _userRepository;
        //     public UserController(IUserRepository userRepository)
        //     {
        //         _userRepository = userRepository;
        //     }

        //     [HttpGet]
        //     public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //     {
        //         var users = await _userRepository.getAllAsync();
        //         return Ok(users);
        //     }

        //     [HttpGet("{id}")]
        //     public async Task<ActionResult<User>> GetUser(int id)
        //     {
        //         var user = await _userRepository.getAsync(id);
        //         if(user == null)
        //             return NotFound();

        //         return Ok(user);
        //     }

        //     [HttpPost]
        //     public async Task<ActionResult> CreateUser(LoginUserDto createUserDto)
        //     {
        //         User user = new()
        //         {
        //             email = createUserDto.Email,
        //             password = createUserDto.Password,
        //         };

        //         await _userRepository.createAsync(user);
        //         return Ok();
        //     }

        //     [HttpDelete("{id}")]
        //     public async Task<ActionResult> DeleteUser(int id)
        //     {
        //         await _userRepository.deleteAsync(id);
        //         return Ok();
        //     }

        //     [HttpPut("{id}")]
        //     public async Task<ActionResult> UpdateUser(int id, LoginUserDto updateUserDto)
        //     {
        //         User user = new()
        //         {
        //             id = id,
        //             email = updateUserDto.Email,
        //             password = updateUserDto.Password
        //         };

        //         await _userRepository.updateAsync(user);
        //         return Ok();
        //     }
    }
}
