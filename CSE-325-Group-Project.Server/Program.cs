using CSE325Project.Server.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var envFile = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(envFile))
{
    foreach (var line in File.ReadAllLines(envFile))
    {
        var parts = line.Split('=', 2);
        var key = parts[0].Trim();
        if (parts.Length == 2 && !key.Contains(' ') && !key.Contains('"'))
        {
            Environment.SetEnvironmentVariable(key, parts[1].Trim());
        }
    }
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Client", policy =>
        policy.WithOrigins("http://localhost:5059")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var connectionString = GetConnectionString(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    GetValue("ConnectionStrings__DefaultConnection", "CONNECTION_STRING", "connection"));

if (string.IsNullOrWhiteSpace(connectionString))
{
    var hostValue = GetValue("DB_HOST", "host");

    if (LooksLikeConnectionString(hostValue))
    {
        connectionString = hostValue;
    }
    else
    {
        var port = GetValue("DB_PORT", "port");
        var database = GetValue("DB_NAME", "database");
        var username = GetValue("DB_USER", "username");
        var password = GetValue("DB_PASSWORD", "password");

        connectionString =
            $"Host={hostValue};" +
            $"Port={port};" +
            $"Database={database};" +
            $"Username={username};" +
            $"Password={password};" +
            "SSL Mode=Require;Trust Server Certificate=true";
    }
}

builder.Services.AddDbContext<StudySpotContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.Title = "My API";
    });
}

app.UseCors("Client");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.MapGet("/", () => "Hello World!");
app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

static string? GetValue(params string[] names)
{
    foreach (var name in names)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }
    }

    return null;
}

static string? GetConnectionString(params string?[] values)
{
    foreach (var value in values)
    {
        var databaseUrl = GetConnectionStringFromUrl(value);
        if (!string.IsNullOrWhiteSpace(databaseUrl))
        {
            return databaseUrl;
        }

        if (LooksLikeConnectionString(value))
        {
            return value!.Trim();
        }
    }

    return null;
}

static string? GetConnectionStringFromUrl(string? value)
{
    if (string.IsNullOrWhiteSpace(value)
        || !Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri)
        || (uri.Scheme != "postgres" && uri.Scheme != "postgresql"))
    {
        return null;
    }

    var userInfo = uri.UserInfo.Split(':', 2);
    var username = Uri.UnescapeDataString(userInfo[0]);
    var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";

    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port > 0 ? uri.Port : 5432,
        Database = uri.AbsolutePath.TrimStart('/'),
        Username = username,
        Password = password,
        SslMode = SslMode.Require
    };

    return builder.ConnectionString;
}

static bool LooksLikeConnectionString(string? value)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return false;
    }

    return value.Contains("Host=", StringComparison.OrdinalIgnoreCase)
        || value.Contains("Server=", StringComparison.OrdinalIgnoreCase);
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
