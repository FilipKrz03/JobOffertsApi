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
    public class JobOfferRawConverter : ITypeConverter<JobOfferRaw, JobOfferProcessed>
    {
        public JobOfferProcessed Convert(JobOfferRaw source, JobOfferProcessed destination, ResolutionContext context)
        {
            var seniority = ConvertSeniority(source.Seniority);
            (int? earningsFrom, int? eariningsTo) = GetEarnings(source.salaryString);

            return new
                (source.OfferTitle, source.OfferCompany, source.Localization,
                source.WorkMode, source.RequiredTechnologies, source.OfferLink, seniority, earningsFrom, eariningsTo);
        }

        private (int? earningsFrom, int? earningsTo) GetEarnings(string? earningsString)
        {

            if (earningsString == null) return (null, null);

            char enDash = (char)0x2013;
            string[] splitEarningStringArray = earningsString.Split(new char[] { enDash, '-' });

            try
            {
                int? earningsFrom = TryParseInt(splitEarningStringArray[0].Replace(" ", ""));
                int? earningsTo = TryParseInt(splitEarningStringArray[1].Replace(" ", "").Replace("zł", ""));
                return (earningsFrom, earningsTo);
            }
            catch
            {
                return (null, null);
            }
        }

        private int? TryParseInt(string source)
        {
            int? value;
            int score;

            if (int.TryParse(source, out score))
            {
                value = score;
            }
            else
            {
                value = null;
            }

            return value;
        }

        private Seniority ConvertSeniority(string source) => source.ToLower().Trim() switch
        {
            "junior" => Seniority.Junior,
            "mid" => Seniority.Mid,
            "senior" => Seniority.Senior,
            _ => Seniority.Unknown
        };
    }
}
