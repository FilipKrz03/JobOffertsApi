using JobOffersApiCore.BaseObjects;

namespace UsersService.Entities
{
    public class Technology : BaseEntity
    {
        public string TechnologyName { get; set; } = string.Empty;
        public List<JobOffer> JobOffers { get; set; } = new();
    }
}
