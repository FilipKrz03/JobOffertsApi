using JobOffersApiCore.Interfaces;
using System.Threading.Tasks;
using UsersService.Entities;

namespace UsersService.Interfaces.RepositoriesInterfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersWithTechnologiesAsync();
    }
}
