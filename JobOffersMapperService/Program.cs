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
using Quartz;
using JobOffersMapperService.Config;
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
        services.AddScoped<IRawJobOfferService, RawJobOfferService>();
        services.AddScoped<IJobOffersBaseRepository, JobOffersBaseRepository>();
        services.AddScoped<IJobCreateMessageProducer, JobCreateMessageProducer>();
        services.AddScoped<IJobCheckIfOutdatedMessageProducer, JobCheckIfOutdatedMessageProducer>();
        services.AddHostedService<RawOffersConsumer>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      
        services.AddDbContext<OffersBaseContext>(options =>
        {
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")!);
        });

        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();

        services.ConfigureOptions<FindOutdatedJobOffersEventSenderJobConfig>();
    })
    .UseSerilog()
    .Build();

_host.RunAsync().Wait();

