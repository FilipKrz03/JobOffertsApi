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
    public class JobOfferBaseProfile : Profile
    {
        public JobOfferBaseProfile()
        {
            CreateMap<JobOfferRaw, JobOfferBase>();
        }
    }
}
