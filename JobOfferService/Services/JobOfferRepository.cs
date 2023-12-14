using JobOffersApiCore.BaseObjects;
using JobOffersService.DbContexts;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobOffersService.Services
{
    public class JobOfferRepository : BaseRepository<JobOffersContext , JobOffer> , IJobOfferRepository
    {
        public JobOfferRepository(JobOffersContext context) : base(context) { }

        public async Task<bool> IsDatabaseInitalized()
        {
            return await Query().AnyAsync();
        }
    }
}
