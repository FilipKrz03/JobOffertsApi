using JobOffersApiCore.Common;
using JobOffersService.Dto;
using JobOffersService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            var result = await _jobOffersService.GetJobOfferDetailAsync(jobOfferId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobOfferBasicResponse>>>
            GetJobOffers([FromQuery] ResourceParamethers resourceParamethers)
        {
            var result = await _jobOffersService.GetJobOffersAsync(resourceParamethers);

            var paginationMetadata = new PaginationMetadata<JobOfferBasicResponse>(result);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(result);
        }
    }
}
