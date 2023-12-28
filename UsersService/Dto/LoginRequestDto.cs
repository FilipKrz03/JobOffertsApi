using System.ComponentModel.DataAnnotations;

namespace UsersService.Dto
{
    public record LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Min length of password is 5 characters")]
        [MaxLength(30, ErrorMessage = "Max length of password is 30")]
        public string Password { get; init; }

        public LoginRequestDto(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}
