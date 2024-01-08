using JobOffersApiCore.Interfaces;
using System.Threading.Tasks;
using UsersService.Dto;
using UsersService.Entities;

namespace UsersService.Interfaces.RepositoriesInterfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<UserWithEmailSeniorityAndTechnolgiesDto>> GetAllUsersWithEmailSeniorityAndTechnologiesAsync();
    }
}
