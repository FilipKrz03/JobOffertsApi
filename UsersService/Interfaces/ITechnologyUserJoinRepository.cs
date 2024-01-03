namespace UsersService.Interfaces
{
    public interface ITechnologyUserJoinRepository
    {
        Task<bool> UserTechnologyExistAsync(Guid userId, Guid technologyId);
    }
}
