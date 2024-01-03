using AutoMapper;
using UsersService.Dto;
using UsersService.Entities;

namespace UsersService.Profiles
{
    public class JobOfferProfile : Profile
    {
        public JobOfferProfile()
        {
            CreateMap<JobOffer, JobOfferDetailResponseDto>()
               .ForMember(dest => dest.PaymentRange, opt =>
               opt.MapFrom(src => PaymentRangeStringConverter(src.EarningsFrom, src.EarningsTo)));
               CreateMap<JobOffer, JobOfferBasicResponseDto>()
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
