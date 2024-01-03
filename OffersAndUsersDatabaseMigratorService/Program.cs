using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffersAndUsersDatabaseMigratorService.DbContexts;

/*
This service applies migrations to databases used by the JobOffersService and UsersService. 
Therefore, these services have different DbContexts, each containing only the entities they need.
However, all these services use the same database server.
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