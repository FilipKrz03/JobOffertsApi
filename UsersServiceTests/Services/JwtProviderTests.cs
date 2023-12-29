using FluentAssertions;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;
using UsersService.Exceptions;
using UsersService.Services;

namespace UsersServiceTests.Services
{
    public class JwtProviderTests
    {
        [Fact]
        public async Task Provider_GetForCredentialsAsync_Should_ThrowInvalidCredentialsException_WhenResponseIsNotSuceeded()
        {
            string fakePath = "https://crazytoken.com";

            Environment.SetEnvironmentVariable("TokenUri", fakePath);

            var httpMock = new MockHttpMessageHandler();

            httpMock.When(HttpMethod.Post, fakePath)
                .Respond(HttpStatusCode.BadRequest);

            var jwtProvider = new JwtProvider(httpMock.ToHttpClient());

            await jwtProvider.Invoking(m => m.GetForCredentialsAsync(new UsersService.Dto.LoginRequestDto("" , "")))
                .Should().ThrowAsync<InvalidCredentialsException>();    
        }

        [Fact]
        public async Task Provider_GetForCredentialsAsync_Should_ReturnProperTokenResponseDto_WhenRequestSucceed()
        {
            string fakePath = "https://crazytoken.com";

            string tokenValue = "tokenValue";
            string expiresInValue = "expiresInValue";
            string refreshTokenValue = "refreshTokenValue";

            Environment.SetEnvironmentVariable("TokenUri", fakePath);

            var httpMock = new MockHttpMessageHandler();

            httpMock.When(HttpMethod.Post, fakePath)
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    IdToken = tokenValue , 
                    ExpiresIn = expiresInValue , 
                    RefreshToken = refreshTokenValue
                }));

            var jwtProvider = new JwtProvider(httpMock.ToHttpClient());

            var result = await jwtProvider.GetForCredentialsAsync(new UsersService.Dto.LoginRequestDto("", ""));

            result.ExpiresIn.Should().Be(expiresInValue);
            result.Idtoken.Should().Be(tokenValue);
            result.RefreshToken.Should().Be(refreshTokenValue);
        }

        [Fact]
        public async Task Provider_GetForRefreshTokenAsync_Should_ThrowInvalidRefreshTokenException_WhenResponseIsNotSucceded()
        {
            string fakePath = "https://crazytoken.com";

            Environment.SetEnvironmentVariable("RefreshUri", fakePath);

            var httpMock = new MockHttpMessageHandler();

            httpMock.When(HttpMethod.Post, fakePath)
                .Respond(HttpStatusCode.BadRequest);

            var jwtProvider = new JwtProvider(httpMock.ToHttpClient());

            await jwtProvider.Invoking(m => m.GetForRefreshTokenAsync(""))
                .Should()
                .ThrowAsync<InvalidRefreshTokenException>();
        }

        [Fact]
        public async Task Provider_GetForRefreshTokenAsync_Should_ReturnProperTokenResponseDto_WhenRequestSucceed()
        {
            string fakePath = "https://crazytoken.com";

            string tokenValue = "tokenValue";
            string expiresInValue = "expiresInValue";
            string refreshTokenValue = "refreshTokenValue";

            Environment.SetEnvironmentVariable("RefreshUri", fakePath);

            var httpMock = new MockHttpMessageHandler();

            httpMock.When(HttpMethod.Post, fakePath)
                .Respond(HttpStatusCode.OK , JsonContent.Create(new 
                {
                    Id_token = tokenValue,
                    Expires_in = expiresInValue,
                    Refresh_token = refreshTokenValue
                }));

            var jwtProvider = new JwtProvider(httpMock.ToHttpClient());

            var result = await jwtProvider.GetForRefreshTokenAsync("");

            result.ExpiresIn.Should().Be(expiresInValue);
            result.Idtoken.Should().Be(tokenValue);
            result.RefreshToken.Should().Be(refreshTokenValue);
        }
    }
}
