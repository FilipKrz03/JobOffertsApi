using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Exceptions;
using System.Linq.Expressions;
using UsersService.Dto;
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
        private readonly IMapper _mapper;

        public FollowedJobOfferService(IUserRepository userRepository,
            IJobOfferRepository jobOfferRepository, IClaimService claimService,
            IJobOfferUserJoinRepository jobOfferUserJoinRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _jobOfferRepository = jobOfferRepository;
            _claimService = claimService;
            _jobOfferUserJoinRepository = jobOfferUserJoinRepository;
            _mapper = mapper;
        }

        public async Task AddFolowedJobOffer(Guid offerId)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            var user = await _userRepository.GetById(userId);

            if (user == null)
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

            bool userJobOfferExist = await _jobOfferUserJoinRepository.UserJobOfferExistAsync(userId, offerId);

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
            var userId = _claimService.GetUserIdFromTokenClaim();

            var jobOfferUser = await _jobOfferUserJoinRepository
                .GetUserJobOfferJoinAsync(userId, followedJobOfferId);

            if (jobOfferUser == null)
            {
                throw new ResourceNotFoundException
                    ("Job offer already do not exist on user followed offers");
            }

            _jobOfferUserJoinRepository.RemoveUserJobOffer(jobOfferUser);

            await _jobOfferUserJoinRepository.SaveChangesAsync();
        }

        public async Task<JobOfferDetailResponseDto?> GetFollowedJobOffer(Guid followedJobOfferId)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            var userJobOffer = await _jobOfferRepository.
                GetUserJobOffer(userId, followedJobOfferId);

            if (userJobOffer == null)
            {
                throw new ResourceNotFoundException
                    ($"Job offer with id {followedJobOfferId} do not exist in user followed offers");
            }

            return _mapper.Map<JobOfferDetailResponseDto>(userJobOffer);
        }

        public async Task<PagedList<JobOfferBasicResponseDto>>
            GetFollowedJobOffers(ResourceParamethers resourceParamethers)
        {
            var usedId = _claimService.GetUserIdFromTokenClaim();

            Expression<Func<JobOffer, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "title" => jobOffer => jobOffer.OfferTitle,
                "link" => jobOffer => jobOffer.OfferLink,
                "company" => jobOffer => jobOffer.OfferCompany,
                "localization" => jobOffer => jobOffer.Localization,
                "earnings" => jobOffer => jobOffer.EarningsFrom ?? 0,
                _ => jobOffer => jobOffer.CreatedAt!
            };

            var userOffersPagedList = await _jobOfferRepository
                .GetUserJobOffersAsync(keySelector, resourceParamethers, usedId);

            return _mapper.Map<PagedList<JobOfferBasicResponseDto>>(userOffersPagedList);
        }
    }
}
