using JobOffersApiCore.BaseObjects;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class JobOfferRepository : BaseRepository<UsersDbContext, JobOffer>, IJobOfferRepository
    {
        public JobOfferRepository(UsersDbContext context) : base(context) { }

        public async Task<JobOffer?> GetUserJobOffer(Guid userId, Guid jobOfferId)
        {
            return await Query()
                .Where(x => x.Id == jobOfferId && x.Users.Any(x => x.Id == userId))
                .Include(x => x.Technologies)
                .FirstOrDefaultAsync();
        }
    }
}
