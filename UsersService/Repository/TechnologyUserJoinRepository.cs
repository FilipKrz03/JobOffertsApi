using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;

namespace UsersService.Repository
{
    public class TechnologyUserJoinRepository : ITechnologyUserJoinRepository
    {

        private readonly UsersDbContext _context;

        public TechnologyUserJoinRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserTechnologyExistAsync(Guid userId , Guid technologyId)
        {
            return await _context.TechnologyUsers
                .AnyAsync(x => x.TechnologyId == technologyId && x.UserId == userId);
        }

        public async Task<TechnologyUser?>GetTechnologyUserJoinEntitiyAsync(Guid userId , Guid technologyId)
        {
            return await _context.TechnologyUsers
               .Where(x => x.TechnologyId == technologyId && x.UserId == userId)
               .FirstOrDefaultAsync();
        }

        public void DeleteTechnologyUserJoinEntity(TechnologyUser entitiy)
        {
            _context.TechnologyUsers.Remove(entitiy);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
