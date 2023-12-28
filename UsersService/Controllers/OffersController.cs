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

        public OffersController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpPost("favouriteoffers")]
        public async Task CreateFavouriteOffer([FromBody] OfferToAddDto request)
        {
            var userId = _claimService.GetUserIdentityIdFromTokenClaim();



        }

    }
}
