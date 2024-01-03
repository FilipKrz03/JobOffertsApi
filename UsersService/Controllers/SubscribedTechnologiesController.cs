using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;

namespace UsersService.Controllers
{
    [Route("api/subscribedtechnologies")]
    [ApiController]
    [Authorize]
    public class SubscribedTechnologiesController : ControllerBase
    {
        [HttpPost]
        public async Task AddSubscribedTechnology([FromBody]SubscribedTechnologyRequestDto request)
        {

        }
    }
}
