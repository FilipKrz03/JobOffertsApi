using JobOffersApiCore.Common;
using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface ISubscribedTechnologyService
    {
        Task AddSubscribedTechnology(Guid technologyId);
        Task DeleteSubscribedTechnology(Guid subscribedTechnologyId);
        Task<PagedList<TechnologyBasicResponseDto>>
            GetSubscribedTechnologies(ResourceParamethers resourceParamethers);
    }
}
