// ═══════════════════════════════════════════════════════════════
// AIGENTS - ASPIRE APP HOST
// ═══════════════════════════════════════════════════════════════
// Orchestrates all services with distributed tracing, health checks,
// and service discovery.
// ═══════════════════════════════════════════════════════════════

var builder = DistributedApplication.CreateBuilder(args);

// ───────────────────────────────────────────────────────────────
// INFRASTRUCTURE
// ───────────────────────────────────────────────────────────────

var redis = builder.AddRedis("redis")
    .WithDataVolume("aigents-redis-data");

var sql = builder.AddSqlServer("sql")
    .WithDataVolume("aigents-sql-data");

var sqlDatabase = sql.AddDatabase("aigentsdb");

// ───────────────────────────────────────────────────────────────
// SECRETS (from environment/config)
// ───────────────────────────────────────────────────────────────

var azureAiEndpoint = builder.AddParameter("azure-ai-endpoint", secret: false);
var azureAiDeployment = builder.AddParameter("azure-ai-deployment", secret: false);
var googleClientId = builder.AddParameter("google-client-id", secret: true);
var googleClientSecret = builder.AddParameter("google-client-secret", secret: true);

// ───────────────────────────────────────────────────────────────
// API SERVICE
// ───────────────────────────────────────────────────────────────

var api = builder.AddProject<Projects.Aigents_Api>("api")
    .WithReference(redis)
    .WithReference(sqlDatabase)
    .WaitFor(redis)
    .WaitFor(sqlDatabase)
    .WithEnvironment("AzureAI__Endpoint", azureAiEndpoint)
    .WithEnvironment("AzureAI__DeploymentName", azureAiDeployment)
    .WithEnvironment("Google__ClientId", googleClientId)
    .WithEnvironment("Google__ClientSecret", googleClientSecret)
    .WithHttpEndpoint(port: 5001, name: "http")
    .WithExternalHttpEndpoints();

// ───────────────────────────────────────────────────────────────
// WEB FRONTEND
// ───────────────────────────────────────────────────────────────

var web = builder.AddProject<Projects.Aigents_Web>("web")
    .WithReference(api)
    .WithReference(redis)
    .WaitFor(api)
    .WaitFor(redis)
    .WithEnvironment("Google__ClientId", googleClientId)
    .WithEnvironment("Google__ClientSecret", googleClientSecret)
    .WithExternalHttpEndpoints();

// ───────────────────────────────────────────────────────────────
// BUILD & RUN
// ───────────────────────────────────────────────────────────────

await builder.Build().RunAsync();
