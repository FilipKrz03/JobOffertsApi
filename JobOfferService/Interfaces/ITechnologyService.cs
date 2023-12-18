using JobOffersApiCore.Common;
using JobOffersApiCore.Helpers;
using JobOffersService.Dto;

namespace JobOffersService.Interfaces
{
    public interface ITechnologyService
    {
        Task<Response<IEnumerable<TechnologyBasicResponse>>>
            GetTechnologies(ResourceParamethers resourceParamethers);
    }
}
