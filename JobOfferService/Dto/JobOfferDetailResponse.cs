using JobOffersApiCore.Enum;
using JobOffersService.Entities;

namespace JobOffersService.Dto
{
    public record JobOfferDetailResponse(string OfferTitle, string OfferCompany,
        string Localization, string WorkMode, string OfferLink, Seniority Seniority , 
        IEnumerable<Technology> Technologies)
    { }
}
