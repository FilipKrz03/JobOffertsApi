using JobOffersApiCore.Interfaces;
using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> UserFavouriteOfferExist(Guid offerId, string userIdentity);

        Task<User?> GetByIdentityId(string identityId);
    }
}
