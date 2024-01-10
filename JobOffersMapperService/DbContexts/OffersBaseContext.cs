using JobOffersMapperService.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.DbContexts
{
    public class OffersBaseContext : DbContext
    {
        public OffersBaseContext(
            DbContextOptions<OffersBaseContext> options ,
            ILogger<OffersBaseContext> logger
            ):base(options)
        {
            try
            {
                // Database Creator needed for docker only 
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured on OfferBaseContext {ex}", ex);
            }
        }

        public DbSet<JobOfferBase> BaseJobOffer { get; set; } = null!;

    }
}
