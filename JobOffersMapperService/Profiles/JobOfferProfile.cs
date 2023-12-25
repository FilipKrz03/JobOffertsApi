using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersMapperService.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Profiles
{
    public class JobOfferProfile : Profile
    {
        public JobOfferProfile()
        {
            CreateMap<JobOfferRaw, JobOfferBase>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); 
            CreateMap<JobOfferRaw, JobOfferProcessed>()
              .ConvertUsing<JobOfferRawConverter>();
        }
    }
}
