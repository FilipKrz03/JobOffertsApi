using System.ComponentModel.DataAnnotations;

namespace UsersService.Dto
{
    public record RegisterRequestDto
    {
        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; init; }

        //[Required(ErrorMessage = "User name is required")]
        //[MaxLength(30 , ErrorMessage = "Max legnth of User name is 30 characters")]
        public string UserName { get; init; }

        //[Required(ErrorMessage = "Password is required")]
        //[MinLength(5 , ErrorMessage = "Min length of password is 5 characters")]
        //[MaxLength(30 ,  ErrorMessage = "Max length of password is 30")]
        public string Password { get; init; }

        public RegisterRequestDto(string Email, string UserName, string Password)
        {
            this.Email = Email;
            this.UserName = UserName;
            this.Password = Password;
        }
    }
}
