using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
    .ConfigureServices(services =>
    {
    
    })
    .UseSerilog()
    .Build();

