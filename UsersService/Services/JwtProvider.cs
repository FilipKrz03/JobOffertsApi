using FirebaseAdmin;
using FirebaseAdmin.Auth;
using UsersService.Dto;
using UsersService.Exceptions;
using UsersService.Interfaces;

namespace UsersService.Services
{
    public class JwtProvider : IJwtProvider
    {

        private readonly HttpClient _httpClient;

        public JwtProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponseDto> GetForCredentialsAsync(string email, string password)
        {
            var request = new
            {
                email,
                password,
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
    }
}
