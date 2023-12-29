using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(RegisterRequestDto request);
    }
}
