// ═══════════════════════════════════════════════════════════════
// AIGENTS WEB - BLAZOR FRONTEND
// ═══════════════════════════════════════════════════════════════

using Aigents.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────────────────────────
// ASPIRE SERVICE DEFAULTS
// ───────────────────────────────────────────────────────────────

builder.AddServiceDefaults();

// ───────────────────────────────────────────────────────────────
// BLAZOR
// ───────────────────────────────────────────────────────────────

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ───────────────────────────────────────────────────────────────
// OUTPUT CACHING (REDIS)
// ───────────────────────────────────────────────────────────────

builder.AddRedisOutputCache("redis");

// ───────────────────────────────────────────────────────────────
// API CLIENT
// ───────────────────────────────────────────────────────────────

builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri("https+http://api");
});

// ───────────────────────────────────────────────────────────────
// AUTHENTICATION (optional - requires Google credentials)
// ───────────────────────────────────────────────────────────────

var googleClientId = builder.Configuration["Google:ClientId"];
var googleClientSecret = builder.Configuration["Google:ClientSecret"];

if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "Google";
    })
    .AddCookie("Cookies")
    .AddGoogle("Google", options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.CallbackPath = "/signin-google";
    });
    
    builder.Services.AddCascadingAuthenticationState();
}

var app = builder.Build();

// ───────────────────────────────────────────────────────────────
// MIDDLEWARE
// ───────────────────────────────────────────────────────────────

// Note: HTTPS redirect disabled for local Aspire development
// Enable in production with proper HTTPS configuration
app.UseStaticFiles();
app.UseAntiforgery();
app.UseOutputCache();

// ───────────────────────────────────────────────────────────────
// ENDPOINTS
// ───────────────────────────────────────────────────────────────

app.MapDefaultEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
