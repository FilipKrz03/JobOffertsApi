using JobOffersApiCore.Exceptions;
using UsersService.Exceptions;
using UsersService.Interfaces;

namespace UsersService.Services
{
    public class SubscribedTechnologyService : ISubscribedTechnologyService
    {

        private readonly IClaimService _claimService;
        private readonly IUserRepository _userRepository;
        private readonly ITechnologyRepository _tecnologyRepository;
        private readonly ITechnologyUserJoinRepository _technologyUserJoinRepository;

        public SubscribedTechnologyService(IClaimService claimService, IUserRepository userRepository , 
            ITechnologyRepository technologyRepository , ITechnologyUserJoinRepository technologyUserJoinRepository)
        {
            _claimService = claimService;
            _userRepository = userRepository;   
            _tecnologyRepository = technologyRepository;
            _technologyUserJoinRepository = technologyUserJoinRepository;
        }

        public async Task AddSubscribedTechnology(Guid technologyId)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            var user = await _userRepository.GetById(userId);

            if(user == null)
            {
                throw new InvalidAccesTokenException
                    ("User with id from your acces token do not exist provide valid token !");
            }

            var technology = await _tecnologyRepository.GetById(technologyId);
            
            if (technology == null)
            {
                throw new ResourceNotFoundException
                    ($"Technology with id {technologyId} do not exist in our database");
            }

            bool userTechnologyExist = await _technologyUserJoinRepository
                .UserTechnologyExistAsync(userId, technologyId);

            if(userTechnologyExist)
            {
                throw new ResourceAlreadyExistException($"User already subscribe technology with id {technologyId}");
            }

            user.Technologies.Add(technology);

            await _userRepository.SaveChangesAsync();
        }
    }
}
