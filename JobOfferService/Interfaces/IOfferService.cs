using JobOffersApiCore.Common;
using JobOffersApiCore.Helpers;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface IOfferService
    {
        Task<Response<JobOfferDetailResponse>> GetJobOfferDetail(Guid jobId);

        Task<Response<IEnumerable<JobOfferBasicResponse>>>GetJobOffers(ResourceParamethers resourceParamethers);
    }
}
