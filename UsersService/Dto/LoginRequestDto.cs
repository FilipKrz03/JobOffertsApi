using System.ComponentModel.DataAnnotations;

namespace UsersService.Dto
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [MinLength(5)]
        public string Password { get; init; }

        public LoginRequestDto(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
