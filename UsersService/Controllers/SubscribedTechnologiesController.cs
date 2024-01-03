using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;
using UsersService.Interfaces;

namespace UsersService.Controllers
{
    [Route("api/subscribedtechnologies")]
    [ApiController]
    [Authorize]
    public class SubscribedTechnologiesController : ControllerBase
    {

        private readonly ISubscribedTechnologyService _subscribedTechnologyService;

        public SubscribedTechnologiesController(ISubscribedTechnologyService subscribedTechnologyService)
        {
            _subscribedTechnologyService = subscribedTechnologyService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSubscribedTechnology([FromBody]SubscribedTechnologyRequestDto request)
        {
            await _subscribedTechnologyService.AddSubscribedTechnology(request.TechnologyId);

            return StatusCode(201);
        }
    }
}
