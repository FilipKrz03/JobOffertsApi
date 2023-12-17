using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersService.Dto;
using JobOffersService.Entities;

namespace JobOffersService.Profiles
{
    public class JobOfferProfile : Profile
    {
        public JobOfferProfile()
        {
            CreateMap<JobOfferProcessed, JobOffer>();
            CreateMap<JobOffer, JobOfferDetailResponse>();
        }
    }
}
