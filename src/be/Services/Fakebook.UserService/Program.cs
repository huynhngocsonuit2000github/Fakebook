using Fakebook.UserService.Data;
using Microsoft.EntityFrameworkCore;
using Fakebook.UserService.Repositories;
using Fakebook.UserService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.UserService.DataSeeding.Models;
using Fakebook.MessageQueueHandler.Consumer;
using Fakebook.DataAccessLayer.HttpRequestHandling;
using Fakebook.DataAccessLayer.Middlewares;
using Fakebook.DataAccessLayer.Authentication.Models;

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
// Retrieve the connection string and the password separately
var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
var password = builder.Configuration.GetSection("ConnectionStringsCredential")["DefaultConnection"];
Console.WriteLine($"Password: {password}");

// Replace the $Password placeholder in the connection string with the actual password
var connectionString = connectionStringTemplate.Replace("$Password", password);
Console.WriteLine($"Connection string: {connectionString}");

// Print the connection string to verify the password injection
Console.WriteLine($"Connection String Used: {connectionString}");

// Database Setup 
builder.Services.AddDbContext<ServiceContext>(options =>
{
    options.UseMySQL(connectionString, mySqlOptions =>
        mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)
    );
});


builder.Services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(sp.GetService<ServiceContext>()!));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor
builder.Services.AddScoped<IUserContextService, UserContextService>();

// Add a hosted service to run the consumer
builder.Services.AddHostedService<RabbitMQConsumerHostedService>();

// Update Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
        });
});


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

    // Seeding data
    new DbSeeder(dbContext).SeedData();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseMiddleware<UserContextMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();