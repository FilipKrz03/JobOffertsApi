// See https://aka.ms/new-console-template for more information
using JobOffersApiCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WebScrapperService.Consumer;
using WebScrapperService.Interfaces;
using WebScrapperService.Producer;
using WebScrapperService.Services;

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
    .ConfigureServices(services =>
    {
        services.AddScoped<IOffersService, OffersService>();
        services.AddHostedService<OffersCreateEventConsumer>();
        services.AddHostedService<OffersUpdateEventConsumer>();
        services.AddHostedService<OfferCheckIfOutdatedEventConsumer>();
        services.AddScoped<IScrapperService, PracujPlScrapperService>();
        services.AddScoped<IWebDriverFactory , WebDriverFactory>();
        services.AddScoped<IScrapperService, TheProtocolScrapperService>();
        services.AddScoped<IJobHandleMessageProducer, JobHandleMessageProducer>();
        services.AddScoped<IJobDeleteMessageProducer , JobDeleteMessageProducer>();
        services.AddSingleton<IJobTopicalityCheckerService , JobTopicalityCheckerService>();
    })
    .UseSerilog()
    .Build();

_host.RunAsync().Wait();
