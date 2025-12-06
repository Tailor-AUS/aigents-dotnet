# 🏠 Aigents - AI Real Estate Platform

**AI-powered buyer's and seller's agent for Brisbane and Gold Coast real estate.**

Built with .NET 8, Aspire, Blazor, Azure AI Foundry (GPT-4o), and vertical slice architecture.

---

## ✨ Features

### 🔍 Buy Journey
- Chat with AI buyer's agent
- Search on-market and off-market properties
- Get suburb insights and valuations
- Book inspections

### 📝 Sell Journey
1. **Enter your address** → AI generates a complete listing
2. **Review & edit** → Customize headline, description, features
3. **Sign agreement** → Open listing agreement (any agent can sell)
4. **Publish** → Listing sent to all local agents as exclusive off-market opportunity
5. **Agent who finds buyer earns commission**

*Like Facebook Marketplace, but for real estate!*

---

## 🚀 Quick Start (Windows ARM64 / Snapdragon)

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (ARM64)
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Azure AI Foundry access

### Setup

```powershell
# 1. Start Docker containers (SQL Server, Redis, MailDev)
.\scripts\setup-local.ps1

# 2. Set your Azure AI credentials
dotnet user-secrets set "Parameters:azure-ai-endpoint" "https://YOUR-RESOURCE.openai.azure.com/" --project src\Aigents.AppHost
dotnet user-secrets set "Parameters:azure-ai-deployment" "gpt-4o" --project src\Aigents.AppHost

# 3. Set Google OAuth credentials
dotnet user-secrets set "Parameters:google-client-id" "YOUR-CLIENT-ID" --project src\Aigents.AppHost
dotnet user-secrets set "Parameters:google-client-secret" "YOUR-SECRET" --project src\Aigents.AppHost

# 4. Run the app
dotnet run --project src\Aigents.AppHost
```

Open the Aspire Dashboard: https://localhost:17225

### Docker Services (ARM64 Compatible)
| Service | Image | Port | Notes |
|---------|-------|------|-------|
| SQL Server | Azure SQL Edge | 1433 | ARM64 native |
| Redis | Redis Alpine | 6379 | ARM64 native |
| MailDev | MailDev | 1080/1025 | Email testing |

```powershell
# View containers
docker compose ps

# View logs
docker compose logs -f

# Stop containers
docker compose down

# Reset all data
docker compose down -v
```

---

## 📁 Project Structure

```
Aigents/
├── src/
│   ├── Aigents.AppHost/          # 🎯 Aspire orchestrator
│   ├── Aigents.Api/              # 🔌 Backend API
│   │   └── Features/             # Vertical slices
│   │       ├── Auth/             # Google SSO
│   │       ├── Chat/             # AI chat (buy journey)
│   │       ├── Leads/            # Lead management
│   │       └── Listings/         # ⭐ Sell journey
│   ├── Aigents.Web/              # 🖥️ Blazor frontend
│   ├── Aigents.Domain/           # 📋 Domain entities
│   └── Aigents.Infrastructure/   # 🔧 EF Core, Azure AI
├── infra/                        # 🏗️ Bicep templates
├── scripts/
│   ├── setup-local.ps1          # Windows setup
│   ├── setup-local.sh           # Mac/Linux setup
│   └── bootstrap-azure.sh       # Azure bootstrap
└── docker-compose.yml           # ARM64 containers
```

---

## 📊 API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/google` | Google SSO |
| GET | `/api/auth/me` | Get current user |

### Chat (Buy Journey)
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/chat` | Send message to AI agent |

### Listings (Sell Journey) ⭐
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/listings` | Create listing (AI generates content) |
| GET | `/api/listings/my/{userId}` | Get my listings |
| GET | `/api/listings/{id}` | Get listing details |
| PUT | `/api/listings/{id}` | Update listing |
| GET | `/api/listings/{id}/agreement` | Get agreement text |
| POST | `/api/listings/{id}/sign` | Sign open listing agreement |
| POST | `/api/listings/{id}/publish` | Distribute to local agents |

### Leads
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/leads` | List all leads |
| POST | `/api/leads/handoff` | Handoff to human agent |

---

## 🏗️ Tech Stack

| Layer | Technology |
|-------|------------|
| Orchestration | .NET Aspire |
| Frontend | Blazor Server |
| API | ASP.NET Core + Carter |
| CQRS | MediatR |
| Validation | FluentValidation |
| Database | SQL Server + EF Core |
| Cache | Redis |
| AI | **Azure AI Foundry (GPT-4o)** |
| Auth | Google OAuth |
| Local DB | Azure SQL Edge (ARM64) |

---

## 🚢 Deploy to Azure

```powershell
# 1. Login to Azure
az login

# 2. Bootstrap (first time - use Git Bash or WSL)
./scripts/bootstrap-azure.sh

# 3. Add GitHub secrets:
#    - AZURE_CREDENTIALS
#    - GOOGLE_CLIENT_ID
#    - GOOGLE_CLIENT_SECRET

# 4. Push to main - CI/CD handles the rest!
git push origin main
```

### What Gets Deployed
- ✅ Azure AI Foundry with GPT-4o
- ✅ Container Apps (API + Web)
- ✅ SQL Server
- ✅ Redis Cache
- ✅ Log Analytics

---

## 💰 Estimated Costs

| Resource | Monthly |
|----------|---------|
| Azure AI Foundry | ~$10-50 |
| Container Apps | ~$30 |
| SQL Server Basic | ~$5 |
| Redis Basic | ~$15 |
| **Total** | **~$60-100/month** |

---

## 📝 License

Proprietary - All rights reserved

## 👥 Team

Built by Knox & AI
