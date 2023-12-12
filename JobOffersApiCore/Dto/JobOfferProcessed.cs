using JobOffersApiCore.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Dto
{
    public record JobOfferProcessed(string OfferTitle , string OfferCompany ,string Localization ,
        string WorkMode , Seniority? Seniority , IEnumerable<string> RequiredTechnologies) { }
    
}
