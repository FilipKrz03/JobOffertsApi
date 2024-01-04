using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using System.Linq.Expressions;
using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface ITechnologyRepository : IBaseRepository<Technology>
    {
        Task<IEnumerable<Technology>> GetUserTechnologiesAsync
           (ResourceParamethers resourceParamethers, Expression<Func<Technology, object>> keySelector, Guid userId);
    }
}
