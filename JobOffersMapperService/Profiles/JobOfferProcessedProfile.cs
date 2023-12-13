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
                .ConvertUsing<JobOfferConverter>();
        }
    }

    public class JobOfferConverter : ITypeConverter<JobOfferRaw, JobOfferProcessed>
    {
        public JobOfferProcessed Convert(JobOfferRaw source, JobOfferProcessed destination, ResolutionContext context)
        {
            var seniority = ConvertSeniority(source.Seniority);

            return new
                (source.OfferTitle, source.OfferCompany, source.Localization ,
                source.WorkMode, source.RequiredTechnologies , source.OfferLink , seniority);
        }

        private Seniority ConvertSeniority(string source) => source.ToLower() switch
        {
            _ when source.Contains("junior") => Seniority.Junior,
            _ when source.Contains("mid") => Seniority.Mid,
            _ when source.Contains("senior") => Seniority.Senior,
            _ => Seniority.Unknown
        };
    }
}
