using JobOffersApiCore.BaseObjects;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class JobOfferRepository : BaseRepository<UsersDbContext , JobOffer>, IJobOfferRepository
    {
        public JobOfferRepository(UsersDbContext context) : base(context) { }
    }
}
