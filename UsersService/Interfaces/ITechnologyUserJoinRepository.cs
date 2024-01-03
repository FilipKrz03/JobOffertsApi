using UsersService.Entities;

namespace UsersService.Interfaces
{
    public interface ITechnologyUserJoinRepository
    {
        Task<bool> UserTechnologyExistAsync(Guid userId, Guid technologyId);
        Task<TechnologyUser?> GetTechnologyUserJoinEntitiyAsync(Guid userId, Guid technologyId);
        void DeleteTechnologyUserJoinEntityAsync(TechnologyUser entitiy);
        Task SaveChangesAsync();

    }
}
