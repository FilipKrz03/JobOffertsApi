using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(RegisterRequestDto request);
        Task<TokenResponseDto> LoginUser(LoginRequestDto request);
        Task<TokenResponseDto> RefreshUserAccessToken(string refreshToken);
    }
}
