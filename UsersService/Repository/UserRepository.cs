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

        public async Task<bool> UserFavouriteOfferExist(Guid offerId, Guid userId)
        {
            return await Query()
                .Where(u => u.Id == userId && u.FavouriteOffers.Any(e => e.OfferId == offerId))
                .AnyAsync();
        }
    }
}
