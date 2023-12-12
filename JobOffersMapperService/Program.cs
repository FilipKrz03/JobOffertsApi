using JobOffersApiCore.Interfaces;
using JobOffersMapperService.Consumer;
using JobOffersMapperService.DbContexts;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Producer;
using JobOffersMapperService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Serilog;

IHost _host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", false, true)
         .AddEnvironmentVariables();

        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Build())
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IRawOfferService, RawOfferService>();
        services.AddScoped<IOffersBaseRepository, OffersBaseRepository>();
        services.AddScoped<IRabbitMessageProducer, JobCreateMessageProducer>();
        services.AddHostedService<RawOffersConsumer>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      
        services.AddDbContext<OffersBaseContext>(options =>
        {
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")!);
        });
    })
    .UseSerilog()
    .Build();

_host.RunAsync().Wait();

