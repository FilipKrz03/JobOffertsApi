namespace UsersService.Interfaces
{
    public interface IJobOfferUserJoinRepository
    {
        Task<bool> UserJobOfferExist(Guid userId, Guid jobId);
    }
}
