using JobOffersApiCore.Dto;
using JobOffersMapperService.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Services
{
    public class JobOfferBaseService : IJobOfferBaseService
    {

        private readonly IJobOffersBaseRepository _jobOffersBaseRepository;
        private readonly ILogger<JobOfferBaseService> _logger;

        public JobOfferBaseService(
            IJobOffersBaseRepository jobOffersBaseRepository,
            ILogger<JobOfferBaseService> logger
            )
        {
            _jobOffersBaseRepository = jobOffersBaseRepository;
            _logger = logger;
        }

        public async Task DeleteJobOfferFromEventAsync(string message)
        {
            var jobOfferToDelete = JsonConvert.DeserializeObject<JobOfferToDeleteFromEventDto>(message);

            if (jobOfferToDelete == null)
            {
                _logger.LogWarning("DeleteJobOfferFromEvent - Failed to parse message");
                return;
            }

            var jobOfferEntitie = await _jobOffersBaseRepository.GetByIdAsync(jobOfferToDelete.Id);

            if (jobOfferEntitie == null)
            {
                _logger.LogWarning("DeleteJobOfferFromEvent - Not found job offer entitie from id from event");
                return;
            }

            _jobOffersBaseRepository.DeleteEntity(jobOfferEntitie);

            await _jobOffersBaseRepository.SaveChangesAsync();
        }
    }
}
