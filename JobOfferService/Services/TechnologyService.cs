using JobOffersApiCore.Common;
using JobOffersApiCore.Helpers;
using JobOffersService.Dto;
using JobOffersService.Interfaces;

namespace JobOffersService.Services
{
    public class TechnologyService
    {

        private readonly ITechnologyRepository _technologyRepository;

        public TechnologyService(ITechnologyRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }

        public async Task<Response<IEnumerable<TechnologyBasicResponse>>>
            GetTechnologies(ResourceParamethers resourceParamethers)
        {
            Response<IEnumerable<TechnologyBasicResponse>> response = new();
        }
    }
}
