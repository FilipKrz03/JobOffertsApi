using JobOffersApiCore.BaseObjects;

namespace JobOffersService.Entities
{
    // Many to many realationship with JobOffer Entitie 
    public class Technology : BaseEntity
    {
        public string TechnologyName {  get; set; }

        public List<JobOffer> JobOffers { get; set; } = new();

        public Technology(string technologyName)
        {
            TechnologyName = technologyName;
        }
    }
}
