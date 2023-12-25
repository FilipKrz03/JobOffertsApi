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
            CreateMap<JobOfferProcessed, JobOffer>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.Technologies, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<JobOffer, JobOfferDetailResponse>()
                .ForMember(dest => dest.PaymentRange, opt =>
                opt.MapFrom(src => PaymentRangeStringConverter(src.EarningsFrom, src.EarningsTo)));
            CreateMap<JobOffer, JobOfferBasicResponse>()
                .ForMember(dest => dest.PaymentRange, opt =>
                opt.MapFrom(src => PaymentRangeStringConverter(src.EarningsFrom, src.EarningsTo)));
        }

        private string? PaymentRangeStringConverter(int? earningsFrom, int? earningsTo)
        {
            if (earningsFrom == null && earningsTo == null) return null;

            return $"{earningsFrom}-{earningsTo} zł";
        }
    }
}
