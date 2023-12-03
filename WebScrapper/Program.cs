// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

IHost _host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {

    }).Build();