using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Helpers;
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

        public TechnologyService(ITechnologyRepository technologyRepository , 
            IMapper mapper)
        {
            _technologyRepository = technologyRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<TechnologyBasicResponse>>>
            GetTechnologies(ResourceParamethers resourceParamethers)
        {
            Expression<Func<Technology, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "name" => technology => technology.TechnologyName,
                _ => technology => technology.CreatedAt!
            };

            var technologieEntities = 
                await _technologyRepository.GetTechnologiesAsync(resourceParamethers, keySelector);

            return Response<IEnumerable<TechnologyBasicResponse>>.
                ReturnValue(_mapper.Map<IEnumerable<TechnologyBasicResponse>>(technologieEntities));
        }

        public async Task<Response<TechnologyDetailResponse>> GetTechnologyWithJobOffers
            (Guid id , ResourceParamethers resourceParamethers)
        {
            var technologyEntityWithJobOffers = await _technologyRepository.
                GetTechnologyWithJobOffersAsync(id , resourceParamethers);

            if(technologyEntityWithJobOffers == null)
            {
                return Response<TechnologyDetailResponse>.ReturnError(404, $"Technology with id {id} not found");
            }

            return Response<TechnologyDetailResponse>.
                ReturnValue(_mapper.Map<TechnologyDetailResponse>(technologyEntityWithJobOffers));
        }
    }
}
