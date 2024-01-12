using JobOffersApiCore.Common;
using JobOffersService.Dto;
using JobOffersService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JobOffersService.Controllers
{
    [Route("api/technologies")]
    [ApiController]
    public class TechnologiesController : ControllerBase
    {

        private readonly ITechnologyService _technologyService;

        public TechnologiesController(ITechnologyService technologyService)
        {
            _technologyService = technologyService;
        }

        [HttpGet("{technologyId}")]
        public async Task<ActionResult<TechnologyDetailResponse>>GetTechnologyWithJobOffersList
            (Guid technologyId , [FromQuery]ResourceParamethers resourceParamethers)
        {
            var result = await _technologyService.GetTechnologyWithJobOffers(technologyId, resourceParamethers);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnologyBasicResponse>>>
            GetTechnologies([FromQuery] ResourceParamethers resourceParamethers)
        {
            var result = await _technologyService.GetTechnologies(resourceParamethers);

            var paginationMetadata = new PaginationMetadata<TechnologyBasicResponse>(result);

            Response.Headers.Add("X-Pagination" , JsonSerializer.Serialize(paginationMetadata));

            return Ok(result);
        }
    }
}
