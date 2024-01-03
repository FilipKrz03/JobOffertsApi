using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface IJobOfferUserJoinRepository
    {
        Task<bool> UserJobOfferExist(Guid userId, Guid jobId);
        Task<JobOfferUser?> GetUserJobOffer(Guid userId, Guid jobId);
        void RemoveUserJobOffer(JobOfferUser entity);
        Task SaveChangesAsync();
    }
}
