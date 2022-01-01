using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Server.Api.Models;

namespace Server.Api.Dtos
{
    public record LoginUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public static LoginUserDto Convert(User user)
        {
            throw new System.Exception("method not implemented");
        }
    }

    [ExcludeFromCodeCoverage]
    public record RegisterUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public int fieldOfStudy { get; set; }

        public static RegisterUserDto Convert(User user)
        {
            throw new System.Exception("method not implemented");
        }
    }

    public record ResponseUserDto
    {
        public string email { get; set; }

        public static ResponseUserDto Convert(User user)
        {
            return new ResponseUserDto()
            {
                email = user.Email
            };
        }
    }
}
