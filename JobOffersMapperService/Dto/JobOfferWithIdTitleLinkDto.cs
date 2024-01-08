using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Dto
{
    public record JobOfferWithIdTitleLinkDto(Guid Id, string OfferTitle, string OfferLink) { }
}
