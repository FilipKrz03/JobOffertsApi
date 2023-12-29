using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;
using UsersService.Interfaces;

namespace UsersService.Controllers
{
    [Route("api/offers")]
    [Authorize]
    [ApiController]
    public class OffersController : ControllerBase
    {

        private readonly IClaimService _claimService;
        private readonly IUserOffersService _userOffersService;


        public OffersController(IClaimService claimService, IUserOffersService userOffersService)
        {
            _claimService = claimService;
            _userOffersService = userOffersService; 

        }

        [HttpPost("favouriteoffers")]
        public async Task<ActionResult> CreateUserFavouriteOffer([FromBody] OfferToAddDto request)
        {
            var userId = _claimService.GetUserIdentityIdFromTokenClaim();

            await _userOffersService.CreateUserFavouriteOffer(userId , request.OfferId);

            return Ok();
        }

    }
}
