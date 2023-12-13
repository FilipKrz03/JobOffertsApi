using JobOffersApiCore.Interfaces;
using JobOffersService.Entities;

namespace JobOffersService.Interfaces
{
    public interface ITechnologyRepository : IBaseRepository<Technology>
    {
        Task<IEnumerable<Technology>> GetTechnologies();
        Task<List<Technology>> GetEntitiesFromTechnologiesNames(IEnumerable<string> technologyNames);
    }
}
