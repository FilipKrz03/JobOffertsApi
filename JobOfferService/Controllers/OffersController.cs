using JobOffersApiCore.Common;
using JobOffersService.Dto;
using JobOffersService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobOffersService.Controllers
{
    [Route("api/offers")]
    [ApiController]
    public class OffersController : ControllerBase
    {

        private readonly IJobOfferService _jobOffersService;

        public OffersController(IJobOfferService jobOffersService)
        {
            _jobOffersService = jobOffersService;
        }

        [HttpGet("{jobOfferId}")]
        public async Task<ActionResult<JobOfferDetailResponse>> GetJobOfferDetail(Guid jobOfferId)
        {
            var result = await _jobOffersService.GetJobOfferDetail(jobOfferId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobOfferBasicResponse>>>
            GetJobOffers([FromQuery] ResourceParamethers resourceParamethers)
        {
            var result = await _jobOffersService.GetJobOffers(resourceParamethers);

            return Ok(result);
        }
    }
}
