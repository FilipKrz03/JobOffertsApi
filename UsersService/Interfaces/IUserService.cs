using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(RegisterRequestDto request);
    }
}
