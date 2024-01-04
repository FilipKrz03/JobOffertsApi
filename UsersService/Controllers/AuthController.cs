using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UsersService.Dto;
using UsersService.Interfaces.ServicesInterfaces;

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
            var tokensInfo = await _jwtProvider.GetForCredentialsAsync(request);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(24),
                HttpOnly = true,
                Secure = true,
            };

            Response.Cookies.Append("RefreshToken", tokensInfo.RefreshToken, cookieOptions);

            var responseObject = new
            {
                accessToken = tokensInfo.Idtoken,
                expiresIn = tokensInfo.ExpiresIn
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

            var responseObject = new
            {
                accessToken = tokens.Idtoken,
                expiresIn = tokens.ExpiresIn
            };

            return Ok(responseObject);
        }
    }
}
