using UsersService.Dto;

namespace UsersService.Interfaces
{
    public interface IUserOffersService
    {
        Task CreateUserFavouriteOffer(Guid userId, Guid offerId);
    }
}
