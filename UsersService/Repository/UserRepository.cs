using JobOffersApiCore.BaseObjects;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class UserRepository : BaseRepository<UsersDbContext , User> ,  IUserRepository
    {
        public UserRepository(UsersDbContext context):base(context) { }

        public async Task<bool> UserFavouriteOfferExist(Guid offerId, string userIdentity)
        {
            return await Query()
                .Where(u => u.IdentityId == userIdentity && u.FavouriteOffers.Any(e => e.OfferId == offerId))
                .AnyAsync();
        }

        public async Task<User?> GetByIdentityId(string identityId)
        {
            return await Query()
                .Where(x => x.IdentityId == identityId)
                .FirstOrDefaultAsync();
        }
    }
}
