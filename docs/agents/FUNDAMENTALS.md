# 🤖 Agent Fundamentals: Aigents

Welcome to the **Aigents** project! This document is your primary onboarding resource. Read this before performing any tasks to ensure you understand the project structure, tech stack, and safety rules.

## 🌟 Project Overview
**Aigents** is an AI-powered real estate platform focusing on the Brisbane and Gold Coast markets. It acts as a digital buyer's and seller's agent, streamlining the property journey using advanced AI.

- **Core Problem:** Traditional real estate processes are slow, opaque, and manual. Aigents automates listing generation, buyer matching, and agreement signing.
- **Key Features:** AI-powered chat for buyers, automated listing creation for sellers, map-based property exploration, and off-market distribution.

## 🛠️ Tech Stack
| Layer | Technology |
|-------|------------|
| **Frontend** | Blazor Server (.NET 8) |
| **Backend API** | ASP.NET Core + Carter (Minimal APIs) |
| **Orchestration** | .NET Aspire |
| **Database** | SQL Server (Azure SQL Edge) + EF Core |
| **Caching** | Redis |
| **AI Engine** | Azure AI Foundry (GPT-4o) |
| **Auth** | Google OAuth 2.0 |
| **Hosting** | Azure Container Apps |

## 🔌 Port Configuration
Default ports used during local development:

| Service | Port | Notes |
|---------|------|-------|
| Aspire Dashboard | 17225 / 15225 | Orchestrator UI (HTTPS/HTTP) |
| Frontend (Web) | Dynamic | Managed by Aspire; access via Dashboard |
| Backend API | 5001 | Fixed HTTP endpoint in AppHost |
| Database | 1433 | SQL Server (Container) |
| Redis | 6379 | Redis (Container) |
| MailDev | 1080 / 1025 | Email testing (UI / SMTP) |

## 📁 Key File Locations
```text
aigents-dotnet/
├── docs/
│   ├── agents/
│   │   ├── FUNDAMENTALS.md  ← You are here (Onboarding)
│   │   └── BACKLOG.md       ← Tasks and issue tracking
│   └── VISION.md            ← Product vision and roadmap
├── src/
│   ├── Aigents.AppHost/     ← Aspire Orchestrator (entry point)
│   ├── Aigents.Web/         ← Blazor Frontend (UI Components)
│   ├── Aigents.Api/         ← Backend API (Vertical Slices)
│   ├── Aigents.Domain/      ← Core Entities & Logic
│   └── Aigents.Infrastructure/ ← External Services & EF Core
├── scripts/                 ← Setup and deployment scripts
└── docker-compose.yml       ← Local infrastructure (DB, Redis)
```

## ⚠️ Critical Rules
- **NEVER** commit secret keys or `appsettings.Development.json` if it contains real credentials. Use `dotnet user-secrets`.
- **ALWAYS** run `dotnet build` before submitting changes to catch compilation errors early.
- **DO NOT** modify the `Infrastructure` layer without checking the Impact on `Domain` and `Api` layers.
- **STRICTLY** follow the Vertical Slice Architecture in `Aigents.Api/Features`.

## 💻 Common Commands
| Action | Command |
|--------|---------|
| Setup Local Infra | `.\scripts\setup-local.ps1` |
| Run Entire App | `dotnet run --project src\Aigents.AppHost` |
| View Containers | `docker compose ps` |
| Reset Database | `docker compose down -v` then run setup script |
| Add Migration | `dotnet ef migrations add <Name> --project src\Aigents.Infrastructure --startup-project src\Aigents.Api` |

## 📝 Reporting Format
Upon task completion, provide a report using this structure:
1. **Problem:** Summary of the issue or feature request.
2. **Root Cause:** (If bug) Why was it happening?
3. **Changes:** List of modified/created files and key logic changes.
4. **Verification:** How did you test it? (Manual steps or automated tests).
5. **Follow-up:** Are there remaining items or technical debt introduced?
