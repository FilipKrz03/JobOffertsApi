// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebScrapperService.Interfaces;
using WebScrapperService.Services;

Console.WriteLine("Hello, World!");

IHost _host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IScrapperService, PracujPlScrapper>();
    }).Build();

var app = _host.Services.GetRequiredService<IScrapperService>();

app.ScrapOfferts();