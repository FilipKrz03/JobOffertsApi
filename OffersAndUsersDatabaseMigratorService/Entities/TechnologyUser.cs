using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersAndUsersDatabaseMigratorService.Entities
{
    public class TechnologyUser
    {
        public Guid TechnologyId {  get; set; } 
        public Guid UserId {  get; set; }

        public Technology Technology { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
