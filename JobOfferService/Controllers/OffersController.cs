using JobOffersApiCore.Common;
using JobOffersApiCore.Helpers;
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

        private readonly IOfferService _jobOffersService;

        public OffersController(IOfferService jobOffersService)
        {
            _jobOffersService = jobOffersService;
        }

        [HttpGet("{jobOfferId}")]
        public async Task<ActionResult<JobOfferDetailResponse>> GetJobOfferDetail(Guid jobOfferId)
        {
            var result = await _jobOffersService.GetJobOfferDetail(jobOfferId);

            return result.ErrorInfo.IsError ? StatusCode(result.ErrorInfo.StatusCode, result.ErrorInfo.ErrorMessage) :
                Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobOfferBasicResponse>>>
            GetJobOffers([FromQuery] ResourceParamethers resourceParamethers)
        {
            var result = await _jobOffersService.GetJobOffers(resourceParamethers);

            return Ok(result.Value);
        }
    }
}
