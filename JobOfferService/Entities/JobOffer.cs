using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Enum;

namespace JobOffersService.Entities
{
    // Many to many realationship with Technology Entitie 
    public class JobOffer : BaseEntity
    {
        
        public string OfferTitle { get; set; }  
        public string OfferCompany {  get; set; }   
        public string Localization {  get; set; }
        public string WorkMode {  get; set; }   
        public Seniority Seniority { get; set; }

        public List<Technology> Technologies { get; set; } = new();

        public JobOffer(string offerTitle , string offerCompany , string localization , 
            string workMode , Seniority seniority)
        {
            OfferTitle = offerTitle;
            OfferCompany = offerCompany;
            Localization = localization;
            WorkMode = workMode;
            Seniority = seniority;
        }
    }
}
