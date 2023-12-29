using JobOffersApiCore.Common;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface IJobOfferService
    {
        Task<JobOfferDetailResponse> GetJobOfferDetail(Guid jobId);
        Task<IEnumerable<JobOfferBasicResponse>>GetJobOffers(ResourceParamethers resourceParamethers);
        Task JobOfferExist(Guid id);
    }
}
