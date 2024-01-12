using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Exceptions;
using JobOffersService.Dto;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using System.Linq.Expressions;

namespace JobOffersService.Services
{
    public class TechnologyService : ITechnologyService
    {

        private readonly ITechnologyRepository _technologyRepository;
        private readonly IMapper _mapper;

        public TechnologyService(
            ITechnologyRepository technologyRepository,
            IMapper mapper
            )
        {
            _technologyRepository = technologyRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<TechnologyBasicResponse>>
            GetTechnologies(ResourceParamethers resourceParamethers)
        {
            Expression<Func<Technology, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "name" => technology => technology.TechnologyName,
                _ => technology => technology.CreatedAt!
            };

            var technologieEntities =
                await _technologyRepository.GetTechnologiesAsync(resourceParamethers, keySelector);

            return _mapper.Map<PagedList<TechnologyBasicResponse>>(technologieEntities);
        }

        public async Task<TechnologyDetailResponse> GetTechnologyWithJobOffers
            (Guid id, ResourceParamethers resourceParamethers)
        {
            var technologyEntityWithJobOffers = await _technologyRepository.
                GetTechnologyWithJobOffersAsync(id, resourceParamethers);

            if (technologyEntityWithJobOffers == null)
            {
                throw new ResourceNotFoundException($"Technology with id {id} do not exist");
            }

            return _mapper.Map<TechnologyDetailResponse>(technologyEntityWithJobOffers);
        }
    }
}
