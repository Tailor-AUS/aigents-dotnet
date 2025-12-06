// ═══════════════════════════════════════════════════════════════
// AIGENTS API - VERTICAL SLICE ARCHITECTURE
// ═══════════════════════════════════════════════════════════════
// Each feature is self-contained with its own endpoint, handler,
// and validation. Uses MediatR for CQRS and Carter for endpoints.
// ═══════════════════════════════════════════════════════════════

using Aigents.Api.Common;
using Aigents.Infrastructure.Data;
using Aigents.Infrastructure.Services.AI;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────────────────────────
// ASPIRE SERVICE DEFAULTS
// ───────────────────────────────────────────────────────────────

builder.AddServiceDefaults();

// ───────────────────────────────────────────────────────────────
// DATABASE
// ───────────────────────────────────────────────────────────────

builder.AddSqlServerDbContext<AigentsDbContext>("aigentsdb");

// ───────────────────────────────────────────────────────────────
// REDIS CACHE
// ───────────────────────────────────────────────────────────────

builder.AddRedisDistributedCache("redis");

// ───────────────────────────────────────────────────────────────
// AI SERVICE (Azure AI Foundry)
// ───────────────────────────────────────────────────────────────

builder.Services.Configure<AzureAiOptions>(
    builder.Configuration.GetSection(AzureAiOptions.SectionName));
builder.Services.AddScoped<IAiService, AzureAiService>();

// ───────────────────────────────────────────────────────────────
// MEDIATR + VALIDATION
// ───────────────────────────────────────────────────────────────

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// ───────────────────────────────────────────────────────────────
// CARTER (ENDPOINTS)
// ───────────────────────────────────────────────────────────────

builder.Services.AddCarter();

// ───────────────────────────────────────────────────────────────
// AUTHENTICATION (optional - requires JWT configuration)
// ───────────────────────────────────────────────────────────────

// Skip auth for local development if not configured
builder.Services.AddAuthorization();

// ───────────────────────────────────────────────────────────────
// CORS
// ───────────────────────────────────────────────────────────────

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ───────────────────────────────────────────────────────────────
// SWAGGER
// ───────────────────────────────────────────────────────────────

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Aigents API", Version = "v1" });
});

var app = builder.Build();

// ───────────────────────────────────────────────────────────────
// MIDDLEWARE
// ───────────────────────────────────────────────────────────────

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();

// ───────────────────────────────────────────────────────────────
// ENDPOINTS
// ───────────────────────────────────────────────────────────────

app.MapDefaultEndpoints(); // Health checks
app.MapCarter(); // Feature endpoints

// ───────────────────────────────────────────────────────────────
// DATABASE INITIALIZATION (with retry for container startup)
// ───────────────────────────────────────────────────────────────

var maxRetries = 10;
var retryDelay = TimeSpan.FromSeconds(5);

for (int i = 0; i < maxRetries; i++)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AigentsDbContext>();
        
        // Apply migrations
        await db.Database.MigrateAsync();
        Console.WriteLine("✅ Database migrations applied successfully");
        break;
    }
    catch (Exception ex) when (i < maxRetries - 1)
    {
        Console.WriteLine($"⏳ Database not ready (attempt {i + 1}/{maxRetries}): {ex.Message}");
        await Task.Delay(retryDelay);
    }
}

app.Run();
