# Australian Real Estate CRM Landscape
## Integration Research & Strategy

> **Version:** 1.0  
> **Last Updated:** 2025-12-11  
> **Purpose:** Map CRM systems used by major agencies for Aigents integration

---

## 1. Executive Summary

The Australian real estate CRM market is dominated by **5-6 major players**, each with open APIs. To achieve widespread agent adoption, Aigents must integrate with the top 4 CRMs which cover approximately **80%+ of the market**.

### Priority Integration Targets

| Priority | CRM | Market Position | API Available |
|----------|-----|-----------------|---------------|
| 🔥 P1 | **Rex** | Market leader, AU/NZ/UK | ✅ Open API |
| 🔥 P1 | **AgentBox** (Reapit) | Most widely used in AU | ✅ Open API |
| 🔴 P2 | **VaultRE** (MRI Vault) | Ray White, large networks | ✅ Open API |
| 🔴 P2 | **MyDesktop** (Domain) | Legacy, many offices | ✅ API |
| 🟡 P3 | **Box+Dice** (MRI) | Large franchises | ✅ API |
| 🟡 P3 | **PropertyMe** | Property Management | ✅ API |

---

## 2. CRM Systems by Agency

### Major Franchise Networks

| Agency | Primary CRM | Notes |
|--------|-------------|-------|
| **Ray White** | VaultRE + NurtureCloud | Custom propensity AI (2023) |
| **LJ Hooker** | Rex + Rex Reach | Digital marketing integration |
| **Place** | Proprietary PLACE Platform | All-in-one, AI-powered |
| **McGrath** | Rex | Cloud-based |
| **Barry Plant** | AgentBox | Victoria leader |
| **Harcourts** | VaultRE / Rex (varies) | Mixed by franchise |
| **Century 21** | AgentBox / Rex | Varies by office |
| **PRDnationwide** | Rex / AgentBox | Mixed |
| **Raine & Horne** | MyDesktop / AgentBox | Legacy + modern |
| **Belle Property** | Rex | Premium segment |

---

## 3. Detailed CRM Analysis

### 3.1 Rex CRM

**Website:** rexsoftware.com  
**HQ:** Brisbane, Australia  
**Markets:** Australia, New Zealand, UK

#### Features
- Cloud-based sales & property management
- Rex Reach (automated digital marketing)
- Rex Websites (agency websites)
- Mobile app (90% desktop functionality)
- Zapier integration

#### API Details

| Aspect | Details |
|--------|---------|
| **Type** | REST API |
| **Auth** | Token-based (login credentials) |
| **Format** | JSON |
| **Docs** | api.rexsoftware.com/docs |
| **Rate Limits** | Yes (unspecified) |
| **Webhooks** | Yes |

#### Key Endpoints
```
GET /contacts          - List contacts
POST /contacts         - Create contact
PUT /contacts/{id}     - Update contact
GET /listings          - List properties
GET /listings/{id}     - Property details
POST /activities       - Log interaction
GET /inspections       - Upcoming inspections
```

#### Integration Value for Aigents
- ✅ Sync contacts bi-directionally
- ✅ Create tasks/follow-ups from call AI
- ✅ Log call summaries as activities
- ✅ Link calls to properties
- ✅ Trigger marketing via Rex Reach

---

### 3.2 AgentBox (Reapit Sales)

**Website:** agentbox.com.au  
**Parent Company:** Reapit (since 2021)  
**Markets:** Australia (dominant)

#### Features
- Zero-duplicate contact management
- Portal inquiry import (REA, Domain)
- CoreLogic RP Data integration
- Console Cloud integration (property management)
- Mobile app

#### API Details

| Aspect | Details |
|--------|---------|
| **Type** | REST API |
| **Auth** | Client ID + API Key |
| **Format** | JSON |
| **Docs** | developer.reapit.cloud |
| **Sandbox** | Yes (demo data) |
| **AppMarket** | Yes (distribution channel) |

#### Key Endpoints
```
GET /contacts          - List contacts
POST /contacts         - Create contact
GET /properties        - List properties
POST /enquiries        - Log inquiry
GET /appointments      - Calendar
POST /notes            - Add notes
```

#### Integration Value for Aigents
- ✅ Import contacts on onboarding
- ✅ Log call transcripts as notes
- ✅ Create appointments from AI scheduling
- ✅ Push inspection check-ins as enquiries
- ✅ List on Reapit AppMarket for discovery

---

### 3.3 VaultRE (MRI Vault CRM)

**Website:** vaultre.com.au  
**Parent Company:** MRI Software  
**Markets:** Australia, NZ

#### Features
- Multi-office management
- Trust accounting & invoicing
- 380+ PropTech integrations
- Mobile app (iOS/Android, offline capable)
- Kiosk/check-in functions

#### API Details

| Aspect | Details |
|--------|---------|
| **Type** | REST API |
| **Auth** | OAuth 2.0 |
| **Format** | JSON |
| **Docs** | developers.vaultre.com |
| **Webhooks** | Yes |
| **Rate Limits** | Yes |
| **GitHub** | Sample code available |

#### Key Endpoints
```
GET /contacts             - List contacts
POST /contacts            - Create contact
GET /properties           - Properties
POST /inquiries           - Capture leads
GET /appointments         - Calendar
POST /contact-notes       - Add notes
GET /staff                - Agent profiles
```

#### Integration Value for Aigents
- ✅ Webhook for new inquiry → instant AI response
- ✅ Push call summaries to contact timeline
- ✅ Sync inspection attendees as contacts
- ✅ Ray White NurtureCloud data enrichment

---

### 3.4 MyDesktop (Domain)

**Website:** mydesktop.com.au  
**Parent Company:** Domain Holdings  
**Markets:** Australia

#### Features
- Domain/REA portal integration
- 300+ software integrations
- REAXML feeds
- Trust accounting
- Legacy but widely deployed

#### API Details

| Aspect | Details |
|--------|---------|
| **Type** | REST API + REAXML |
| **Auth** | API Key |
| **Format** | JSON / XML |
| **Docs** | developer.domain.com.au |
| **Domain API** | Unified listings API |

#### Key Endpoints
```
GET /agents              - Agent profiles
GET /listings            - Property listings
POST /enquiries          - Capture leads
GET /contacts            - CRM contacts
POST /activities         - Log activities
```

#### Integration Value for Aigents
- ✅ LeadScope integration (Domain product)
- ✅ Direct inquiry capture
- ✅ Activity logging

---

### 3.5 MRI Box+Dice

**Website:** boxdice.com.au  
**Parent Company:** MRI Software  
**Markets:** Australia

#### Features
- Designed for large multi-office networks
- Franchise management
- Centralized client database
- Marketing automation

#### API Details
- REST API available
- OAuth 2.0 authentication
- JSON format
- Documentation via MRI developer portal

---

### 3.6 PropertyMe (Property Management)

**Website:** propertyme.com.au  
**Markets:** Australia, NZ  
**Reach:** 6,200+ agencies, ~50% of AU property managers

#### Features
- Trust accounting (state-specific)
- Tenant/landlord portals
- Maintenance management
- Phoenix CRM add-on

#### API Details
- REST API
- Webhook support
- Integrates with sales CRMs

---

## 4. Proprietary Systems

Some major agencies have built custom platforms:

### Place Platform
- AI-powered all-in-one system
- Custom CRM, marketing, transaction management
- No public API
- **Strategy:** Partner discussion required

### Ray White NurtureCloud
- Propensity modeling + AI
- Seller/buyer identification
- Built on VaultRE data
- **Strategy:** Integrate via VaultRE API

---

## 5. Integration Architecture

### 5.1 Data Flow

```
┌─────────────────────────────────────────────────────────────┐
│                     AIGENTS PLATFORM                         │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │               INTEGRATION HUB                          │ │
│  │                                                        │ │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ │ │
│  │  │  Rex     │ │ AgentBox │ │ VaultRE  │ │ MyDesktop│ │ │
│  │  │ Adapter  │ │ Adapter  │ │ Adapter  │ │ Adapter  │ │ │
│  │  └────┬─────┘ └────┬─────┘ └────┬─────┘ └────┬─────┘ │ │
│  │       │            │            │            │       │ │
│  └───────┼────────────┼────────────┼────────────┼───────┘ │
│          │            │            │            │          │
│          ▼            ▼            ▼            ▼          │
│  ┌──────────────────────────────────────────────────────┐ │
│  │            NORMALIZED DATA MODEL                      │ │
│  │                                                       │ │
│  │  Contact { id, name, phone, email, source, ... }     │ │
│  │  Property { id, address, price, type, ... }          │ │
│  │  Interaction { type, timestamp, summary, ... }       │ │
│  │  Task { description, dueDate, priority, ... }        │ │
│  │                                                       │ │
│  └──────────────────────────────────────────────────────┘ │
│                                                              │
└─────────────────────────────────────────────────────────────┘
                              │
                              │ Webhooks / Polling
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                    EXTERNAL CRMs                             │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐       │
│  │   Rex    │ │ AgentBox │ │ VaultRE  │ │ MyDesktop│       │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘       │
└─────────────────────────────────────────────────────────────┘
```

### 5.2 Sync Patterns

| Pattern | Use Case | Direction |
|---------|----------|-----------|
| **Initial Import** | Onboarding - pull all contacts | CRM → Aigents |
| **Real-time Webhook** | New inquiry arrives | CRM → Aigents |
| **Activity Push** | Call completed, log to CRM | Aigents → CRM |
| **Task Creation** | AI action item → CRM task | Aigents → CRM |
| **Contact Update** | Lead score change, new notes | Aigents → CRM |
| **Periodic Sync** | Reconciliation (hourly) | Bidirectional |

### 5.3 Field Mapping Example

| Aigents Field | Rex | AgentBox | VaultRE |
|---------------|-----|----------|---------|
| name | name | fullName | displayName |
| phone | phone.mobile | mobilePhone | contactMobile |
| email | email.primary | emailAddress | email |
| source | source | leadSource | enquirySource |
| leadScore | customFields.score | tags | customField1 |

---

## 6. Implementation Roadmap

### Phase 1: Core Integrations (Q1 2025)

| CRM | Scope | Effort |
|-----|-------|--------|
| Rex | Full bidirectional sync | 4 weeks |
| AgentBox | Full bidirectional sync | 4 weeks |
| VaultRE | Full bidirectional sync | 3 weeks |
| MyDesktop | Read + activity logging | 2 weeks |

### Phase 2: Enhanced Features (Q2 2025)

- Webhook-based real-time sync
- AppMarket listings (Reapit, VaultRE)
- Conflict resolution logic
- Bulk import optimization

### Phase 3: Expansion (Q3 2025)

- Box+Dice integration
- PropertyMe (PM sector)
- Zapier connector (catch-all)
- HubSpot/Salesforce (enterprise)

---

## 7. API Access Requirements

### Developer Registration

| CRM | Registration URL | Approval Time |
|-----|-----------------|---------------|
| Rex | api.rexsoftware.com | 1-2 weeks |
| AgentBox | developer.reapit.cloud | 1-2 weeks |
| VaultRE | developers.vaultre.com | 1-2 weeks |
| Domain | developer.domain.com.au | 1-2 weeks |

### Typical Requirements
- Company ABN
- Product description
- Security questionnaire
- Test environment demo
- Privacy policy review

---

## 8. Competitive Intelligence

### CRM Vendor AI Features

| Vendor | AI Features | Threat Level |
|--------|-------------|--------------|
| **NurtureCloud** (Ray White) | Propensity modeling, seller prediction | 🔴 High |
| **Rex Reach** | Automated ad targeting | 🟡 Medium |
| **AgentBox** | Duplicate detection | 🟢 Low |
| **VaultRE** | Basic automation | 🟢 Low |

### Positioning Strategy

Aigents differentiates by offering:
1. **Call Intelligence** - CRMs don't transcribe calls
2. **Cross-CRM** - Works with any CRM, not locked in
3. **Mobile-First** - Better UX than CRM mobile apps
4. **AI Depth** - Beyond simple automation

---

## 9. Data Privacy Considerations

| Requirement | Implementation |
|-------------|----------------|
| Agent Consent | OAuth flow grants access to their data |
| Contact Consent | Privacy notice on check-in forms |
| Data Residency | AU data centers (Azure Australia East) |
| Right to Delete | Cascade delete across systems |
| Audit Trail | Log all CRM read/write operations |

---

## 10. Next Steps

1. [ ] Register as developer with Rex, AgentBox, VaultRE
2. [ ] Obtain sandbox API credentials
3. [ ] Build integration adapters
4. [ ] Test with pilot agents
5. [ ] Submit to AppMarkets for distribution
6. [ ] Initiate Place partnership discussions

---

*Document: CRM_INTEGRATION_RESEARCH.md*
