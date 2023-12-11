using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Dto
{
    public record JobOfferRaw(string OfferTitle , string OfferCompany, string Localization,
      string WorkMode, string Seniority, IEnumerable<string> RequiredTechnologies , string OfferLink);
}
