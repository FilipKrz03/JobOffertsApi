// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WebScrapperService.Interfaces;
using WebScrapperService.Services;

Console.WriteLine("Hello, World!");

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
        services.AddSingleton<IScrapperService, TheProtocolScrapper>();
    })
    .UseSerilog()
    .Build();

var app = _host.Services.GetRequiredService<IScrapperService>();

app.ScrapOfferts();