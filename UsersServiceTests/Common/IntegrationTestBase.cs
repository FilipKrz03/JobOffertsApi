
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UsersService.DbContexts;

namespace UsersServiceTests.Common
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
                    builder.ConfigureTestServices(services =>
                    {
                        // To avoid errors due to rabbitMqConnection
                        services.RemoveAll(typeof(IHostedService));

                        services.RemoveAll(typeof(UsersDbContext));

                        services.AddDbContext<UsersDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });

                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UsersDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<UsersDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });

                        services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                            .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
                    });
                });
            _factory = appFactory;
            _httpClient = appFactory.CreateClient();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        }

        public void DbSeeder(Action<UsersDbContext> dbAction)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UsersDbContext>();

                dbAction.Invoke(db);
            }
        }

        public void ClearDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UsersDbContext>();
                db.Database.EnsureDeleted();
            }
        }

        public void SetJwtUserEntitieId(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Add("userEntiteId", id.ToString());
        }

        public UsersDbContext DbContextGetter()
        {
            var scope = _factory.Services.CreateScope();
            
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<UsersDbContext>();

            return db;
        }
    }
}
