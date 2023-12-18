using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using JobOffersService.Entities;
using System.Linq.Expressions;

namespace JobOffersService.Interfaces
{
    public interface ITechnologyRepository : IBaseRepository<Technology>
    {
        Task<IEnumerable<Technology>> GetAllTechnologiesAsync();
        Task<List<Technology>> GetEntitiesFromTechnologiesNamesAsync(IEnumerable<string> technologyNames);

        Task<IEnumerable<Technology>> GetTechnologiesAsync
            (ResourceParamethers resourceParamethers, Expression<Func<Technology, object>> keySelector);

        Task<Technology>
            GetTechnologyWithJobOffersAsync(Guid id, ResourceParamethers resourceParamethers);
    }
}
