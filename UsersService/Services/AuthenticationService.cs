﻿using FirebaseAdmin.Auth;
using UsersService.Interfaces;

namespace UsersService.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        public async Task<string> RegisterAsync(string email, string password)
        {
            var userArgs = new UserRecordArgs()
            {
                Email = email,
                Password = password,
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            return userRecord.Uid;
        }
    }
}