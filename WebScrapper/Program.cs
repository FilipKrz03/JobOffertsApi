// See https://aka.ms/new-console-template for more information
using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WebScrapperService.Consumer;
using WebScrapperService.Dto;
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
    services.AddSingleton<IScrapperService, PracujPlScrapper>();
    services.AddScoped<IRabbitMessageProducer , JobMessageProducer>();
    services.AddHostedService<OffersEventConsumer>();
    })
    .UseSerilog()
    .Build();

_host.RunAsync().Wait();

//var app = _host.Services.GetRequiredService<IScrapperService>();

//app.ScrapOfferts();