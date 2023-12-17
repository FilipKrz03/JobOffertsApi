using JobOffersApiCore.Helpers;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface IJobOfferService
    {
        Task<Response<JobOfferDetailResponse>> GetJobOfferDetail(Guid jobId);
    }
}
