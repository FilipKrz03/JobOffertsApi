using System.Text.Json.Serialization;

namespace UsersService.Dto
{
    public record TokenResponseDto(string Idtoken , string ExpiresIn ,  string RefreshToken) { }
}
