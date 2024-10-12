using Fakebook.UserService.Data;
using Microsoft.EntityFrameworkCore;
using Fakebook.UserService.Repositories;
using Fakebook.UserService.Services;
using Fakebook.UserService.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Fakebook.UserService.HttpRequestHandling;
using Fakebook.UserService.Middlewares;
using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.DataAccessLayer.Implementaions;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Settings configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

// Add Authentication services and JWT Bearer options
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthorization();

// Register your TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

// Database Setup - checking
var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"Environment: {environment}");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String Used: {connectionString}");

// Database Setup
builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(sp.GetService<ServiceContext>()!));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserUservice, UserUservice>();


builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor
builder.Services.AddScoped<IUserContextService, UserContextService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply pending migrations to ensure the data is setup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ServiceContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<UserContextMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
