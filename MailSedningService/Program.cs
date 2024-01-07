using MailSedningService.Consumer;
using MailSedningService.Interfaces;
using MailSedningService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


IHost _host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", false, true)
         .AddEnvironmentVariables();

    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient<IMailService, MailService>();
        services.AddHostedService<MailSendingEventConsumer>();
    })
    .Build();

_host.RunAsync().Wait();


