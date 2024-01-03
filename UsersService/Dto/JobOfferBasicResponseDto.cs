using JobOffersApiCore.Enum;

namespace UsersService.Dto
{
    public record JobOfferBasicResponseDto(Guid Id, string OfferTitle, string OfferCompany,
      string Localization, string WorkMode, string OfferLink, string? PaymentRange, Seniority Seniority)
    {
        // Needed for AutoMapper
        public JobOfferBasicResponseDto() : this(Guid.Empty, "", "", "", "", "", "", Seniority.Unknown) { }
    }
}
