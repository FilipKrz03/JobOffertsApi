
namespace JobOffersService.Dto
{
    public record TechnologyDetailResponse
        (Guid Id, string TechnologyName , IEnumerable<JobOfferBasicResponse> jobOffers) { }
}
