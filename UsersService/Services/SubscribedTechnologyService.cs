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
    public class SubscribedTechnologyService : ISubscribedTechnologyService
    {

        private readonly IClaimService _claimService;
        private readonly IUserRepository _userRepository;
        private readonly ITechnologyRepository _tecnologyRepository;
        private readonly ITechnologyUserJoinRepository _technologyUserJoinRepository;
        private readonly IMapper _mapper;

        public SubscribedTechnologyService(IClaimService claimService, IUserRepository userRepository,
            ITechnologyRepository technologyRepository, ITechnologyUserJoinRepository technologyUserJoinRepository , 
            IMapper mapper)
        {
            _claimService = claimService;
            _userRepository = userRepository;
            _tecnologyRepository = technologyRepository;
            _technologyUserJoinRepository = technologyUserJoinRepository;
            _mapper = mapper;
        }

        public async Task AddSubscribedTechnology(Guid technologyId)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            var user = await _userRepository.GetById(userId);

            if (user == null)
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

            if (userTechnologyExist)
            {
                throw new ResourceAlreadyExistException($"User already subscribe technology with id {technologyId}");
            }

            user.Technologies.Add(technology);

            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteSubscribedTechnology(Guid subscribedTechnologyId)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            var userTechnologyJoinEntity = await
                _technologyUserJoinRepository.GetTechnologyUserJoinEntitiyAsync(userId, subscribedTechnologyId);

            if (userTechnologyJoinEntity == null)
            {
                throw new ResourceNotFoundException($"User do not subscibe technology with id {subscribedTechnologyId}");
            }

            _technologyUserJoinRepository.DeleteTechnologyUserJoinEntity(userTechnologyJoinEntity);

            await _technologyUserJoinRepository.SaveChangesAsync();
        }

        public async Task<PagedList<TechnologyBasicResponseDto>>
            GetSubscribedTechnologies(ResourceParamethers resourceParamethers)
        {
            var userId = _claimService.GetUserIdFromTokenClaim();

            Expression<Func<Technology, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "name" => technology => technology.TechnologyName,
                _ => technology => technology.CreatedAt!
            };

            var sortedAndFilteredUserOffers = await _tecnologyRepository
                .GetUserTechnologiesAsync(resourceParamethers, keySelector, userId);

            return _mapper.Map<PagedList<TechnologyBasicResponseDto>>(sortedAndFilteredUserOffers);
        }
    }
}
