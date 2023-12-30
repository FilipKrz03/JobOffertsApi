using JobOffersApiCore.BaseObjects;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class FavouriteOfferRepository : BaseRepository<UsersDbContext , FavouriteOffer> , IFavouriteOfferRepositroy
    {
        public FavouriteOfferRepository(UsersDbContext context) : base(context) { }


        public async Task<FavouriteOffer?> GetUserFavouriteOffer(Guid userId , Guid offerId)
        {
            return await Query()
                .Where(o => o.Id == offerId && o.UserId == userId)
                .FirstOrDefaultAsync();
        }

    }
}
