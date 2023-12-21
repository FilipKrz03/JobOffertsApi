using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Profiles
{
    public class JobOfferProcessedProfile : Profile
    {
        public JobOfferProcessedProfile()
        {
            CreateMap<JobOfferRaw, JobOfferProcessed>()
                .ConvertUsing<JobOfferRawConverter>();
        }
    }
}
