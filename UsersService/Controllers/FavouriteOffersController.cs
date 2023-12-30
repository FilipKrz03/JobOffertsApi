using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;
using UsersService.Interfaces;

namespace UsersService.Controllers
{
    [Route("api/favouriteoffers")]
    [Authorize]
    [ApiController]
    public class FavouriteOffersController : ControllerBase
    {

        private readonly IClaimService _claimService;
        private readonly IFavouriteOfferService _favouriteOfferService;

        public FavouriteOffersController(IClaimService claimService, IFavouriteOfferService favouriteOfferService)
        {
            _claimService = claimService;
            _favouriteOfferService = favouriteOfferService; 
        }

        [HttpPost]
        public async Task<ActionResult> CreateFavouriteOffer([FromBody] OfferToAddDto request)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            await _favouriteOfferService.CreateFavouriteOffer(userId , request.OfferId);

            return StatusCode(201);
        }

        [HttpDelete("{favouriteOfferId}")]
        public async Task<ActionResult> DeleteFavouriteOffer(Guid favouriteOfferId)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            await _favouriteOfferService.DeleteFavouriteOffer(userId, favouriteOfferId);

            return NoContent();
        }
    }
}
