using JobOffersApiCore.Common;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface IJobOfferService
    {
        Task<JobOfferDetailResponse> GetJobOfferDetail(Guid jobId);
        Task<PagedList<JobOfferBasicResponse>>GetJobOffers(ResourceParamethers resourceParamethers);
        Task JobOfferExist(Guid id);
        Task DeleteJobOfferFromEvent(string message);
    }
}
