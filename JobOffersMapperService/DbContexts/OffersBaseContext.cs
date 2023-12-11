using JobOffersMapperService.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.DbContexts
{
    public class OffersBaseContext : DbContext
    {
        public OffersBaseContext(DbContextOptions<OffersBaseContext> options):base(options) { }

        public DbSet<JobOfferBase> BaseJobOffer { get; set; } = null!;

    }
}
