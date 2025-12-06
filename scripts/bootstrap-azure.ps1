# ═══════════════════════════════════════════════════════════════
# AIGENTS - AZURE BOOTSTRAP (Windows PowerShell)
# ═══════════════════════════════════════════════════════════════
# Run: .\scripts\bootstrap-azure.ps1
# ═══════════════════════════════════════════════════════════════

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Blue
Write-Host "║           AIGENTS - AZURE BOOTSTRAP                           ║" -ForegroundColor Blue
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Blue
Write-Host ""

# ───────────────────────────────────────────────────────────────
# CHECK PREREQUISITES
# ───────────────────────────────────────────────────────────────

Write-Host "Checking prerequisites..." -ForegroundColor Cyan

try {
    $azVersion = az --version | Select-Object -First 1
    Write-Host "✓ Azure CLI found" -ForegroundColor Green
} catch {
    Write-Host "❌ Azure CLI not found. Install: https://docs.microsoft.com/cli/azure/install-azure-cli" -ForegroundColor Red
    exit 1
}

# ───────────────────────────────────────────────────────────────
# LOGIN
# ───────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "Logging in to Azure..." -ForegroundColor Cyan

$account = az account show 2>$null | ConvertFrom-Json
if (-not $account) {
    az login
    $account = az account show | ConvertFrom-Json
}

$subscriptionId = $account.id
$subscriptionName = $account.name
Write-Host "✓ Logged in to: $subscriptionName" -ForegroundColor Green

# ───────────────────────────────────────────────────────────────
# DEPLOY BOOTSTRAP
# ───────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "Deploying bootstrap infrastructure..." -ForegroundColor Cyan

$deploymentOutput = az deployment sub create `
    --location australiaeast `
    --template-file infra/bootstrap.bicep `
    --query "{rg:properties.outputs.resourceGroupName.value, acr:properties.outputs.acrName.value, acrServer:properties.outputs.acrLoginServer.value}" `
    -o json | ConvertFrom-Json

$rgName = $deploymentOutput.rg
$acrName = $deploymentOutput.acr
$acrServer = $deploymentOutput.acrServer

Write-Host "✓ Resource Group: $rgName" -ForegroundColor Green
Write-Host "✓ Container Registry: $acrName" -ForegroundColor Green

# ───────────────────────────────────────────────────────────────
# CREATE SERVICE PRINCIPAL
# ───────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "Creating Service Principal for GitHub Actions..." -ForegroundColor Cyan

$spOutput = az ad sp create-for-rbac `
    --name "aigents-github-actions" `
    --role contributor `
    --scopes "/subscriptions/$subscriptionId/resourceGroups/$rgName" `
    --sdk-auth

Write-Host "✓ Service Principal created" -ForegroundColor Green

# ───────────────────────────────────────────────────────────────
# OUTPUT
# ───────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                    BOOTSTRAP COMPLETE! 🎉                     ║" -ForegroundColor Green
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

Write-Host "Now add these GitHub Secrets:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. AZURE_CREDENTIALS:" -ForegroundColor Cyan
Write-Host $spOutput
Write-Host ""
Write-Host "2. Update .github/workflows/cd.yml:" -ForegroundColor Cyan
Write-Host "   ACR_NAME: $acrName"
Write-Host ""
Write-Host "3. Don't forget to add:" -ForegroundColor Cyan
Write-Host "   - GOOGLE_CLIENT_ID"
Write-Host "   - GOOGLE_CLIENT_SECRET"
Write-Host ""
Write-Host "Note: Azure AI Foundry (GPT-4o) is deployed automatically - no API key needed!" -ForegroundColor Yellow
Write-Host ""
Write-Host "Next step:" -ForegroundColor Cyan
Write-Host "   Push to main branch and watch the magic happen! ✨"
Write-Host ""
