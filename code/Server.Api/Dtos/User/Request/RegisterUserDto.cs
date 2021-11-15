using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Api.Dtos
{
    public record RegisterUserDto
    {
        [Required()]
        public string email { get; set; }
        [Required()]
        public string password1 { get; set; }
        [Required()]
        public string password2 { get; set; }
        public string fieldOfStudy { get; set; }
    }
}