using JobOffersService.Filters;
using JobOffersService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobOffersService.Controllers
{
    // Controller not for users , only for communication with user microservice !
    // Users microservice has API key
    [ApiKeyAuthorizationFilterAtributte]
    [Route("api/proxy")]
    [ApiController]
    public class MicroserviceProxyController : ControllerBase
    {

        private readonly IJobOfferService _jobOfferService;

        public MicroserviceProxyController(IJobOfferService jobOfferService)
        {
            _jobOfferService = jobOfferService;
        }

        [HttpGet("offers/offerexist/{offerId}")]
        public async Task<ActionResult> JobOfferExistInfo(Guid offerId)
        {
            await _jobOfferService.JobOfferExist(offerId);
            // If the offer not found, an exception will be thrown, and a 404 status code will be returned

            return NoContent();
        }
    }
}
