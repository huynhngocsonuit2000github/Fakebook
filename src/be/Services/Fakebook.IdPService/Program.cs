using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.IdPService.Data;
using Fakebook.IdPService.DataSeeding.Models;
using Fakebook.IdPService.Helpers;
using Fakebook.IdPService.Repositories;
using Fakebook.IdPService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ITokenHelper, TokenHelper>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
builder.Services.AddScoped<IUserUservice, UserUservice>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
