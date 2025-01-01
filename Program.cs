using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AppAny.HotChocolate.FluentValidation;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using lego_api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var sqlUrl = Environment.GetEnvironmentVariable("SQL_URL");
var sqlPassword = Environment.GetEnvironmentVariable("SQL_PASSWORD");

if (string.IsNullOrEmpty(sqlUrl) || string.IsNullOrEmpty(sqlPassword))
{
    Console.WriteLine("Error: Missing environment variables required for database connection");
    Environment.Exit(1);
}

var accessTokenSecret = Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET");
var refreshTokenSecret = Environment.GetEnvironmentVariable("REFRESH_TOKEN_SECRET");

if (string.IsNullOrEmpty(accessTokenSecret) || string.IsNullOrEmpty(refreshTokenSecret))
{
    Console.WriteLine("Error: Missing environment variables required for authentication");
    Environment.Exit(1);
}

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
    .AddPooledDbContextFactory<AppDbContext>(options =>
        options.UseSqlServer($"Server={sqlUrl};Database=lego;User Id=lego-api;Password={sqlPassword};TrustServerCertificate=True;"))
    .AddScoped<BrickService>()
    .AddScoped<UserService>()
    .AddScoped<AuthService>()
    .AddValidatorsFromAssemblyContaining<CreateBrickInputValidator>()
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(accessTokenSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFluentValidation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var dbContext = contextFactory.CreateDbContext();
    dbContext.Database.Migrate();
    await AppDbContext.SeedData(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGraphQL();
app.Run();