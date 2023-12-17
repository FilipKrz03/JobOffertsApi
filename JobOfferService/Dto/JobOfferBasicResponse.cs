using JobOffersApiCore.Enum;

namespace JobOffersService.Dto
{
    public record JobOfferBasicResponse(string OfferTitle, string OfferCompany,
        string Localization, string WorkMode, string OfferLink, Seniority Seniority) { }
}
