using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;
using UsersService.Interfaces;

namespace UsersService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
            
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;    
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterRequestDto request)
        {
             await _userService.RegisterUser(request);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody]LoginRequestDto request)
        {
            var tokens = await _userService.LoginUser(request);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(24),
                HttpOnly = true,
                Secure = true,
            };

            Response.Cookies.Append("RefreshToken", tokens.RefreshToken , cookieOptions);

            var responseObject = new
            {
                accessToken = tokens.Idtoken,
                expiresIn = tokens.ExpiresIn
            };

            return Ok(responseObject);
        }
    }
}
