using AutoMapper;
using JobOffersApiCore.Helpers;
using JobOffersService.Dto;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobOffersService.Services
{

    public class JobOffersService : IJobOffersService
    {

        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IMapper _mapper;

        public JobOffersService(IJobOfferRepository jobOfferRepository , 
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

            var jobOfferToReturn = _mapper.Map<JobOfferDetailResponse>(jobOffer);

            return response.ReturnValue(jobOfferToReturn);
        }
    }
}
