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
        services.AddHostedService<OffersEventConsumer>();
        //services.AddScoped<IScrapperService, PracujPlScrapper>();
        services.AddScoped<IWebDriverFactory , WebDriverFactory>();
        services.AddScoped<IScrapperService, TheProtocolScrapper>();
        services.AddScoped<IRabbitMessageProducer, JobHandleMessageProducer>();
    })
    .UseSerilog()
    .Build();

//_host.RunAsync().Wait();

var app = _host.Services.GetRequiredService<IScrapperService>();

app.ScrapOfferts(false);