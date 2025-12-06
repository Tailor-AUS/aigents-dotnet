#!/bin/bash

# ═══════════════════════════════════════════════════════════════
# AIGENTS - AZURE BOOTSTRAP SCRIPT
# ═══════════════════════════════════════════════════════════════
# Run this ONCE to set up Azure infrastructure
# ═══════════════════════════════════════════════════════════════

set -e

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}"
echo "╔═══════════════════════════════════════════════════════════════╗"
echo "║           AIGENTS - AZURE BOOTSTRAP                           ║"
echo "╚═══════════════════════════════════════════════════════════════╝"
echo -e "${NC}"

# ───────────────────────────────────────────────────────────────
# CHECK PREREQUISITES
# ───────────────────────────────────────────────────────────────

echo -e "${BLUE}Checking prerequisites...${NC}"

if ! command -v az &> /dev/null; then
    echo -e "${RED}❌ Azure CLI not found. Install: https://docs.microsoft.com/cli/azure/install-azure-cli${NC}"
    exit 1
fi
echo -e "${GREEN}✓ Azure CLI found${NC}"

if ! command -v gh &> /dev/null; then
    echo -e "${YELLOW}⚠ GitHub CLI not found (optional). Install: https://cli.github.com/${NC}"
fi

# ───────────────────────────────────────────────────────────────
# LOGIN
# ───────────────────────────────────────────────────────────────

echo -e "\n${BLUE}Logging in to Azure...${NC}"
if ! az account show &> /dev/null; then
    az login
fi

SUBSCRIPTION_ID=$(az account show --query id -o tsv)
SUBSCRIPTION_NAME=$(az account show --query name -o tsv)
echo -e "${GREEN}✓ Logged in to: ${SUBSCRIPTION_NAME}${NC}"

# ───────────────────────────────────────────────────────────────
# DEPLOY BOOTSTRAP
# ───────────────────────────────────────────────────────────────

echo -e "\n${BLUE}Deploying bootstrap infrastructure...${NC}"

DEPLOYMENT_OUTPUT=$(az deployment sub create \
  --location australiaeast \
  --template-file infra/bootstrap.bicep \
  --query "{rg:properties.outputs.resourceGroupName.value, acr:properties.outputs.acrName.value, acrServer:properties.outputs.acrLoginServer.value}" \
  -o json)

RG_NAME=$(echo $DEPLOYMENT_OUTPUT | jq -r '.rg')
ACR_NAME=$(echo $DEPLOYMENT_OUTPUT | jq -r '.acr')
ACR_SERVER=$(echo $DEPLOYMENT_OUTPUT | jq -r '.acrServer')

echo -e "${GREEN}✓ Resource Group: ${RG_NAME}${NC}"
echo -e "${GREEN}✓ Container Registry: ${ACR_NAME}${NC}"

# ───────────────────────────────────────────────────────────────
# CREATE SERVICE PRINCIPAL
# ───────────────────────────────────────────────────────────────

echo -e "\n${BLUE}Creating Service Principal for GitHub Actions...${NC}"

SP_OUTPUT=$(az ad sp create-for-rbac \
  --name "aigents-github-actions" \
  --role contributor \
  --scopes /subscriptions/${SUBSCRIPTION_ID}/resourceGroups/${RG_NAME} \
  --sdk-auth)

echo -e "${GREEN}✓ Service Principal created${NC}"

# ───────────────────────────────────────────────────────────────
# GET ACR CREDENTIALS
# ───────────────────────────────────────────────────────────────

ACR_USERNAME=$(az acr credential show --name $ACR_NAME --query username -o tsv)
ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --query "passwords[0].value" -o tsv)

# ───────────────────────────────────────────────────────────────
# OUTPUT
# ───────────────────────────────────────────────────────────────

echo -e "\n${GREEN}"
echo "╔═══════════════════════════════════════════════════════════════╗"
echo "║                    BOOTSTRAP COMPLETE! 🎉                      ║"
echo "╚═══════════════════════════════════════════════════════════════╝"
echo -e "${NC}"

echo -e "${YELLOW}Now add these GitHub Secrets:${NC}"
echo ""
echo -e "${BLUE}1. AZURE_CREDENTIALS:${NC}"
echo "$SP_OUTPUT"
echo ""
echo -e "${BLUE}2. Update .github/workflows/cd.yml:${NC}"
echo "   ACR_NAME: ${ACR_NAME}"
echo ""
echo -e "${BLUE}3. Don't forget to add:${NC}"
echo "   - GOOGLE_CLIENT_ID"  
echo "   - GOOGLE_CLIENT_SECRET"
echo ""
echo -e "${YELLOW}Note: Azure AI Foundry (GPT-4o) is deployed automatically - no API key needed!${NC}"
echo ""
echo -e "${BLUE}Next step:${NC}"
echo "   Push to main branch and watch the magic happen! ✨"
echo ""
