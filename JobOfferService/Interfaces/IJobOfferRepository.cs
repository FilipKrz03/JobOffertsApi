using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using JobOffersService.Entities;

namespace JobOffersService.Interfaces
{
    public interface IJobOfferRepository : IBaseRepository<JobOffer>
    {
        Task<bool> IsDatabaseInitalized();
        Task<IEnumerable<JobOffer>> GetJobOffersAsync(ResourceParamethers resourceParamethers);
    }
}
