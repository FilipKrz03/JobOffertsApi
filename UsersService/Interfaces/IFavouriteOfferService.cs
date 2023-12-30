using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IFavouriteOfferService
    {
        Task CreateFavouriteOffer(Guid userId, Guid offerId);
        Task DeleteFavouriteOffer(Guid userId, Guid favouriteOfferId);
    }
}
