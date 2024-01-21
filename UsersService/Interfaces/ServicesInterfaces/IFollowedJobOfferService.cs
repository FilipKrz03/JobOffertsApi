using JobOffersApiCore.Common;
using UsersService.Dto;

namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface IFollowedJobOfferService
    {
        Task AddFolowedJobOfferAsync(Guid offerId);
        Task DeleteFollowedJobOfferAsync(Guid followedJobOfferId);
        Task<JobOfferDetailResponseDto?> GetFollowedJobOfferAsync(Guid followedJobOfferId);
        Task<PagedList<JobOfferBasicResponseDto>>
            GetFollowedJobOffersAsync(ResourceParamethers resourceParamethers);
    }
}
