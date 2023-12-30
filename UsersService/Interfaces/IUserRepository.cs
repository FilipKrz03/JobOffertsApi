using JobOffersApiCore.Interfaces;
using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> UserFavouriteOfferExist(Guid offerId, Guid userId);
    }
}
