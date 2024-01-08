using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Enum;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Dto;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;

namespace UsersService.Repository
{
    public class UserRepository : BaseRepository<UsersDbContext , User> ,  IUserRepository
    {
        public UserRepository(UsersDbContext context):base(context) { }

        public async Task<IEnumerable<UserWithEmailSeniorityAndTechnolgiesDto>>
            GetAllUsersWithEmailSeniorityAndTechnologiesAsync()
        {
            // Only for analyze service
            return await Query()
                .Select(x => new UserWithEmailSeniorityAndTechnolgiesDto(x.Email , x.DesiredSeniority , x.Technologies))      
                .ToListAsync();
        }
    }
}
