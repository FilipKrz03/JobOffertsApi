using UsersService.Dto;

namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface IJwtProvider
    {
        Task<TokenResponseDto> GetForCredentialsAsync(LoginRequestDto request);
        Task<TokenResponseDto> GetForRefreshTokenAsync(string refreshToken);
    }
}
