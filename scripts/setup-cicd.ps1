# ═══════════════════════════════════════════════════════════════
# AIGENTS CI/CD SETUP SCRIPT (PowerShell)
# Sets up Azure resources and GitHub secrets
# ═══════════════════════════════════════════════════════════════

param(
    [string]$ResourceGroup = "aigents-rg",
    [string]$Location = "australiaeast",
    [string]$AcrName = "aigentsacr",
    [string]$GitHubRepo = "aigents-dotnet"
)

Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  AIGENTS CI/CD SETUP" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

# ─────────────────────────────────────────────────────────────
# CHECK PREREQUISITES
# ─────────────────────────────────────────────────────────────

Write-Host "📋 Checking prerequisites..." -ForegroundColor Yellow

# Check Azure CLI
if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Azure CLI not found. Install from: https://docs.microsoft.com/cli/azure/install-azure-cli" -ForegroundColor Red
    exit 1
}

# Check GitHub CLI
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Host "❌ GitHub CLI not found. Install from: https://cli.github.com/" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Prerequisites OK" -ForegroundColor Green
Write-Host ""

# ─────────────────────────────────────────────────────────────
# LOGIN TO AZURE
# ─────────────────────────────────────────────────────────────

Write-Host "🔐 Checking Azure login..." -ForegroundColor Yellow
$azAccount = az account show 2>$null | ConvertFrom-Json
if (-not $azAccount) {
    Write-Host "   Logging in to Azure..." -ForegroundColor Gray
    az login
    $azAccount = az account show | ConvertFrom-Json
}
Write-Host "✅ Logged in as: $($azAccount.user.name)" -ForegroundColor Green
Write-Host "   Subscription: $($azAccount.name)" -ForegroundColor Gray
$SubscriptionId = $azAccount.id
Write-Host ""

# ─────────────────────────────────────────────────────────────
# LOGIN TO GITHUB
# ─────────────────────────────────────────────────────────────

Write-Host "🔐 Checking GitHub login..." -ForegroundColor Yellow
$ghStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "   Logging in to GitHub..." -ForegroundColor Gray
    gh auth login
}
Write-Host "✅ GitHub authenticated" -ForegroundColor Green
Write-Host ""

# Get GitHub repo info
$repoInfo = gh repo view --json nameWithOwner 2>$null | ConvertFrom-Json
if (-not $repoInfo) {
    Write-Host "❌ Not in a GitHub repository. Please run from your repo directory." -ForegroundColor Red
    exit 1
}
$GitHubRepoFull = $repoInfo.nameWithOwner
Write-Host "📁 Repository: $GitHubRepoFull" -ForegroundColor Cyan
Write-Host ""

# ─────────────────────────────────────────────────────────────
# CREATE AZURE RESOURCE GROUP
# ─────────────────────────────────────────────────────────────

Write-Host "☁️  Creating Azure Resource Group..." -ForegroundColor Yellow
az group create --name $ResourceGroup --location $Location --output none 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Resource Group: $ResourceGroup" -ForegroundColor Green
} else {
    Write-Host "   Resource Group already exists" -ForegroundColor Gray
}
Write-Host ""

# ─────────────────────────────────────────────────────────────
# CREATE AZURE CONTAINER REGISTRY
# ─────────────────────────────────────────────────────────────

Write-Host "📦 Creating Azure Container Registry..." -ForegroundColor Yellow
$acrExists = az acr show --name $AcrName --resource-group $ResourceGroup 2>$null
if (-not $acrExists) {
    az acr create `
        --name $AcrName `
        --resource-group $ResourceGroup `
        --sku Basic `
        --admin-enabled true `
        --output none
    Write-Host "✅ ACR Created: $AcrName.azurecr.io" -ForegroundColor Green
} else {
    Write-Host "   ACR already exists: $AcrName.azurecr.io" -ForegroundColor Gray
}
Write-Host ""

# ─────────────────────────────────────────────────────────────
# CREATE SERVICE PRINCIPAL
# ─────────────────────────────────────────────────────────────

Write-Host "🔑 Creating Service Principal for GitHub Actions..." -ForegroundColor Yellow
$spName = "aigents-github-actions"

# Check if SP already exists
$existingSp = az ad sp list --display-name $spName --query "[0]" 2>$null | ConvertFrom-Json
if ($existingSp) {
    Write-Host "   Deleting existing Service Principal..." -ForegroundColor Gray
    az ad sp delete --id $existingSp.appId 2>$null
}

# Create new Service Principal
$spCredentials = az ad sp create-for-rbac `
    --name $spName `
    --role contributor `
    --scopes "/subscriptions/$SubscriptionId/resourceGroups/$ResourceGroup" `
    --sdk-auth | ConvertFrom-Json

if ($spCredentials) {
    Write-Host "✅ Service Principal created" -ForegroundColor Green
    $azureCredentialsJson = $spCredentials | ConvertTo-Json -Compress
} else {
    Write-Host "❌ Failed to create Service Principal" -ForegroundColor Red
    exit 1
}
Write-Host ""

# ─────────────────────────────────────────────────────────────
# GRANT ACR PULL/PUSH PERMISSIONS
# ─────────────────────────────────────────────────────────────

Write-Host "🔒 Granting ACR permissions to Service Principal..." -ForegroundColor Yellow
$acrId = az acr show --name $AcrName --resource-group $ResourceGroup --query id -o tsv

az role assignment create `
    --assignee $spCredentials.clientId `
    --role AcrPush `
    --scope $acrId `
    --output none 2>$null

Write-Host "✅ ACR permissions granted" -ForegroundColor Green
Write-Host ""

# ─────────────────────────────────────────────────────────────
# SET GITHUB SECRETS
# ─────────────────────────────────────────────────────────────

Write-Host "🔐 Setting GitHub Secrets..." -ForegroundColor Yellow

# AZURE_CREDENTIALS
Write-Host "   Setting AZURE_CREDENTIALS..." -ForegroundColor Gray
$azureCredentialsJson | gh secret set AZURE_CREDENTIALS

# AZURE_SUBSCRIPTION_ID
Write-Host "   Setting AZURE_SUBSCRIPTION_ID..." -ForegroundColor Gray
$SubscriptionId | gh secret set AZURE_SUBSCRIPTION_ID

Write-Host "✅ GitHub secrets configured" -ForegroundColor Green
Write-Host ""

# ─────────────────────────────────────────────────────────────
# PROMPT FOR GOOGLE OAUTH (OPTIONAL)
# ─────────────────────────────────────────────────────────────

Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  GOOGLE OAUTH SETUP (Optional)" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""
Write-Host "To enable Google sign-in, you need OAuth credentials from:" -ForegroundColor Yellow
Write-Host "https://console.cloud.google.com/apis/credentials" -ForegroundColor Cyan
Write-Host ""

$setupGoogle = Read-Host "Do you want to configure Google OAuth now? (y/N)"
if ($setupGoogle -eq 'y' -or $setupGoogle -eq 'Y') {
    $googleClientId = Read-Host "Enter Google Client ID"
    $googleClientSecret = Read-Host "Enter Google Client Secret" -AsSecureString
    $googleClientSecretPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($googleClientSecret))
    
    Write-Host "   Setting GOOGLE_CLIENT_ID..." -ForegroundColor Gray
    $googleClientId | gh secret set GOOGLE_CLIENT_ID
    
    Write-Host "   Setting GOOGLE_CLIENT_SECRET..." -ForegroundColor Gray
    $googleClientSecretPlain | gh secret set GOOGLE_CLIENT_SECRET
    
    Write-Host "✅ Google OAuth secrets configured" -ForegroundColor Green
} else {
    Write-Host "⏭️  Skipping Google OAuth setup" -ForegroundColor Gray
    Write-Host "   You can add these secrets later in GitHub Settings" -ForegroundColor Gray
}
Write-Host ""

# ─────────────────────────────────────────────────────────────
# SUMMARY
# ─────────────────────────────────────────────────────────────

Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host "  ✅ SETUP COMPLETE!" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host ""
Write-Host "Azure Resources:" -ForegroundColor Cyan
Write-Host "  • Resource Group: $ResourceGroup" -ForegroundColor White
Write-Host "  • Container Registry: $AcrName.azurecr.io" -ForegroundColor White
Write-Host "  • Location: $Location" -ForegroundColor White
Write-Host ""
Write-Host "GitHub Secrets Configured:" -ForegroundColor Cyan
Write-Host "  • AZURE_CREDENTIALS" -ForegroundColor White
Write-Host "  • AZURE_SUBSCRIPTION_ID" -ForegroundColor White
if ($setupGoogle -eq 'y' -or $setupGoogle -eq 'Y') {
    Write-Host "  • GOOGLE_CLIENT_ID" -ForegroundColor White
    Write-Host "  • GOOGLE_CLIENT_SECRET" -ForegroundColor White
}
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Push your code to trigger deployment:" -ForegroundColor White
Write-Host "     git add . && git commit -m 'Deploy' && git push origin main" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. Monitor deployment at:" -ForegroundColor White
Write-Host "     https://github.com/$GitHubRepoFull/actions" -ForegroundColor Cyan
Write-Host ""
Write-Host "  3. After deployment, configure DNS for aigents.au" -ForegroundColor White
Write-Host ""


