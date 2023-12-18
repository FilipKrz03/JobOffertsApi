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
            Response<IEnumerable<TechnologyBasicResponse>> response = new();

            Expression<Func<Technology, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "name" => technology => technology.TechnologyName,
                _ => technology => technology.Id
            };

            var technologieEntities = 
                await _technologyRepository.GetTechnologiesAsync(resourceParamethers, keySelector);

            return response.ReturnValue(_mapper.Map<IEnumerable<TechnologyBasicResponse>>(technologieEntities));
        }
    }
}
