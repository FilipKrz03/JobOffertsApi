using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Enum;

namespace JobOffersService.Entities
{
    // Many to many realationship with Technology Entitie 
    public class JobOffer : BaseEntity
    {
        public string OfferTitle { get; set; } = string.Empty;
        public string OfferCompany { get; set; } = string.Empty;
        public string Localization { get; set; } = string.Empty;
        public string WorkMode { get; set; } = string.Empty;
        public string OfferLink {  get; set; } = string.Empty;  

        public Seniority Seniority { get; set; } 

        public List<Technology> Technologies { get; set; } = new();

    }
}
