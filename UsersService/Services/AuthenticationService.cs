using FirebaseAdmin.Auth;
using UsersService.Interfaces;

namespace UsersService.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        public async Task<string> RegisterAsync(string email, string password , string userName)
        {
            var userArgs = new UserRecordArgs()
            {
                Email = email,
                Password = password,
                DisplayName = userName , 
                EmailVerified = true
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            return userRecord.Uid;
        }
    }
}
