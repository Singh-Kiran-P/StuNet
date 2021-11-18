using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Api.Dtos
{
    public record LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}