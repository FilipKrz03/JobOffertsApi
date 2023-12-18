using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using JobOffersService.Entities;
using System.Linq.Expressions;

namespace JobOffersService.Interfaces
{
    public interface ITechnologyRepository : IBaseRepository<Technology>
    {
        Task<IEnumerable<Technology>> GetTechnologies();
        Task<List<Technology>> GetEntitiesFromTechnologiesNames(IEnumerable<string> technologyNames);

        Task<IEnumerable<Technology>> GetTechnologiesAsync
            (ResourceParamethers resourceParamethers, Expression<Func<Technology, object>> keySelector);

        Task<Technology>
            GetTechnologyWithJobOffersAsync(Guid id, ResourceParamethers resourceParamethers);
    }
}
