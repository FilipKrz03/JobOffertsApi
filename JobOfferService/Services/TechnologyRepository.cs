using JobOffersApiCore.BaseObjects;
using JobOffersService.DbContexts;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobOffersService.Services
{
    public class TechnologyRepository : BaseRepository<JobOffersContext , Technology> , ITechnologyRepository
    {
        public TechnologyRepository(JobOffersContext context) : base(context) { }

        public async Task<IEnumerable<Technology>> GetTechnologies()
        {
            return await Query().ToListAsync();
        }
    }
}
