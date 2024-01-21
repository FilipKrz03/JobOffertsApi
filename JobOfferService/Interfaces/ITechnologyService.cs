using JobOffersApiCore.Common;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface ITechnologyService
    {
        Task<PagedList<TechnologyBasicResponse>>GetTechnologiesAsync(ResourceParamethers resourceParamethers);

        Task<TechnologyDetailResponse> GetTechnologyWithJobOffersAsync(Guid id, ResourceParamethers resourceParamethers);
    }
}
