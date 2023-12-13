using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Enum;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Security.Cryptography;

namespace JobOffersService.Services
{
    public class ProcessedOfferService : IProcessedOfferService
    {

        private readonly ITechnologyRepository _technologyRepository;
        private readonly ILogger<ProcessedOfferService> _logger;
        private readonly IMapper _mapper;

        public ProcessedOfferService(ITechnologyRepository technologyRepository , 
            ILogger<ProcessedOfferService> logger , IMapper mapper)
        {
            _technologyRepository = technologyRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task HandleProcessedOffer(string body)
        {
            try
            {
                var processedJobOffer = JsonConvert.DeserializeObject<JobOfferProcessed>(body);

                if (processedJobOffer == null) return;

                var technologies = await _technologyRepository.GetTechnologies();

                var newTechnologies = processedJobOffer.RequiredTechnologies.Where
                    (t => technologies.All(t2 => t2.TechnologyName != t));

                var newTechnologiesEntites = _mapper.Map<IEnumerable<Technology>>(newTechnologies);

                _technologyRepository.AddRange(newTechnologiesEntites);

                await _technologyRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured on Handle processed offer {ex}", ex);

            }
        }
    }
}
