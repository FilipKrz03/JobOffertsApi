using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Enum;
using System.ComponentModel.DataAnnotations;

namespace UsersService.Entities
{
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
        public string OfferLink { get; set; } = string.Empty;

        [Required]
        public Seniority Seniority { get; set; }

        public int? EarningsFrom { get; set; }
        public int? EarningsTo { get; set; }

        public List<Technology> Technologies { get; set; } = new();

        public List<User> FollowingUsers { get; set; } = new();
    }
}
