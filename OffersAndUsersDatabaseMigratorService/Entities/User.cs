using JobOffersApiCore.BaseObjects;
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

        public List<JobOffer> FollowingJobOffers { get; set; } =
            new List<JobOffer>();
    }
}
