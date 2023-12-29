using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UsersService.Dto;
using UsersService.Interfaces;

namespace UsersService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtProvider _jwtProvider;

        public AuthController(IAuthenticationService authenticationService , IJwtProvider jwtProvider)
        {
           _authenticationService = authenticationService;
           _jwtProvider = jwtProvider;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterRequestDto request)
        {
            await   _authenticationService.RegisterAsync(request);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginRequestDto request)
        {
            var tokens = await _jwtProvider.GetForCredentialsAsync(request);

            SetupRefreshTokenCookie(Response, tokens.RefreshToken);

            var responseObject = new
            {
                accessToken = tokens.Idtoken,
                expiresIn = tokens.ExpiresIn
            };

            return Ok(responseObject);
        }

        [HttpGet("refresh")]
        public async Task<ActionResult> RefreshAccessToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (refreshToken == null)
            {
                return BadRequest("You do not have refresh token in your cookie ! , " +
                    "To use refresh route you need to signIn first");
            }

            var tokens = await _jwtProvider.GetForRefreshTokenAsync(refreshToken);

            SetupRefreshTokenCookie(Response, refreshToken);

            var responseObject = new
            {
                accessToken = tokens.Idtoken,
                expiresIn = tokens.ExpiresIn
            };

            return Ok(responseObject);
        }

        private void SetupRefreshTokenCookie(HttpResponse resposne, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(24),
                HttpOnly = true,
                Secure = true,
            };

            resposne.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        }
    }
}
