using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Enum;

namespace UsersService.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;

        public Seniority DesiredSeniority { get; set; } = Seniority.Unknown;

        public List<JobOffer> JobOffers { get; set; } =
           new List<JobOffer>();
        public List<JobOfferUser> JobOfferUsers { get; set; } = new();

        public List<Technology> Technologies { get; set; } = new();
        public List<TechnologyUser> TechnologyUsers { get; set; } = new();
    }
}
