namespace UsersService.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(string email , string password , string userName);
    }
}
