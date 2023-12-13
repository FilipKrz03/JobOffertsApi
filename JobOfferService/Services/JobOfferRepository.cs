using JobOffersApiCore.BaseObjects;
using JobOffersService.DbContexts;
using JobOffersService.Entities;
using JobOffersService.Interfaces;

namespace JobOffersService.Services
{
    public class JobOfferRepository : BaseRepository<JobOffersContext , JobOffer> , IJobOfferRepository
    {
        public JobOfferRepository(JobOffersContext context) : base(context) { }

    }
}
