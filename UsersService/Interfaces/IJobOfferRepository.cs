using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using System.Linq.Expressions;
using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface IJobOfferRepository : IBaseRepository<JobOffer>
    {
        Task<JobOffer?> GetUserJobOffer(Guid userId, Guid jobOfferId);
        Task<PagedList<JobOffer>> GetUserJobOffersAsync
           (Expression<Func<JobOffer, object>> keySelector, ResourceParamethers resourceParamethers, Guid userId);
    }
}
