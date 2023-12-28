
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

        public UsersService(Interfaces.IAuthenticationService authenticationService , IUserRepository userRepository , 
            IMapper mapper , IJwtProvider jwtProvider)
        {
            _authenticationService = authenticationService;
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;   
            _mapper = mapper;
        }

        public async Task RegisterUser(RegisterRequestDto request)
        {
            var user = _mapper.Map<User>(request);  

            // If user already exist firebase error will be thrown and properly served by exception handler middleware
            var identityId = await _authenticationService.RegisterAsync(request.Email, request.Password , request.UserName);

            user.IdentityId = identityId;

            _userRepository.Insert(user);

            await _userRepository.SaveChangesAsync();
        }

        public async Task<TokenResponseDto> LoginUser(LoginRequestDto request)
        {
            return await _jwtProvider.GetForCredentialsAsync(request.Email, request.Password);
        }

        public async Task<TokenResponseDto> RefreshUserAccessToken(string refreshToken)
        {
            return await _jwtProvider.GetForRefreshTokenAsync(refreshToken);
        }
    }
}
