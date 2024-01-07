using JobOffersApiCore.BaseObjects;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;

namespace UsersService.Repository
{
    public class UserRepository : BaseRepository<UsersDbContext , User> ,  IUserRepository
    {
        public UserRepository(UsersDbContext context):base(context) { }

        public async Task<IEnumerable<User>> GetAllUsersWithTechnologiesAsync()
        {
            // Only for analyze service
            return await Query()
                .Include(x => x.Technologies)
                .ToListAsync();
        }
    }
}
