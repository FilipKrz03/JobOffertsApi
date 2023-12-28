using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using UsersService.DbContexts;
using UsersService.Interfaces;
using UsersService.Repository;
using UsersService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddTransient<UsersService.Middleware.ExceptionHandlerMiddleware>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<UsersService.Interfaces.IAuthenticationService, UsersService.Services.AuthenticationService>();
builder.Services.AddTransient<IUserService, UsersService.Services.UsersService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

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