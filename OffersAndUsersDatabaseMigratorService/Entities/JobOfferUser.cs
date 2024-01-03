using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersAndUsersDatabaseMigratorService.Entities
{
    public class JobOfferUser
    {
        public Guid JobOfferId { get; set; }
        public Guid UserId { get; set; }

        public JobOffer JobOffer { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
