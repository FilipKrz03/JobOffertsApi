using System.ComponentModel.DataAnnotations;

namespace UsersService.Dto
{
    public class RegisterRequestDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [MaxLength(30)]
        public string UserName { get; init; }

        [Required]
        [MinLength(5)]
        public string Password { get; init; }

        public RegisterRequestDto(string email, string userName, string password)
        {
            Email = email;
            UserName = userName;
            Password = password;
        }
    }
}
