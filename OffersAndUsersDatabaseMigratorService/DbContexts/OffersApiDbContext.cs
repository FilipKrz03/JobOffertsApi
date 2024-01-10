using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OffersAndUsersDatabaseMigratorService.Entities;

namespace OffersAndUsersDatabaseMigratorService.DbContexts
{
    public class OffersApiDbContext : DbContext
    {
        public OffersApiDbContext(DbContextOptions<OffersApiDbContext> options) : base(options) { }

        public DbSet<JobOffer> JobOffers { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Technology> Technologies { get; set; } = null!;

        public DbSet<JobOfferUser> JobOfferUsers { get; set; } = null!;

        public DbSet<TechnologyUser> TechnologyUsers { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobOffer>()
                  .HasMany(e => e.Users)
                  .WithMany(e => e.JobOffers)
                  .UsingEntity<JobOfferUser>();

            modelBuilder.Entity<Technology>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Technologies)
                .UsingEntity<TechnologyUser>();
        }
    }
}
