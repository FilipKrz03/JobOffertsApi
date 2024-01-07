using JobOffersApiCore.Enum;

namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface IUserService
    {
        Task UpdateUserSeniority(Seniority seniority);
    }
}
