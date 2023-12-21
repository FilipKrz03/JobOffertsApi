using JobOffersApiCore.Enum;
using JobOffersService.Entities;

namespace JobOffersService.Dto
{
    public record JobOfferDetailResponse(Guid Id, string OfferTitle, string OfferCompany,
        string Localization, string WorkMode, string OfferLink, string? PaymentRange, Seniority Seniority,
        IEnumerable<TechnologyBasicResponse> Technologies)
    {
        // Needed for AutoMapper
        public JobOfferDetailResponse() : this(Guid.Empty, "", "", "", "", "", "",
            Seniority.Unknown, Enumerable.Empty<TechnologyBasicResponse>()){ }   
    }
}
