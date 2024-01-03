using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using UsersService.Entities;

namespace UsersService.DbContexts
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        public DbSet<JobOffer> JobOffers { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Technology> Technologies { get; set; } = null!;

        public DbSet<JobOfferUser> JobOfferUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobOffer>()
                  .HasMany(e => e.Users)
                  .WithMany(e => e.JobOffers)
                  .UsingEntity<JobOfferUser>();
        }
    }
}
