using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

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

// Add environment-specific Ocelot configuration
var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile($"ocelot.{environment}.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot();


var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAllOrigins");

app.UseOcelot().Wait();

app.Run();
