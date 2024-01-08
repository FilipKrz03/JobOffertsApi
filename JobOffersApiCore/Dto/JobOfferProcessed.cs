using JobOffersApiCore.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Dto
{
    public record JobOfferProcessed(Guid Id , string OfferTitle , string OfferCompany ,string Localization ,
        string WorkMode , IEnumerable<string> RequiredTechnologies , string OfferLink ,
        Seniority Seniority , int? EarningsFrom , int? EarningsTo ) { }
}
