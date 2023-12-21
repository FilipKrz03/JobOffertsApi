using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Enum;
using System.ComponentModel.DataAnnotations;

namespace JobOffersService.Entities
{
    // Many to many realationship with Technology Entitie 
    public class JobOffer : BaseEntity
    {
        [Required]
        public string OfferTitle { get; set; } = string.Empty;

        [Required]
        public string OfferCompany { get; set; } = string.Empty;

        [Required]
        public string Localization { get; set; } = string.Empty;

        [Required]
        public string WorkMode { get; set; } = string.Empty;

        [Required]
        public string OfferLink {  get; set; } = string.Empty;

        [Required]
        public Seniority Seniority { get; set; }

        public int? EarningsFrom {  get; set; } 
        public int? EarningsTo {  get; set; }   

        public List<Technology> Technologies { get; set; } = new();

    }
}
