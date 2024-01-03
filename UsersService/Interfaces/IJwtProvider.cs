using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IJwtProvider
    {
        Task<TokenResponseDto> GetForCredentialsAsync(LoginRequestDto request);
        Task<TokenResponseDto> GetForRefreshTokenAsync(string refreshToken);    
    }
}
