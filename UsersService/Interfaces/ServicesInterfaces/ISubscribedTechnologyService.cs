using JobOffersApiCore.Common;
using UsersService.Dto;

namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface ISubscribedTechnologyService
    {
        Task AddSubscribedTechnologyAsync(Guid technologyId);
        Task DeleteSubscribedTechnologyAsync(Guid subscribedTechnologyId);
        Task<PagedList<TechnologyBasicResponseDto>>
            GetSubscribedTechnologiesAsync(ResourceParamethers resourceParamethers);
    }
}
