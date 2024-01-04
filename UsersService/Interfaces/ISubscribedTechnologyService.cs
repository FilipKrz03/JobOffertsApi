namespace UsersService.Interfaces
{
    public interface ISubscribedTechnologyService
    {
        Task AddSubscribedTechnology(Guid technologyId);
        Task DeleteSubscribedTechnology(Guid subscribedTechnologyId);
    }
}
