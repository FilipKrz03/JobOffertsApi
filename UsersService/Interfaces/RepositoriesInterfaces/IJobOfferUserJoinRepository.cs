using UsersService.Entities;

namespace UsersService.Interfaces.RepositoriesInterfaces
{
    public interface IJobOfferUserJoinRepository
    {
        Task<bool> UserJobOfferExistAsync(Guid userId, Guid jobId);
        Task<JobOfferUser?> GetUserJobOfferJoinAsync(Guid userId, Guid jobId);
        void RemoveUserJobOffer(JobOfferUser entity);
        Task SaveChangesAsync();
    }
}
