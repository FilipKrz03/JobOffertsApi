using JobOffersApiCore.Dto;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Newtonsoft.Json;

namespace JobOffersService.Services
{
    public class ProcessedOfferSercvice
    {

        private readonly ITechnologyRepository _technologyRepository;
        private readonly ILogger<ProcessedOfferSercvice> _logger;   

        public ProcessedOfferSercvice(ITechnologyRepository technologyRepository , 
            ILogger<ProcessedOfferSercvice> logger)
        {
            _technologyRepository = technologyRepository;
            _logger = logger;
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
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured on Handle processed offer {ex}" , ex);
            }

        }
    }
}
