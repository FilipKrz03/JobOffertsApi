using JobOffersApiCore.Interfaces;
using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface IFavouriteOfferRepositroy : IBaseRepository<FavouriteOffer>
    {
        Task<FavouriteOffer?> GetUserFavouriteOffer(Guid userId, Guid offerId);
    }
}
