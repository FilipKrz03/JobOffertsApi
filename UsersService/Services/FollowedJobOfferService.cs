using JobOffersApiCore.Exceptions;
using UsersService.Entities;
using UsersService.Exceptions;
using UsersService.Interfaces;

namespace UsersService.Services
{

    public class FollowedJobOfferService : IFollowedJobOfferService
    {

        private readonly IUserRepository _userRepository;
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IClaimService _claimService;
        private readonly IJobOfferUserJoinRepository _jobOfferUserJoinRepository;

        public FollowedJobOfferService(IUserRepository userRepository , 
            IJobOfferRepository jobOfferRepository , IClaimService claimService , 
            IJobOfferUserJoinRepository jobOfferUserJoinRepository)
        {
            _userRepository = userRepository;
            _jobOfferRepository = jobOfferRepository;
            _claimService = claimService;
            _jobOfferUserJoinRepository = jobOfferUserJoinRepository;
        }

        public async Task AddFolowedJobOffer(Guid offerId)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            var user = await _userRepository.GetById(userId);

            if(user == null)
            {
                throw new InvalidAccesTokenException
                    ("User with id from your acces token do not exist provide valid token !");
            }

            var jobOffer = await _jobOfferRepository.GetById(offerId);

            if (jobOffer == null)
            {
                throw new ResourceNotFoundException
                    ($"Job offer with id {offerId} not found in our database");
            }

            bool userJobOfferExist = await _jobOfferUserJoinRepository.UserJobOfferExist(userId, offerId);
          
            if (userJobOfferExist)
            {
                throw new ResourceAlreadyExistException
                    ($"You alredy have offer with id {offerId} in your following offers");
            }

            user.JobOffers.Add(jobOffer);

            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteFollowedJobOffer(Guid followedJobOfferId)
        {
          
           
        }
    }
}
