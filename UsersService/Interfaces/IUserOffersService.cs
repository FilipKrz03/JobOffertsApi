using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IUserOffersService
    {
        Task CreateUserFavouriteOffer(string userId, Guid offerId);
    }
}
