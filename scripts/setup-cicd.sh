#!/bin/bash
# ═══════════════════════════════════════════════════════════════
# AIGENTS CI/CD SETUP SCRIPT (Bash)
# Sets up Azure resources and GitHub secrets
# ═══════════════════════════════════════════════════════════════

set -e

RESOURCE_GROUP="${1:-aigents-rg}"
LOCATION="${2:-australiaeast}"
ACR_NAME="${3:-aigentsacr}"

echo "═══════════════════════════════════════════════════════════════"
echo "  AIGENTS CI/CD SETUP"
echo "═══════════════════════════════════════════════════════════════"
echo ""

# ─────────────────────────────────────────────────────────────
# CHECK PREREQUISITES
# ─────────────────────────────────────────────────────────────

echo "📋 Checking prerequisites..."

if ! command -v az &> /dev/null; then
    echo "❌ Azure CLI not found. Install from: https://docs.microsoft.com/cli/azure/install-azure-cli"
    exit 1
fi

if ! command -v gh &> /dev/null; then
    echo "❌ GitHub CLI not found. Install from: https://cli.github.com/"
    exit 1
fi

echo "✅ Prerequisites OK"
echo ""

# ─────────────────────────────────────────────────────────────
# LOGIN TO AZURE
# ─────────────────────────────────────────────────────────────

echo "🔐 Checking Azure login..."
if ! az account show &> /dev/null; then
    echo "   Logging in to Azure..."
    az login
fi
SUBSCRIPTION_ID=$(az account show --query id -o tsv)
ACCOUNT_NAME=$(az account show --query user.name -o tsv)
echo "✅ Logged in as: $ACCOUNT_NAME"
echo "   Subscription: $SUBSCRIPTION_ID"
echo ""

# ─────────────────────────────────────────────────────────────
# LOGIN TO GITHUB
# ─────────────────────────────────────────────────────────────

echo "🔐 Checking GitHub login..."
if ! gh auth status &> /dev/null; then
    echo "   Logging in to GitHub..."
    gh auth login
fi
echo "✅ GitHub authenticated"

GITHUB_REPO=$(gh repo view --json nameWithOwner -q .nameWithOwner 2>/dev/null || echo "")
if [ -z "$GITHUB_REPO" ]; then
    echo "❌ Not in a GitHub repository. Please run from your repo directory."
    exit 1
fi
echo "📁 Repository: $GITHUB_REPO"
echo ""

# ─────────────────────────────────────────────────────────────
# CREATE AZURE RESOURCE GROUP
# ─────────────────────────────────────────────────────────────

echo "☁️  Creating Azure Resource Group..."
az group create --name "$RESOURCE_GROUP" --location "$LOCATION" --output none 2>/dev/null || true
echo "✅ Resource Group: $RESOURCE_GROUP"
echo ""

# ─────────────────────────────────────────────────────────────
# CREATE AZURE CONTAINER REGISTRY
# ─────────────────────────────────────────────────────────────

echo "📦 Creating Azure Container Registry..."
if ! az acr show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" &> /dev/null; then
    az acr create \
        --name "$ACR_NAME" \
        --resource-group "$RESOURCE_GROUP" \
        --sku Basic \
        --admin-enabled true \
        --output none
    echo "✅ ACR Created: $ACR_NAME.azurecr.io"
else
    echo "   ACR already exists: $ACR_NAME.azurecr.io"
fi
echo ""

# ─────────────────────────────────────────────────────────────
# CREATE SERVICE PRINCIPAL
# ─────────────────────────────────────────────────────────────

echo "🔑 Creating Service Principal for GitHub Actions..."
SP_NAME="aigents-github-actions"

# Delete existing SP if exists
EXISTING_SP=$(az ad sp list --display-name "$SP_NAME" --query "[0].appId" -o tsv 2>/dev/null || echo "")
if [ -n "$EXISTING_SP" ]; then
    echo "   Deleting existing Service Principal..."
    az ad sp delete --id "$EXISTING_SP" 2>/dev/null || true
fi

# Create new Service Principal
SP_CREDENTIALS=$(az ad sp create-for-rbac \
    --name "$SP_NAME" \
    --role contributor \
    --scopes "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP" \
    --sdk-auth)

if [ -z "$SP_CREDENTIALS" ]; then
    echo "❌ Failed to create Service Principal"
    exit 1
fi
echo "✅ Service Principal created"

CLIENT_ID=$(echo "$SP_CREDENTIALS" | jq -r .clientId)
echo ""

# ─────────────────────────────────────────────────────────────
# GRANT ACR PERMISSIONS
# ─────────────────────────────────────────────────────────────

echo "🔒 Granting ACR permissions to Service Principal..."
ACR_ID=$(az acr show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" --query id -o tsv)
az role assignment create \
    --assignee "$CLIENT_ID" \
    --role AcrPush \
    --scope "$ACR_ID" \
    --output none 2>/dev/null || true
echo "✅ ACR permissions granted"
echo ""

# ─────────────────────────────────────────────────────────────
# SET GITHUB SECRETS
# ─────────────────────────────────────────────────────────────

echo "🔐 Setting GitHub Secrets..."

echo "   Setting AZURE_CREDENTIALS..."
echo "$SP_CREDENTIALS" | gh secret set AZURE_CREDENTIALS

echo "   Setting AZURE_SUBSCRIPTION_ID..."
echo "$SUBSCRIPTION_ID" | gh secret set AZURE_SUBSCRIPTION_ID

echo "✅ GitHub secrets configured"
echo ""

# ─────────────────────────────────────────────────────────────
# GOOGLE OAUTH (OPTIONAL)
# ─────────────────────────────────────────────────────────────

echo "═══════════════════════════════════════════════════════════════"
echo "  GOOGLE OAUTH SETUP (Optional)"
echo "═══════════════════════════════════════════════════════════════"
echo ""
echo "To enable Google sign-in, you need OAuth credentials from:"
echo "https://console.cloud.google.com/apis/credentials"
echo ""

read -p "Do you want to configure Google OAuth now? (y/N) " -n 1 -r
echo ""
if [[ $REPLY =~ ^[Yy]$ ]]; then
    read -p "Enter Google Client ID: " GOOGLE_CLIENT_ID
    read -s -p "Enter Google Client Secret: " GOOGLE_CLIENT_SECRET
    echo ""
    
    echo "   Setting GOOGLE_CLIENT_ID..."
    echo "$GOOGLE_CLIENT_ID" | gh secret set GOOGLE_CLIENT_ID
    
    echo "   Setting GOOGLE_CLIENT_SECRET..."
    echo "$GOOGLE_CLIENT_SECRET" | gh secret set GOOGLE_CLIENT_SECRET
    
    echo "✅ Google OAuth secrets configured"
else
    echo "⏭️  Skipping Google OAuth setup"
    echo "   You can add these secrets later in GitHub Settings"
fi
echo ""

# ─────────────────────────────────────────────────────────────
# SUMMARY
# ─────────────────────────────────────────────────────────────

echo "═══════════════════════════════════════════════════════════════"
echo "  ✅ SETUP COMPLETE!"
echo "═══════════════════════════════════════════════════════════════"
echo ""
echo "Azure Resources:"
echo "  • Resource Group: $RESOURCE_GROUP"
echo "  • Container Registry: $ACR_NAME.azurecr.io"
echo "  • Location: $LOCATION"
echo ""
echo "GitHub Secrets Configured:"
echo "  • AZURE_CREDENTIALS"
echo "  • AZURE_SUBSCRIPTION_ID"
echo ""
echo "Next Steps:"
echo "  1. Push your code to trigger deployment:"
echo "     git add . && git commit -m 'Deploy' && git push origin main"
echo ""
echo "  2. Monitor deployment at:"
echo "     https://github.com/$GITHUB_REPO/actions"
echo ""
echo "  3. After deployment, configure DNS for aigents.au"
echo ""



