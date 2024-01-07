using JobOffersApiCore.Enum;
using UsersService.Exceptions;
using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Services
{
    
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClaimService _claimService;
        public UserService(
            IUserRepository userRepository , IClaimService claimService)
        {
            _userRepository = userRepository;
            _claimService = claimService;
        }

        public async Task UpdateUserSeniority(Seniority seniority)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            var user = await _userRepository.GetById(userId);

            if(user == null)
            {
                throw new InvalidAccesTokenException("User with id from your token do not exist !");
            }

            user.DesiredSeniority = seniority;

            await _userRepository.SaveChangesAsync();
        }
    }
}
