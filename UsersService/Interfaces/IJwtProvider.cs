using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IJwtProvider
    {
        Task<TokenResponseDto> GetForCredentialsAsync(string email, string password);
    }
}
