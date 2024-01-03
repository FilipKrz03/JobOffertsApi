using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Interfaces;

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
                .AnyAsync(e => e.TechnologyId == technologyId && e.UserId == userId);
        }
    }
}
