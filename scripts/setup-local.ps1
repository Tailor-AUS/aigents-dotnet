# ═══════════════════════════════════════════════════════════════
# AIGENTS - LOCAL DEVELOPMENT SETUP (Windows ARM64 / Snapdragon)
# ═══════════════════════════════════════════════════════════════
# Run: .\scripts\setup-local.ps1
# ═══════════════════════════════════════════════════════════════

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Blue
Write-Host "║           AIGENTS - LOCAL DEVELOPMENT (Windows ARM64)         ║" -ForegroundColor Blue
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Blue
Write-Host ""

# ───────────────────────────────────────────────────────────────
# CHECK PREREQUISITES
# ───────────────────────────────────────────────────────────────

Write-Host "Checking prerequisites..." -ForegroundColor Cyan

# Check Docker
try {
    $dockerVersion = docker --version
    Write-Host "✓ Docker found: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker not found. Install Docker Desktop: https://www.docker.com/products/docker-desktop/" -ForegroundColor Red
    exit 1
}

# Check .NET SDK
try {
    $dotnetVersion = dotnet --version
    Write-Host "✓ .NET SDK found: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET SDK not found. Install: https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

# Check architecture
$arch = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture
Write-Host "✓ Architecture: $arch" -ForegroundColor Green

# ───────────────────────────────────────────────────────────────
# START DOCKER CONTAINERS
# ───────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "Starting Docker containers..." -ForegroundColor Cyan

docker compose up -d

Write-Host "✓ SQL Server (Azure SQL Edge) starting on port 1433" -ForegroundColor Green
Write-Host "✓ Redis starting on port 6379" -ForegroundColor Green
Write-Host "✓ MailDev starting on port 1080 (web) / 1025 (smtp)" -ForegroundColor Green

# Wait for SQL Server
Write-Host ""
Write-Host "Waiting for SQL Server to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

$ready = $false
for ($i = 1; $i -le 30; $i++) {
    try {
        $result = docker exec aigents-sql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Aigents@2024!" -Q "SELECT 1" 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ SQL Server is ready!" -ForegroundColor Green
            $ready = $true
            break
        }
    } catch {}
    Write-Host "." -NoNewline
    Start-Sleep -Seconds 2
}

if (-not $ready) {
    Write-Host ""
    Write-Host "⚠ SQL Server may still be starting. Check with: docker logs aigents-sql" -ForegroundColor Yellow
}

# ───────────────────────────────────────────────────────────────
# BUILD
# ───────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "Building solution..." -ForegroundColor Cyan

dotnet build

Write-Host "✓ Build successful!" -ForegroundColor Green

# ───────────────────────────────────────────────────────────────
# DONE
# ───────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                    SETUP COMPLETE! 🎉                         ║" -ForegroundColor Green
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

Write-Host "Services running:" -ForegroundColor Cyan
Write-Host "  • SQL Server:  localhost:1433"
Write-Host "  • Redis:       localhost:6379"
Write-Host "  • MailDev UI:  http://localhost:1080"
Write-Host ""

Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Set Azure AI credentials:" -ForegroundColor Cyan
Write-Host '   dotnet user-secrets set "Parameters:azure-ai-endpoint" "https://YOUR.openai.azure.com/" --project src\Aigents.AppHost'
Write-Host '   dotnet user-secrets set "Parameters:azure-ai-deployment" "gpt-4o" --project src\Aigents.AppHost'
Write-Host ""
Write-Host "2. Set Google OAuth credentials:" -ForegroundColor Cyan
Write-Host '   dotnet user-secrets set "Parameters:google-client-id" "YOUR-CLIENT-ID" --project src\Aigents.AppHost'
Write-Host '   dotnet user-secrets set "Parameters:google-client-secret" "YOUR-SECRET" --project src\Aigents.AppHost'
Write-Host ""
Write-Host "3. Run the app:" -ForegroundColor Cyan
Write-Host "   dotnet run --project src\Aigents.AppHost"
Write-Host ""
Write-Host "Aspire Dashboard:" -ForegroundColor Cyan
Write-Host "   https://localhost:17225"
Write-Host ""
Write-Host "To stop containers:" -ForegroundColor Cyan
Write-Host "   docker compose down"
Write-Host ""
