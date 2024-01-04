using UsersService.Dto;

namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(RegisterRequestDto request);
    }
}
