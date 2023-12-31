using JobOffersApiCore.BaseObjects;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class FavouriteOfferRepository : BaseRepository<UsersDbContext, FavouriteOffer>, IFavouriteOfferRepositroy
    {
        public FavouriteOfferRepository(UsersDbContext context) : base(context) { }

        public async Task<FavouriteOffer?> GetUserFavouriteOffer(Guid userId, Guid id)
        {
            return await Query()
                .Where(o => o.Id == id && o.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserFavouriteOfferExist(Guid offerId, Guid userId)
        {
            return await Query()
            .Where(u => u.UserId == userId && u.OfferId == offerId)
            .AnyAsync();
        }
    }
}
