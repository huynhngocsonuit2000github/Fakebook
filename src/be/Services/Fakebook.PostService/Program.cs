using System.Text;
using Fakebook.DataAccessLayer.Authentication.Models;
using Fakebook.DataAccessLayer.HttpRequestHandling;
using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.DataAccessLayer.Middlewares;
using Fakebook.PostService.Data;
using Fakebook.PostService.DataSeeding.Models;
using Fakebook.PostService.Repositories;
using Fakebook.PostService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
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

builder.Services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(sp.GetService<ServiceContext>()!));
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();


builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor
builder.Services.AddScoped<IUserContextService, UserContextService>();

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
