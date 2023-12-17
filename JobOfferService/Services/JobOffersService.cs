using JobOffersApiCore.Helpers;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobOffersService.Services
{

    public class JobOffersService : IJobOffersService
    {

        private readonly IJobOfferRepository _jobOfferRepository;

        public JobOffersService(IJobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;
        }


        public async Task<Response<JobOffer>> GetJobOffer(Guid jobId)
        {
            Response<JobOffer> response = new();

            var jobOffer = await _jobOfferRepository.GetByIdQuery(jobId)
                .Include(e => e.Technologies).SingleAsync();

            if (jobOffer == null)
            {
               return response.ReturnError(404, "Job offer not found");
            }

            return response.ReturnValue(jobOffer);
        }
    }
}
