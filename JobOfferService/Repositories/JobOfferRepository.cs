using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Common;
using JobOffersService.DbContexts;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobOffersService.Repositories
{
    public class JobOfferRepository : BaseRepository<JobOffersContext, JobOffer>, IJobOfferRepository
    {
        public JobOfferRepository(JobOffersContext context) : base(context) { }

        public async Task<bool> IsDatabaseInitalized()
        {
            return await Query().AnyAsync();
        }

        public async Task<IEnumerable<JobOffer>> GetJobOffersAsync(ResourceParamethers resourceParamethers)
        {
            return await PagedList<JobOffer>
                .CreateAsync(Query(), resourceParamethers.PageSize, resourceParamethers.PageNumber);
        }
    }
}
