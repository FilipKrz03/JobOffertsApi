using JobOffersApiCore.Interfaces;
using JobOfferService.Producer;
using JobOffersService.Config;
using JobOffersService.Consumer;
using JobOffersService.DbContexts;
using JobOffersService.Interfaces;
using JobOffersService.Middleware;
using JobOffersService.Repositories;
using JobOffersService.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();
builder.Services.AddSingleton<IRabbitMessageProducer, ScrapperMessageProducer>();

builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();
builder.Services.AddScoped<IProcessedOfferService, JobOffersService.Services.ProcessedJobOfferService>();
builder.Services.AddScoped<IJobOfferRepository, JobOffersRepository>();
builder.Services.AddScoped<IJobOfferService, JobOffersService.Services.JobOfferService>();
builder.Services.AddScoped<ITechnologyService, TechnologyService>();
builder.Services.AddHostedService<OffersToCreateConsumer>();
builder.Services.AddHostedService<OffersToDeleteConsumer>();

builder.Services.AddDbContext<JobOffersContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
});

builder.Services.AddQuartzHostedService();

builder.Services.ConfigureOptions<ScrapperEventManagerJobConfig>();

Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Console()
      .CreateLogger();

builder.Host.UseSerilog();

var app = builder
    .Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }

