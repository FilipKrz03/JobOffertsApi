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
    public class ProcessedJobOfferService : IProcessedOfferService
    {

        private readonly ITechnologyRepository _technologyRepository;
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly ILogger<ProcessedJobOfferService> _logger;
        private readonly IMapper _mapper;

        public ProcessedJobOfferService(
            ITechnologyRepository technologyRepository,
            ILogger<ProcessedJobOfferService> logger,
            IMapper mapper,
            IJobOfferRepository jobOfferRepository
            )
        {
            _jobOfferRepository = jobOfferRepository;
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

                var technologies = await _technologyRepository.GetAllTechnologiesAsync();

                var newTechnologies = processedJobOffer.RequiredTechnologies.Where
                    (t => technologies.All(t2 => t2.TechnologyName.ToLower() != t.ToLower()));

                var newTechnologiesEntites = _mapper.Map<IEnumerable<Technology>>(newTechnologies);

                _technologyRepository.AddRange(newTechnologiesEntites);

                await _technologyRepository.SaveChangesAsync();

                var jobOfferEntitie = _mapper.Map<JobOffer>(processedJobOffer);

                jobOfferEntitie.Technologies =
                    await _technologyRepository.GetEntitiesFromTechnologiesNamesAsync(processedJobOffer.RequiredTechnologies);

                _jobOfferRepository.Insert(jobOfferEntitie);

                await _jobOfferRepository.SaveChangesAsync();

                _logger.LogInformation("New job offer succesfully added to database !");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured on Handle processed offer {ex}", ex);
            }
        }
    }
}
