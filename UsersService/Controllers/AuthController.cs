using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;

namespace UsersService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task RegisterUser([FromBody] RegisterRequestDto request)
        {

        }
    }
}
