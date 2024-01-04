using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Exceptions;
using JobOffersService.Dto;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobOffersService.Services
{
    public class JobOfferService : IJobOfferService
    {

        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IMapper _mapper;

        public JobOfferService(IJobOfferRepository jobOfferRepository , 
            IMapper mapper)
        {
            _jobOfferRepository = jobOfferRepository;
            _mapper = mapper;
        }

        public async Task<JobOfferDetailResponse> GetJobOfferDetail(Guid jobId)
        {
            var jobOffer = await _jobOfferRepository.GetJobOfferWithTechnologies(jobId);

            if (jobOffer == null)
            {
                throw new ResourceNotFoundException($"Job offer with id {jobId} do not exist");
            }

            return _mapper.Map<JobOfferDetailResponse>(jobOffer);
        }

        public async Task<IEnumerable<JobOfferBasicResponse>>
            GetJobOffers(ResourceParamethers resourceParamethers)
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

            return _mapper.Map<IEnumerable<JobOfferBasicResponse>>(jobOffers);
        }

        public async Task JobOfferExist(Guid id)
        {
            bool jobOfferExist =  await _jobOfferRepository.EntityExistAsync(id);

            if(jobOfferExist == false)
            {
                throw new ResourceNotFoundException("Job offer not found");
            }
        }
    }
}
