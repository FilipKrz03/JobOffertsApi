using JobOffersApiCore.BaseObjects;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class FavouriteOfferRepository : BaseRepository<UsersDbContext , FavouriteOffer> , IFavouriteOfferRepositroy
    {
        public FavouriteOfferRepository(UsersDbContext context) : base(context) { }
    }
}
