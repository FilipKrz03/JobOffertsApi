using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;
using UsersService.Interfaces;

namespace UsersService.Controllers
{
    [Route("api/followedjoboffers")]
    [ApiController]
    [Authorize]
    public class FollowedJobOfferController : ControllerBase
    {

        private readonly IFollowedJobOfferService _followedJobOfferService;

        public FollowedJobOfferController(IFollowedJobOfferService followedJobOfferService)
        {
            _followedJobOfferService = followedJobOfferService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFolowedJobOffer([FromBody] OfferToAddDto request)
        {
            await _followedJobOfferService.AddFolowedJobOffer(request.OfferId);

            return StatusCode(201);
        }

        [HttpDelete("{followedJobOfferId}")]
        public async Task<IActionResult> DeleteFollowedJobOffer([FromQuery] Guid followedJobOfferId)
        {


            return NoContent();
        }

    }
}
