using System;

namespace Server.Api.Dtos
{
    public record LoginUserDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}