using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using JobOffersApiCore.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Quartz;
using UsersService.Config;
using UsersService.DbContexts;
using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;
using UsersService.Producer;
using UsersService.Repository;
using UsersService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddTransient<UsersService.Middleware.ExceptionHandlerMiddleware>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddTransient<IJobOfferRepository, JobOfferRepository>();
builder.Services.AddTransient<UsersService.Interfaces.ServicesInterfaces.IAuthenticationService, UsersService.Services.AuthenticationService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IFollowedJobOfferService, FollowedJobOfferService>();
builder.Services.AddTransient<IJobOfferUserJoinRepository, JobOfferUserJoinRepository>();
builder.Services.AddTransient<ISubscribedTechnologyService, SubscribedTechnologyService>();
builder.Services.AddTransient<ITechnologyRepository, TechnologyRepository>();
builder.Services.AddTransient<ITechnologyUserJoinRepository, TechnologyUserJoinRepository>();
builder.Services.AddTransient<IUserAnalyzeService, UserAnalyzeService>();
builder.Services.AddTransient<IRabbitMessageProducer, SendEmailWithRecomendedOffersToUsersGroupMessageProducer>();
builder.Services.AddSingleton<IMailContentCreatorService, MailContentCreatorService>();

builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddHttpClient<IJwtProvider, JwtProvider>();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOptions =>
    {
        jwtOptions.Authority = Environment.GetEnvironmentVariable("ValidIssuer");
        jwtOptions.Audience = Environment.GetEnvironmentVariable("Audience");
        jwtOptions.TokenValidationParameters.ValidIssuer = Environment.GetEnvironmentVariable("ValidIssuer");
    });

builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
});

builder.Services.AddQuartzHostedService();

builder.Services.ConfigureOptions<UserAnalyzeBackgroundJobConfig>();

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("firebase.json")
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<UsersService.Middleware.ExceptionHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
