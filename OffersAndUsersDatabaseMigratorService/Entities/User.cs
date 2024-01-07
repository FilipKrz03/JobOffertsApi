using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersAndUsersDatabaseMigratorService.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;

        public Seniority DesiredSeniority { get; set; } 

        public List<JobOffer> JobOffers { get; set; } =
           new List<JobOffer>();
        public List<JobOfferUser> JobOfferUsers { get; set; } = new();

        public List<Technology> Technologies { get; set; } = new();
        public List<TechnologyUser> TechnologyUsers { get; set; } = new();
    }
}
