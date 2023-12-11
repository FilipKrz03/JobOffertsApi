﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Dto
{
    public record JobOfferRaw(string JobTitle, string Company, string Localization,
      string WorkMode, string Seniority, IEnumerable<string> RequiredTechnologies , string JobLink);
}
