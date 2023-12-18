
namespace JobOffersService.Dto
{
    public record TechnologyDetailResponse
        (string TechnologyName , IEnumerable<JobOfferBasicResponse> jobOffers) { }
}
