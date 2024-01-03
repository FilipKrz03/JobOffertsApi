using JobOffersApiCore.Interfaces;
using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface IJobOfferRepository : IBaseRepository<JobOffer>
    {
        Task<JobOffer?> GetUserJobOffer(Guid userId, Guid jobOfferId);
    }
}
