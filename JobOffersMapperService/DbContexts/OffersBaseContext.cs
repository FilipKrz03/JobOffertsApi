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
        public OffersBaseContext(DbContextOptions<OffersBaseContext> options , ILogger<OffersBaseContext> logger):base(options)
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

        public DbSet<JobOfferBase> BaseJobOffer { get; set; } = null!;

    }
}
