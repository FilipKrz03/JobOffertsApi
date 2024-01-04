using FirebaseAdmin;
using FirebaseAdmin.Auth;
using UsersService.Dto;
using UsersService.Exceptions;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Services
{
    public class JwtProvider : IJwtProvider
    {

        private readonly HttpClient _httpClient;

        public JwtProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponseDto> GetForCredentialsAsync(LoginRequestDto loginRequest)
        {
            var request = new
            {
                email = loginRequest.Email,
                password = loginRequest.Password,
                returnSecureToken = true
            };

            string path = Environment.GetEnvironmentVariable("TokenUri")!;

            var response = await _httpClient.PostAsJsonAsync(path, request);

            if(!response.IsSuccessStatusCode)
            {
                throw new InvalidCredentialsException("Invalid credentials");
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

            return tokenResponse!;
        }

        public async Task<TokenResponseDto> GetForRefreshTokenAsync(string refreshToken)
        {
            var request = new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken
            };

            string path = Environment.GetEnvironmentVariable("RefreshUri")!;

            var response = await _httpClient.PostAsJsonAsync(path, request);

            if(!response.IsSuccessStatusCode)
            {
                throw new InvalidRefreshTokenException("Invalid refresh token provided");
            }

            // Token response from refresh action in firebase has diffrent format than from signIn action
            var tokenResponse = await response.Content
                .ReadFromJsonAsync<TokenResponseFromRefreshFirebaseActionDto>();

            return new TokenResponseDto
                (tokenResponse!.Id_token, tokenResponse.Expires_in, tokenResponse.Refresh_token);
        }
    }
}
