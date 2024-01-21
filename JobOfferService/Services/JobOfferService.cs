using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Exceptions;
using JobOffersService.Dto;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace JobOffersService.Services
{
    public class JobOfferService : IJobOfferService
    {

        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<JobOfferService> _logger;  

        public JobOfferService(
            IJobOfferRepository jobOfferRepository , 
            IMapper mapper , 
            ILogger<JobOfferService> logger
            )
        {
            _jobOfferRepository = jobOfferRepository;
            _mapper = mapper;
            _logger = logger;   
        }

        public async Task<JobOfferDetailResponse> GetJobOfferDetailAsync(Guid jobId)
        {
            var jobOffer = await _jobOfferRepository.GetJobOfferWithTechnologies(jobId);

            if (jobOffer == null)
            {
                throw new ResourceNotFoundException($"Job offer with id {jobId} do not exist");
            }

            return _mapper.Map<JobOfferDetailResponse>(jobOffer);
        }

        public async Task<PagedList<JobOfferBasicResponse>>
            GetJobOffersAsync(ResourceParamethers resourceParamethers)
        {
            Expression<Func<JobOffer, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "title" => jobOffer => jobOffer.OfferTitle,
                "link" => jobOffer => jobOffer.OfferLink,
                "company" => jobOffer => jobOffer.OfferCompany,
                "localization" => jobOffer => jobOffer.Localization,
                "earnings" => jobOffer => jobOffer.EarningsFrom ?? 0 , 
                "seniority" => jobOffer => jobOffer.Seniority ,
                _ => jobOffer => jobOffer.CreatedAt!
            };
            
            var jobOffers = await _jobOfferRepository.GetJobOffersAsync(resourceParamethers , keySelector);

            return _mapper.Map<PagedList<JobOfferBasicResponse>>(jobOffers);
        }

        public async Task JobOfferExistAsync(Guid id)
        {
            bool jobOfferExist =  await _jobOfferRepository.EntityExistAsync(id);

            if(jobOfferExist == false)
            {
                throw new ResourceNotFoundException("Job offer not found");
            }
        }

        public async Task DeleteJobOfferFromEventAsync(string message)
        {
            var jobOfferFromEvent = JsonConvert.DeserializeObject<JobOfferToDeleteFromEventDto>(message);

            if(jobOfferFromEvent == null)
            {
                _logger.LogWarning("DeleteJobOfferFromEvent - failed to parse message");
                return;
            }

            var jobOfferEntitie =  await _jobOfferRepository.GetByIdAsync(jobOfferFromEvent.Id);

            if(jobOfferEntitie == null)
            {
                _logger.LogWarning
                    ("DeleteJobOfferFromEvent - Job offer entitie with if from message not found");
                return;
            }

            _jobOfferRepository.DeleteEntity(jobOfferEntitie);

            await _jobOfferRepository.SaveChangesAsync();
        }
    }
}
