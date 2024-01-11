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
                .ForMember(x => x.Id, opt => opt.MapFrom(x => GetGuid()))
                   .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<JobOfferRaw, JobOfferProcessed>()
              .ConvertUsing<JobOfferRawConverter>();
        }

        private Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}
