using JobOffersApiCore.Enum;

namespace UsersService.Dto
{
    public record JobOfferDetailResponseDto(Guid Id, string OfferTitle, string OfferCompany,
        string Localization, string WorkMode, string OfferLink, string? PaymentRange, Seniority Seniority,
        IEnumerable<TechnologyBasicResponseDto> Technologies)
    {
        // Needed for AutoMapper
        public JobOfferDetailResponseDto() : this(Guid.Empty, "", "", "", "", "", "",
            Seniority.Unknown, Enumerable.Empty<TechnologyBasicResponseDto>())
        { }
    }

}
