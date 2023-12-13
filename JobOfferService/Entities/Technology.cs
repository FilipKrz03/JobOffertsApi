using JobOffersApiCore.BaseObjects;

namespace JobOffersService.Entities
{
    // Many to many realationship with JobOffer Entitie 
    public class Technology : BaseEntity
    {
        public string TechnologyName { get; set; } = string.Empty;

        public List<JobOffer> JobOffers { get; set; } = new();
    }
}
