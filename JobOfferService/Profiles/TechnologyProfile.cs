using AutoMapper;
using JobOffersService.Dto;
using JobOffersService.Entities;

namespace JobOffersService.Profiles
{
    public class TechnologyProfile : Profile
    {
        public TechnologyProfile()
        {
            CreateMap<string, Technology>()
                .ForMember(dest => dest.TechnologyName, opt => opt.MapFrom(
                    src => src));
            CreateMap<Technology, TechnologyBasicResponse>();
            CreateMap<Technology, TechnologyDetailResponse>();
        }
    }
}
