using AutoMapper;
using FirebaseAdmin.Auth;
using UsersService.Dto;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Services
{

    public class AuthenticationService : IAuthenticationService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AuthenticationService(IUserRepository userRepository , IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterRequestDto request)
        {
            var user = _mapper.Map<User>(request);

            var userArgs = new UserRecordArgs()
            {
                Email = request.Email,
                Password = request.Password,
                DisplayName = request.UserName , 
                EmailVerified = true
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            user.IdentityId = userRecord.Uid;

            _userRepository.Insert(user);

            await _userRepository.SaveChangesAsync();
        }
    }
}
