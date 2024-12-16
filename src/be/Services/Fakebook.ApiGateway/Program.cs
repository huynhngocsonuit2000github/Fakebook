using Fakebook.ApiGateway.Middlewares;
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

builder.Services.AddHttpContextAccessor();

// Add Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Enable console logging for debug purposes
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddOcelot();

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAllOrigins");

// Add the custom middleware
app.UseMiddleware<CustomAuthorizationMiddleware>();

app.UseDeveloperExceptionPage(); // Show exception details in development
await app.UseOcelot();

app.Run();
