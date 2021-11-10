using System;

namespace Server.Api.Controllers
{
    public record UserDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}