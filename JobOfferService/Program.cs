using JobOffersApiCore.Interfaces;
using JobOfferService.Producer;
using JobOfferService.Services;
using JobOffersService.Consumer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRabbitMessageProducer, ScrapperMessageProducer>();
builder.Services.AddHostedService<ScrapperEventManagerService>();
builder.Services.AddHostedService<OffersToCreateConsumer>();

Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Console()
      .CreateLogger();
        

builder.Host.UseSerilog();

var app = builder
    .Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
