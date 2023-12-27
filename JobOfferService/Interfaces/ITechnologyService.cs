using JobOffersApiCore.Common;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface ITechnologyService
    {
        Task<IEnumerable<TechnologyBasicResponse>>GetTechnologies(ResourceParamethers resourceParamethers);

        Task<TechnologyDetailResponse> GetTechnologyWithJobOffers (Guid id, ResourceParamethers resourceParamethers);
    }
}
