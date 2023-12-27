
using JobOffersApiCore.Helpers;
using Microsoft.AspNetCore.Authentication;
using UsersService.Dto;
using UsersService.Interfaces;
using UsersService.Entities;

namespace UsersService.Services
{
  
    public class UsersService 
    {

        private readonly Interfaces.IAuthenticationService _authenticationService;

        public UsersService(Interfaces.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task RegisterUser(RegisterRequestDto request)
        {
            var identityId = _authenticationService.RegisterAsync(request.Email, request.Password);
        }
    }
}
