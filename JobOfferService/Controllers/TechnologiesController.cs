using JobOffersApiCore.Common;
using JobOffersService.Dto;
using JobOffersService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnologyBasicResponse>>>
            GetTechnologies([FromQuery] ResourceParamethers resourceParamethers)
        {
            var result = await _technologyService.GetTechnologies(resourceParamethers);

            return Ok(result.Value);
        }
    }
}
