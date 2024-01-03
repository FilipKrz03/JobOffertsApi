using Microsoft.EntityFrameworkCore;
using OffersAndUsersDatabaseMigratorService.Entities;

namespace OffersAndUsersDatabaseMigratorService.DbContexts
{
    public class OffersApiDbContext : DbContext
    {
        public OffersApiDbContext(DbContextOptions<OffersApiDbContext> options) : base(options) { }

        public DbSet<JobOffer> JobOffers { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Technology> Technologies { get; set; } = null!;
    }
}
