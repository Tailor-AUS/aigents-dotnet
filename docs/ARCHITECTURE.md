# Aigents Platform Architecture
## Infrastructure & Technical Design v1.0

---

## 1. Architecture Overview

```
┌─────────────────────────────────────────────────────────────────────────┐
│                              CLIENTS                                     │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                          │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐                │
│  │  Web     │  │  iOS     │  │  Android │  │  Watch   │                │
│  │  (Blazor)│  │  App     │  │  App     │  │  Apps    │                │
│  └────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘                │
│       │             │             │             │                        │
└───────┴─────────────┴─────────────┴─────────────┴────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                         AZURE FRONT DOOR                                 │
│                    (Global Load Balancer + WAF + CDN)                    │
└───────────────────────────────────┬─────────────────────────────────────┘
                                    │
        ┌───────────────────────────┴───────────────────────────┐
        │                                                        │
        ▼                                                        ▼
┌───────────────────┐                              ┌───────────────────┐
│   WEB TIER        │                              │   API TIER        │
├───────────────────┤                              ├───────────────────┤
│ Azure Container   │                              │ Azure Container   │
│ Apps              │                              │ Apps              │
│                   │                              │                   │
│ • Aigents.Web     │◄────────────────────────────►│ • Aigents.Api     │
│ • Blazor SSR      │                              │ • REST/gRPC       │
│ • Static Assets   │                              │ • Minimal API     │
└───────────────────┘                              └─────────┬─────────┘
                                                              │
┌─────────────────────────────────────────────────────────────┴─────────┐
│                         SERVICES LAYER                                  │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐                  │
│  │ AI Service   │  │ Call Intel   │  │ Property     │                  │
│  │              │  │ Service      │  │ Service      │                  │
│  │ • Gemini     │  │ • Transcribe │  │ • Listings   │                  │
│  │ • Embeddings │  │ • Summarize  │  │ • Valuations │                  │
│  │ • RAG        │  │ • Actions    │  │ • Search     │                  │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘                  │
│         │                  │                  │                          │
└─────────┴──────────────────┴──────────────────┴──────────────────────────┘
                                    │
┌───────────────────────────────────┴─────────────────────────────────────┐
│                          DATA LAYER                                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌────────────┐  │
│  │ Cosmos DB    │  │ Azure Redis  │  │ Blob Storage │  │ AI Search  │  │
│  │              │  │              │  │              │  │            │  │
│  │ • Contacts   │  │ • Sessions   │  │ • Recordings │  │ • Vector   │  │
│  │ • Properties │  │ • Cache      │  │ • Documents  │  │ • Semantic │  │
│  │ • Calls      │  │ • Pub/Sub    │  │ • Transcripts│  │ • Hybrid   │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  └────────────┘  │
│                                                                          │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## 2. Component Details

### 2.1 Web Tier (Aigents.Web)

| Aspect | Technology |
|--------|------------|
| Framework | .NET 9 Blazor (SSR + WebAssembly hybrid) |
| Hosting | Azure Container Apps |
| Auth | Microsoft Entra ID + Google OAuth |
| State | Redis distributed cache |

### 2.2 API Tier (Aigents.Api)

| Aspect | Technology |
|--------|------------|
| Framework | .NET 9 Minimal API |
| Auth | JWT Bearer tokens |
| Docs | OpenAPI/Swagger |
| Rate Limiting | Azure API Management |

### 2.3 Mobile Apps

| Platform | Technology |
|----------|------------|
| iOS | .NET MAUI or Swift native |
| Android | .NET MAUI or Kotlin native |
| Shared | REST API + SignalR |

---

## 3. Call Intelligence Pipeline

```
┌─────────────────────────────────────────────────────────────────────┐
│                    CALL INTELLIGENCE FLOW                            │
└─────────────────────────────────────────────────────────────────────┘

  MOBILE APP                    AZURE                         AI
  ─────────                    ─────                         ──

  ┌─────────┐
  │ Agent   │
  │ takes   │
  │ call    │
  └────┬────┘
       │
       │ 1. Audio stream (WebSocket)
       ▼
  ┌─────────────────────┐
  │ Blob Storage        │◄──── Store raw audio
  │ (call-recordings)   │
  └─────────┬───────────┘
            │
            │ 2. Trigger transcription
            ▼
  ┌─────────────────────┐
  │ Azure Speech        │
  │ Service             │───── Real-time STT
  │ (Speech-to-Text)    │
  └─────────┬───────────┘
            │
            │ 3. Transcript text
            ▼
  ┌─────────────────────┐
  │ Event Hub           │───── Queue for processing
  │ (call-transcripts)  │
  └─────────┬───────────┘
            │
            ▼
  ┌─────────────────────┐     ┌─────────────────┐
  │ Call Intel Service  │────►│ Gemini API      │
  │ (Azure Function)    │     │                 │
  │                     │     │ • Summarize     │
  │ • Extract entities  │◄────│ • Action items  │
  │ • Match properties  │     │ • Sentiment     │
  │ • Score lead        │     └─────────────────┘
  └─────────┬───────────┘
            │
            │ 4. Structured data
            ▼
  ┌─────────────────────┐
  │ Cosmos DB           │
  │                     │
  │ • Call record       │
  │ • Contact update    │
  │ • Tasks created     │
  └─────────────────────┘
            │
            │ 5. Push notification
            ▼
  ┌─────────────────────┐
  │ Mobile App          │───── "Call summary ready"
  └─────────────────────┘
```

---

## 4. Data Models

### 4.1 Contact

```json
{
  "id": "uuid",
  "partitionKey": "agent-uuid",
  "type": "Contact",
  "name": "John Smith",
  "phone": "+61412345678",
  "email": "john@example.com",
  "classification": "Buyer",
  "leadScore": 85,
  "status": "Hot",
  "source": "ContactImport",
  "propertyInterests": [
    {
      "propertyId": "prop-uuid",
      "address": "45 Ocean St, Manly",
      "interestLevel": "High",
      "notes": "Attended open home"
    }
  ],
  "interactions": [
    {
      "timestamp": "2025-12-11T10:45:00Z",
      "type": "Call",
      "duration": 272,
      "summary": "Ready to make offer...",
      "actionItems": ["Call Thursday", "Send comparables"]
    }
  ],
  "createdAt": "2025-12-01T00:00:00Z",
  "updatedAt": "2025-12-11T10:50:00Z"
}
```

### 4.2 CallRecord

```json
{
  "id": "uuid",
  "partitionKey": "agent-uuid",
  "type": "CallRecord",
  "contactId": "contact-uuid",
  "direction": "Incoming",
  "timestamp": "2025-12-11T10:45:00Z",
  "duration": 272,
  "recordingUrl": "https://blob.../calls/uuid.wav",
  "transcriptUrl": "https://blob.../transcripts/uuid.json",
  "aiSummary": "John is ready to offer on 45 Ocean St...",
  "sentiment": "Positive",
  "propertiesMentioned": ["prop-uuid-1", "prop-uuid-2"],
  "actionItems": [
    {
      "description": "Call Thursday after conveyancer",
      "dueDate": "2025-12-12",
      "status": "Pending"
    }
  ]
}
```

### 4.3 InspectionCheckin

```json
{
  "id": "uuid",
  "partitionKey": "property-uuid",
  "type": "InspectionCheckin",
  "propertyId": "prop-uuid",
  "inspectionDate": "2025-12-11T10:00:00Z",
  "attendee": {
    "name": "Sarah Chen",
    "phone": "+61498765432",
    "email": "sarah@example.com"
  },
  "preApproved": true,
  "budget": "$2M",
  "timeline": "Now",
  "optInAlerts": true,
  "checkinTime": "2025-12-11T10:05:32Z"
}
```

---

## 5. Azure Resources

### 5.1 Resource Map

```
aigents-prod (Resource Group)
├── aigents-fd              Azure Front Door
├── aigents-web-aca         Container App (Web)
├── aigents-api-aca         Container App (API)
├── aigents-func            Function App (Background Jobs)
├── aigents-cosmos          Cosmos DB (NoSQL)
├── aigents-redis           Azure Cache for Redis
├── aigents-storage         Storage Account
│   ├── call-recordings     Blob Container
│   ├── transcripts         Blob Container
│   └── documents           Blob Container
├── aigents-speech          Speech Services
├── aigents-ai-search       AI Search
├── aigents-eventhub        Event Hub Namespace
├── aigents-kv              Key Vault
├── aigents-apim            API Management
├── aigents-logs            Log Analytics
└── aigents-insights        Application Insights
```

### 5.2 Estimated Costs (Monthly)

| Resource | SKU | Est. Cost |
|----------|-----|-----------|
| Container Apps (Web) | 1 vCPU, 2GB | $50 |
| Container Apps (API) | 2 vCPU, 4GB | $100 |
| Cosmos DB | Serverless | $50-200 |
| Redis Cache | Basic C0 | $20 |
| Storage | LRS Hot | $20 |
| Speech Services | Pay-as-you-go | $50-100 |
| AI Search | Basic | $75 |
| Event Hub | Basic | $15 |
| Front Door | Standard | $35 |
| **Total** | | **$400-600** |

---

## 6. Security

### 6.1 Authentication Flow

```
┌──────────┐          ┌──────────┐          ┌──────────┐
│  Mobile  │──────────│  Entra   │──────────│  API     │
│  App     │          │  ID      │          │          │
└────┬─────┘          └────┬─────┘          └────┬─────┘
     │                     │                     │
     │  1. Login request   │                     │
     │────────────────────►│                     │
     │                     │                     │
     │  2. OAuth flow      │                     │
     │◄────────────────────│                     │
     │                     │                     │
     │  3. ID + Access     │                     │
     │     tokens          │                     │
     │◄────────────────────│                     │
     │                     │                     │
     │  4. API call with Bearer token            │
     │───────────────────────────────────────────►
     │                     │                     │
     │                     │  5. Validate token  │
     │                     │◄────────────────────│
     │                     │                     │
     │  6. Response                              │
     │◄──────────────────────────────────────────│
```

### 6.2 Data Encryption

| Layer | Method |
|-------|--------|
| Transit | TLS 1.3 |
| Rest (Cosmos) | AES-256 |
| Rest (Blobs) | AES-256 |
| Call Recordings | Customer-managed keys |
| Secrets | Azure Key Vault |

### 6.3 Privacy Compliance

| Requirement | Implementation |
|-------------|----------------|
| Call Recording Consent | Pre-call disclaimer + opt-out |
| Data Retention | 7 years (configurable) |
| Right to Delete | Self-service in app |
| Data Export | GDPR export endpoint |
| Audit Logs | Cosmos DB change feed |

---

## 7. Scalability

### 7.1 Auto-Scaling Rules

| Component | Trigger | Scale |
|-----------|---------|-------|
| Web | CPU > 70% | +1 replica (max 10) |
| API | Requests > 1000/min | +1 replica (max 20) |
| Functions | Queue depth > 100 | +1 instance |

### 7.2 Performance Targets

| Metric | Target |
|--------|--------|
| API Response Time (p95) | < 200ms |
| Call Transcription Latency | < 30 seconds |
| Dashboard Load Time | < 2 seconds |
| Push Notification Delivery | < 5 seconds |

---

## 8. Mobile Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     MOBILE APP LAYERS                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │                    UI LAYER                             │ │
│  │  • SwiftUI (iOS) / Jetpack Compose (Android)           │ │
│  │  • MVVM Pattern                                         │ │
│  └────────────────────────────────────────────────────────┘ │
│                            │                                 │
│                            ▼                                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │                  DOMAIN LAYER                           │ │
│  │  • Use Cases                                            │ │
│  │  • Business Logic                                       │ │
│  │  • Repositories (Interfaces)                            │ │
│  └────────────────────────────────────────────────────────┘ │
│                            │                                 │
│                            ▼                                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │                   DATA LAYER                            │ │
│  │                                                         │ │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐ │ │
│  │  │ API Client  │  │ Local DB    │  │ Call Capture    │ │ │
│  │  │ (REST)      │  │ (SQLite)    │  │ (CallKit/       │ │ │
│  │  │             │  │             │  │  Telecom)       │ │ │
│  │  └─────────────┘  └─────────────┘  └─────────────────┘ │ │
│  │                                                         │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### 8.1 iOS Call Capture

```swift
// CallKit Integration
class CallDirectoryHandler: CXCallDirectoryProvider {
    // Identify incoming callers
    func addIdentificationEntries(to context: CXCallDirectoryExtensionContext) {
        // Match against imported contacts
    }
}

// Post-call hook
class CallObserver: CXCallObserverDelegate {
    func callObserver(_ observer: CXCallObserver, 
                      callChanged call: CXCall) {
        if call.hasEnded {
            // Trigger transcription upload
        }
    }
}
```

### 8.2 Android Call Capture

```kotlin
// Telecom Integration
class CallReceiver : BroadcastReceiver() {
    override fun onReceive(context: Context, intent: Intent) {
        when (intent.action) {
            TelephonyManager.ACTION_PHONE_STATE_CHANGED -> {
                // Track call state
            }
        }
    }
}
```

---

## 9. DevOps

### 9.1 CI/CD Pipeline

```
┌─────────────────────────────────────────────────────────────┐
│                      GitHub Actions                          │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌─────────┐    ┌─────────┐    ┌─────────┐    ┌─────────┐ │
│  │ Build   │───►│ Test    │───►│ Publish │───►│ Deploy  │ │
│  │         │    │         │    │         │    │         │ │
│  │ dotnet  │    │ Unit    │    │ Docker  │    │ ACA     │ │
│  │ build   │    │ E2E     │    │ Push    │    │ Bicep   │ │
│  └─────────┘    └─────────┘    └─────────┘    └─────────┘ │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### 9.2 Environments

| Environment | Purpose | URL |
|-------------|---------|-----|
| dev | Development | dev.aigents.au |
| staging | Pre-prod testing | staging.aigents.au |
| prod | Production | aigents.au |

---

## 10. Monitoring

### 10.1 Key Metrics

| Category | Metric | Alert Threshold |
|----------|--------|-----------------|
| Availability | Uptime | < 99.9% |
| Performance | API p95 latency | > 500ms |
| Errors | 5xx rate | > 1% |
| Business | Calls processed | < 100/hour |
| Business | Active agents | < baseline - 20% |

### 10.2 Dashboards

- **Operations**: Infrastructure health, costs
- **Product**: User engagement, feature usage
- **Business**: Leads captured, calls processed

---

*Document: ARCHITECTURE.md*
