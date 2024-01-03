using JobOffersApiCore.BaseObjects;

namespace UsersService.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public List<JobOffer> JobOffers { get; set; } =
           new List<JobOffer>();
        public List<JobOfferUser> JobOfferUsers { get; set; } = new();
    }
}
