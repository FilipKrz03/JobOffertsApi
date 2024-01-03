using JobOffersApiCore.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq.Expressions;
using UsersService.Dto;
using UsersService.Entities;
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
        public async Task<IActionResult> DeleteFollowedJobOffer(Guid followedJobOfferId)
        {
            await _followedJobOfferService.DeleteFollowedJobOffer(followedJobOfferId);

            return NoContent();
        }

        [HttpGet("{followedJobOfferId}")]
        public async Task<IActionResult> GetFollowedJobOffer(Guid followedJobOfferId)
        {
            var jobOffer = await _followedJobOfferService.GetFollowedJobOffer(followedJobOfferId);

            return Ok(jobOffer);
        }

        [HttpGet]
        public async Task<IActionResult> GetFollowedJobOffers([FromQuery] ResourceParamethers resourceParamethers)
        {
            var result = await _followedJobOfferService.GetFollowedJobOffers(resourceParamethers);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount , 
                pageSize = result.PageSize , 
                hasPrevious = result.HasPrevious ,
                hasNext = result.HasNext ,
                totalPages = result.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            
            return Ok(result);
        }

    }
}
