using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffersAndUsersDatabaseMigratorService.DbContexts;

/*
This service applies migrations to databases used by the JobOffersService and UsersService. 
Therefore, these services have different DbContexts, each containing only the entities they need.
However, both services use the same database server.
*/

IHost _host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", false, true)
         .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<OffersApiDbContext>(options =>
        {
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")!);
        });
    })
    .Build();

using (var scope = _host.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<OffersApiDbContext>();

    try
    {
        var databaseCreator = dbContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        if (databaseCreator != null)
        {
            if (!databaseCreator.Exists()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }
}