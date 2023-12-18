using JobOffersApiCore.Enum;

namespace JobOffersService.Dto
{
    public record JobOfferBasicResponse(Guid Id , string OfferTitle, string OfferCompany,
        string Localization, string WorkMode, string OfferLink, Seniority Seniority) { }
}
