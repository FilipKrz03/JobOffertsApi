using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Helpers;
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

        public async Task<Response<JobOfferDetailResponse>> GetJobOfferDetail(Guid jobId)
        {
            var jobOffer = await _jobOfferRepository.GetJobOfferWithTechnologies(jobId);

            if (jobOffer == null)
            {
               return Response<JobOfferDetailResponse>.ReturnError(404, "Job offer not found");
            }

            return Response<JobOfferDetailResponse>.ReturnValue(_mapper.Map<JobOfferDetailResponse>(jobOffer));
        }

        public async Task<Response<IEnumerable<JobOfferBasicResponse>>>
            GetJobOffers(ResourceParamethers resourceParamethers)
        {
            Expression<Func<JobOffer, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "title" => jobOffer => jobOffer.OfferTitle,
                "link" => jobOffer => jobOffer.OfferLink,
                "company" => jobOffer => jobOffer.OfferCompany,
                "localization" => jobOffer => jobOffer.Localization,
                "earnings" => jobOffer => jobOffer.EarningsFrom ?? 0 , 
                _ => jobOffer => jobOffer.CreatedAt!
            };
            
            var jobOffers = await _jobOfferRepository.GetJobOffersAsync(resourceParamethers , keySelector);

            return Response<IEnumerable<JobOfferBasicResponse>>
                .ReturnValue(_mapper.Map<IEnumerable<JobOfferBasicResponse>>(jobOffers));
        }
    }
}
