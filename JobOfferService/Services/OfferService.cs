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
    public class OfferService : IOfferService
    {

        private readonly IOfferRepository _jobOfferRepository;
        private readonly IMapper _mapper;

        public OfferService(IOfferRepository jobOfferRepository , 
            IMapper mapper)
        {
            _jobOfferRepository = jobOfferRepository;
            _mapper = mapper;
        }

        public async Task<Response<JobOfferDetailResponse>> GetJobOfferDetail(Guid jobId)
        {
            Response<JobOfferDetailResponse> response = new();

            var jobOffer = await _jobOfferRepository.GetJobOfferWithTechnologies(jobId);

            if (jobOffer == null)
            {
               return response.ReturnError(404, "Job offer not found");
            }

            return response.ReturnValue(_mapper.Map<JobOfferDetailResponse>(jobOffer));
        }

        public async Task<Response<IEnumerable<JobOfferBasicResponse>>>
            GetJobOffers(ResourceParamethers resourceParamethers)
        {
            Response<IEnumerable<JobOfferBasicResponse>> response = new();

            Expression<Func<JobOffer, object>> keySelector = resourceParamethers.SortColumn?.ToLower() switch
            {
                "title" => jobOffer => jobOffer.OfferTitle,
                "link" => jobOffer => jobOffer.OfferLink,
                "company" => jobOffer => jobOffer.OfferCompany,
                "localization" => jobOffer => jobOffer.Localization,
                "earnings" => jobOffer => jobOffer.EarningsFrom ?? 0 , 
                _ => jobOffer => jobOffer.Id
            };

            var jobOffers = await _jobOfferRepository.GetJobOffersAsync(resourceParamethers , keySelector);

            return response.ReturnValue(_mapper.Map<IEnumerable<JobOfferBasicResponse>>(jobOffers));
        }
    }
}
