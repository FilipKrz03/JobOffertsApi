﻿using AutoMapper;
using JobOffersService.Dto;
using JobOffersService.Entities;

namespace JobOffersService.Profiles
{
    public class TechnologyProfile : Profile
    {
        public TechnologyProfile()
        {
            CreateMap<string, Technology>()
                .ForMember(dest => dest.JobOffers, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TechnologyName, opt => opt.MapFrom(
                    src => src))
                   .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<Technology, TechnologyBasicResponse>();
            CreateMap<Technology, TechnologyDetailResponse>();
        }
    }
}
