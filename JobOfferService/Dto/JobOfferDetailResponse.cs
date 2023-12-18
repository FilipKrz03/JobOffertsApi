using JobOffersApiCore.Enum;
using JobOffersService.Entities;

namespace JobOffersService.Dto
{
    public record JobOfferDetailResponse(Guid Id , string OfferTitle, string OfferCompany,
        string Localization, string WorkMode, string OfferLink, Seniority Seniority , 
        IEnumerable<TechnologyBasicResponse> Technologies)
    { }
}
