using JobOffersApiCore.Common;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface IJobOfferService
    {
        Task<JobOfferDetailResponse> GetJobOfferDetailAsync(Guid jobId);
        Task<PagedList<JobOfferBasicResponse>>GetJobOffersAsync(ResourceParamethers resourceParamethers);
        Task JobOfferExistAsync(Guid id);
        Task DeleteJobOfferFromEventAsync(string message);
    }
}
