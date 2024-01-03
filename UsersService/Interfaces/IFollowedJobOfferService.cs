using JobOffersApiCore.Common;
using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IFollowedJobOfferService
    {
        Task AddFolowedJobOffer(Guid offerId);
        Task DeleteFollowedJobOffer(Guid followedJobOfferId);
        Task<JobOfferDetailResponseDto?> GetFollowedJobOffer(Guid followedJobOfferId);
        Task<PagedList<JobOfferBasicResponseDto>>
            GetFollowedJobOffers(ResourceParamethers resourceParamethers);
    }
}
