using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface IJobOfferUserJoinRepository
    {
        Task<bool> UserJobOfferExistAsync(Guid userId, Guid jobId);
        Task<JobOfferUser?> GetUserJobOfferAsync(Guid userId, Guid jobId);
        void RemoveUserJobOffer(JobOfferUser entity);
        Task SaveChangesAsync();
    }
}
