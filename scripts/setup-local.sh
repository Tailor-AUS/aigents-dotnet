#!/bin/bash

# ═══════════════════════════════════════════════════════════════
# AIGENTS - LOCAL DEVELOPMENT SETUP (ARM64)
# ═══════════════════════════════════════════════════════════════
# Sets up Docker containers and runs the app locally
# Compatible with Apple Silicon / ARM64
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
echo "║           AIGENTS - LOCAL DEVELOPMENT                         ║"
echo "╚═══════════════════════════════════════════════════════════════╝"
echo -e "${NC}"

# ───────────────────────────────────────────────────────────────
# CHECK PREREQUISITES
# ───────────────────────────────────────────────────────────────

echo -e "${BLUE}Checking prerequisites...${NC}"

if ! command -v docker &> /dev/null; then
    echo -e "${RED}❌ Docker not found. Install Docker Desktop: https://www.docker.com/products/docker-desktop/${NC}"
    exit 1
fi
echo -e "${GREEN}✓ Docker found${NC}"

if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}❌ .NET SDK not found. Install: https://dotnet.microsoft.com/download${NC}"
    exit 1
fi
echo -e "${GREEN}✓ .NET SDK found ($(dotnet --version))${NC}"

# Check architecture
ARCH=$(uname -m)
echo -e "${GREEN}✓ Architecture: ${ARCH}${NC}"

# ───────────────────────────────────────────────────────────────
# START DOCKER CONTAINERS
# ───────────────────────────────────────────────────────────────

echo -e "\n${BLUE}Starting Docker containers...${NC}"

docker compose up -d

echo -e "${GREEN}✓ SQL Server (Azure SQL Edge) starting on port 1433${NC}"
echo -e "${GREEN}✓ Redis starting on port 6379${NC}"
echo -e "${GREEN}✓ MailDev starting on port 1080 (web) / 1025 (smtp)${NC}"

# Wait for SQL Server to be ready
echo -e "\n${YELLOW}Waiting for SQL Server to be ready...${NC}"
sleep 10

for i in {1..30}; do
    if docker exec aigents-sql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Aigents@2024!" -Q "SELECT 1" &> /dev/null; then
        echo -e "${GREEN}✓ SQL Server is ready!${NC}"
        break
    fi
    echo -n "."
    sleep 2
done

# ───────────────────────────────────────────────────────────────
# CONFIGURE USER SECRETS
# ───────────────────────────────────────────────────────────────

echo -e "\n${BLUE}Configuring user secrets...${NC}"

# Check if secrets already exist
SECRETS_FILE="$HOME/.microsoft/usersecrets/$(grep -o 'UserSecretsId>[^<]*' src/Aigents.AppHost/*.csproj 2>/dev/null | cut -d'>' -f2)/secrets.json"

if [ ! -f "$SECRETS_FILE" ]; then
    echo -e "${YELLOW}Setting up secrets for local development...${NC}"
    
    # Set connection strings for local Docker
    dotnet user-secrets set "ConnectionStrings:aigentsdb" "Server=localhost,1433;Database=aigentsdb;User Id=sa;Password=Aigents@2024!;TrustServerCertificate=True" --project src/Aigents.Api
    dotnet user-secrets set "ConnectionStrings:redis" "localhost:6379" --project src/Aigents.Api
    
    echo -e "${YELLOW}⚠ You need to set Azure AI Foundry secrets manually:${NC}"
    echo ""
    echo "  dotnet user-secrets set \"Parameters:azure-ai-endpoint\" \"https://YOUR-RESOURCE.openai.azure.com/\" --project src/Aigents.AppHost"
    echo "  dotnet user-secrets set \"Parameters:azure-ai-deployment\" \"gpt-4o\" --project src/Aigents.AppHost"
    echo ""
    echo -e "${YELLOW}⚠ And Google OAuth secrets:${NC}"
    echo ""
    echo "  dotnet user-secrets set \"Parameters:google-client-id\" \"YOUR-CLIENT-ID\" --project src/Aigents.AppHost"
    echo "  dotnet user-secrets set \"Parameters:google-client-secret\" \"YOUR-SECRET\" --project src/Aigents.AppHost"
    echo ""
else
    echo -e "${GREEN}✓ Secrets already configured${NC}"
fi

# ───────────────────────────────────────────────────────────────
# BUILD
# ───────────────────────────────────────────────────────────────

echo -e "\n${BLUE}Building solution...${NC}"

dotnet build

echo -e "${GREEN}✓ Build successful!${NC}"

# ───────────────────────────────────────────────────────────────
# DONE
# ───────────────────────────────────────────────────────────────

echo -e "\n${GREEN}"
echo "╔═══════════════════════════════════════════════════════════════╗"
echo "║                    SETUP COMPLETE! 🎉                         ║"
echo "╚═══════════════════════════════════════════════════════════════╝"
echo -e "${NC}"

echo -e "${BLUE}Services running:${NC}"
echo "  • SQL Server:  localhost:1433"
echo "  • Redis:       localhost:6379"
echo "  • MailDev UI:  http://localhost:1080"
echo ""
echo -e "${BLUE}To run the app:${NC}"
echo "  dotnet run --project src/Aigents.AppHost"
echo ""
echo -e "${BLUE}Aspire Dashboard:${NC}"
echo "  https://localhost:17225"
echo ""
echo -e "${BLUE}To stop containers:${NC}"
echo "  docker compose down"
echo ""
