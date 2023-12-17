using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Helpers;
using JobOffersService.Dto;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            Response<JobOfferDetailResponse> response = new();

            var jobOffer = await _jobOfferRepository.GetByIdQuery(jobId)
                .Include(e => e.Technologies).SingleAsync();

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

            var jobOffers = await _jobOfferRepository.GetJobOffersAsync(resourceParamethers);

            return response.ReturnValue(_mapper.Map<IEnumerable<JobOfferBasicResponse>>(jobOffers));
        }
    }
}
