using JobOffersApiCore.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UsersService.Dto;
using UsersService.Interfaces.ServicesInterfaces;

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
        public async Task<IActionResult> AddSubscribedTechnology([FromBody] SubscribedTechnologyRequestDto request)
        {
            await _subscribedTechnologyService.AddSubscribedTechnology(request.TechnologyId);

            return StatusCode(201);
        }

        [HttpDelete("{subscribedTechnologyId}")]
        public async Task<IActionResult> DeleteSubscribedTechnology(Guid subscribedTechnologyId)
        {
            await _subscribedTechnologyService.DeleteSubscribedTechnology(subscribedTechnologyId);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscribedTechnolgoy([FromQuery] ResourceParamethers resourceParamethers)
        {
            var result = await _subscribedTechnologyService.
                GetSubscribedTechnologies(resourceParamethers);

            var paginationMetadata = new PaginationMetadata<TechnologyBasicResponseDto>(result);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(result);
        }
    }
}
