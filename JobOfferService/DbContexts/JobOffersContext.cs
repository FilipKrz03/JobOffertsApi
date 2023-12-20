using JobOffersService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobOffersService.DbContexts
{
    public class JobOffersContext : DbContext
    {
        public JobOffersContext(DbContextOptions<JobOffersContext> options, ILogger<JobOffersContext> logger) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured in job offers context {ex}", ex);
            }
        }

        public DbSet<Technology> Technologies { get; set; } = null!;

        public DbSet<JobOffer> JobOffers { get; set; } = null!;
    }
}
