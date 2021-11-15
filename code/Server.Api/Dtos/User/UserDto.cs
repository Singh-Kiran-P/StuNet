using System;

namespace Server.Api.Dtos
{
    public record UserDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}