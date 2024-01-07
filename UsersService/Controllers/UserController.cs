using JobOffersApiCore.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Only put request on seniority because user will always have seniority (not nullable field)
        [HttpPut("seniority")]
        public async Task<IActionResult> PutSeniority([FromBody] PutSeniorityRequestDto request)
        {
            await _userService.UpdateUserSeniority(request.Seniority);

            return NoContent();
        }
    }
}
