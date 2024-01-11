using JobOffersService.Consumer;
using JobOffersService.DbContexts;
using JobOffersService.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JobOfferServiceTests.Common
{
    public class IntegrationTestBase
    {

        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly HttpClient _httpClient;

        public IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // To avoid errors due to rabbitMqConnection
                        services.RemoveAll(typeof(IHostedService));

                        services.RemoveAll(typeof(JobOffersContext));
                        services.AddDbContext<JobOffersContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });

                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<JobOffersContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<JobOffersContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            _factory = appFactory;
            _httpClient = appFactory.CreateClient();
        }

        public void DbSeeder(Action<JobOffersContext> dbAction)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<JobOffersContext>();

                dbAction.Invoke(db);
            }
        }
        public void ClearDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<JobOffersContext>();
                db.Database.EnsureDeleted();
            }
        }
    }
}

