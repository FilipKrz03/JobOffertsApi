using JobOffersApiCore.Enum;

namespace JobOffersService.Dto
{
    public record JobOfferBasicResponse(Guid Id, string OfferTitle, string OfferCompany,
        string Localization, string WorkMode, string OfferLink, string? PaymentRange, Seniority Seniority)
    {
        // Needed for AutoMapper
        public JobOfferBasicResponse() : this(Guid.Empty, "", "", "", "", "", "", Seniority.Unknown) { }
    }
}
