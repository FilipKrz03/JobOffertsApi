using JobOffersApiCore.Common;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface ITechnologyService
    {
        Task<PagedList<TechnologyBasicResponse>>GetTechnologies(ResourceParamethers resourceParamethers);

        Task<TechnologyDetailResponse> GetTechnologyWithJobOffers (Guid id, ResourceParamethers resourceParamethers);
    }
}
