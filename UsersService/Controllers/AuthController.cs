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
    }
}
