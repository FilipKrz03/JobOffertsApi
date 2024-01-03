using JobOffersApiCore.BaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersAndUsersDatabaseMigratorService.Entities
{
    public class Technology : BaseEntity
    {
        public string TechnologyName { get; set; } = string.Empty;

        public List<JobOffer> JobOffers { get; set; } = new();

        public List<User> Users {  get; set; } = new();  
        public List<TechnologyUser> TechnologyUsers { get; set;} = new();
    }
}
