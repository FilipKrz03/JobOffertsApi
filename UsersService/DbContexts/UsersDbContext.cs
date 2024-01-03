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
    }
}
