
using Microsoft.AspNetCore.Authentication;
using UsersService.Dto;
using UsersService.Interfaces;
using UsersService.Entities;
using AutoMapper;

namespace UsersService.Services
{

    public class UsersService : IUserService
    {

        private readonly Interfaces.IAuthenticationService _authenticationService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersService(Interfaces.IAuthenticationService authenticationService, IUserRepository userRepository,
            IMapper mapper, IJwtProvider jwtProvider)
        {
            _authenticationService = authenticationService;
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUserFavouriteOffer(string userId, Guid offerId)
        {
            // Check if offer exist in job offers db (? create separete service for that)

            // No ? Throw error 

            // Fetch user entitie
            // Add job offer to user entite

            //Save changes on db context
        }
    }
}
