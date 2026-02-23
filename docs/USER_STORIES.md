# Aigents User Stories & Customer Journeys

> **Version:** 1.0  
> **Last Updated:** 2025-12-11  
> **Platform:** aigents.au

---

## Executive Summary

Aigents is an AI-powered real estate platform that serves four distinct customer personas, each with unique pain points and goals. This document maps the complete user journey for each persona, from initial pain recognition through successful outcome.

---

## 🏠 Persona 1: Property Buyer

### Customer Profile

| Attribute | Description |
|-----------|-------------|
| **Who** | First-home buyers, investors, upgraders |
| **Age Range** | 25-55 |
| **Tech Comfort** | Medium to High |
| **Decision Timeline** | 3-12 months |

### The Pain

> *"I keep missing out on properties. By the time I find out about a listing, inspect it, and make an offer, it's already sold. I don't have time to attend every open home, and I feel like I'm always one step behind buyers who have more time or better connections."*

**Key Frustrations:**
1. **Information Asymmetry** — Agents know more than buyers; listings appear on portals days after going live
2. **Time Poverty** — Can't attend all open homes; properties sell before weekends
3. **Emotional Decision-Making** — FOMO leads to overpaying; missing genuine value
4. **Negotiation Disadvantage** — Agents negotiate daily; buyers negotiate once every 5-10 years
5. **Hidden Costs** — Unexpected issues post-purchase (pests, structural, legal)

### User Stories

#### US-BUY-001: Early Property Discovery
```
AS A property buyer
I WANT to be notified about new listings before they hit major portals
SO THAT I can be first to inspect and make an offer
```

**Acceptance Criteria:**
- [ ] AI monitors off-market channels, agency websites, and social media
- [ ] Push notification within 15 minutes of listing detection
- [ ] Match score (0-100) based on user preferences
- [ ] One-click "express interest" action

---

#### US-BUY-002: Automated Property Analysis
```
AS A property buyer
I WANT AI to analyze a property's true value and risks
SO THAT I can make informed offers without overpaying
```

**Acceptance Criteria:**
- [ ] Comparable sales analysis (last 6 months, 1km radius)
- [ ] Price prediction with confidence interval
- [ ] Risk flags (flood zone, flight path, planned developments)
- [ ] Historical price tracking for the property
- [ ] Rental yield estimation for investors

---

#### US-BUY-003: Virtual Open Home Attendance
```
AS A time-poor buyer
I WANT an AI agent to attend open homes on my behalf
SO THAT I only physically visit shortlisted properties
```

**Acceptance Criteria:**
- [ ] AI sends questions to listing agent pre/post inspection
- [ ] Receives and compiles inspection notes, photos, videos
- [ ] Generates summary report with pros/cons
- [ ] Flags discrepancies between listing and reality
- [ ] Schedules private inspection if property passes screening

---

#### US-BUY-004: AI-Powered Negotiation
```
AS A buyer uncomfortable with negotiation
I WANT AI to negotiate on my behalf
SO THAT I get the best possible price without emotional decision-making
```

**Acceptance Criteria:**
- [ ] AI drafts and submits offers based on valuation
- [ ] Counter-offer strategy recommendations
- [ ] Real-time market condition adjustments
- [ ] Walk-away price enforcement
- [ ] Settlement term negotiation assistance

---

#### US-BUY-005: Purchase Journey Tracking
```
AS A buyer under contract
I WANT to track all milestones and deadlines
SO THAT I don't miss critical dates and the purchase completes smoothly
```

**Acceptance Criteria:**
- [ ] Timeline visualization with key dates
- [ ] Automated reminders (finance, building inspection, settlement)
- [ ] Document checklist with upload capability
- [ ] Conveyancer/solicitor integration
- [ ] Settlement countdown with task completion status

---

### Success Metrics

| Metric | Target |
|--------|--------|
| Properties found before portal listing | 40% |
| Average savings vs. asking price | $15,000+ |
| Time from search start to purchase | -30% |
| User NPS | 70+ |

---

## 💰 Persona 2: Property Seller

### Customer Profile

| Attribute | Description |
|-----------|-------------|
| **Who** | Homeowners, investors, downsizers |
| **Age Range** | 35-70 |
| **Tech Comfort** | Low to Medium |
| **Primary Motivation** | Maximize sale price, minimize hassle |

### The Pain

> *"I'm paying $25,000-$50,000 in commission for what exactly? An agent who lists my property on the same portals I could use myself, then tells me to accept a lower offer. I'm doing most of the work keeping the house inspection-ready anyway."*

**Key Frustrations:**
1. **Excessive Commissions** — 2-3% of sale price for service of diminishing value
2. **Lack of Transparency** — Unclear marketing spend, buyer feedback, campaign performance
3. **Misaligned Incentives** — Agents want quick sales; sellers want maximum price
4. **Inspection Fatigue** — Keeping house "show-ready" for weeks/months
5. **Vendor Paid Advertising** — $5-15k marketing costs regardless of outcome

### User Stories

#### US-SELL-001: Free Property Valuation
```
AS A potential seller
I WANT an instant, accurate valuation of my property
SO THAT I can decide whether to sell and at what price
```

**Acceptance Criteria:**
- [ ] Valuation within 60 seconds using property address
- [ ] Comparable sales breakdown
- [ ] Market condition commentary
- [ ] Best time to sell recommendation
- [ ] No personal data required for initial estimate

---

#### US-SELL-002: Commission-Free Listing
```
AS A property seller
I WANT to list my property without paying traditional agent commissions
SO THAT I keep more of my sale proceeds
```

**Acceptance Criteria:**
- [ ] Flat-fee or success-based pricing model
- [ ] Professional photography arrangement
- [ ] Listing syndication to major portals (REA, Domain)
- [ ] "For Sale" signboard options
- [ ] Legal document templates

---

#### US-SELL-003: AI Buyer Qualification
```
AS A seller
I WANT AI to screen and qualify potential buyers
SO THAT I only deal with serious, financially capable buyers
```

**Acceptance Criteria:**
- [ ] Pre-approval verification prompts
- [ ] Buyer timeline and motivation assessment
- [ ] Automated response to inquiry messages
- [ ] Lead scoring (hot, warm, cold)
- [ ] Spam/time-waster filtering

---

#### US-SELL-004: Intelligent Offer Negotiation
```
AS A seller receiving offers
I WANT AI to analyze offers and negotiate on my behalf
SO THAT I achieve the maximum sale price
```

**Acceptance Criteria:**
- [ ] Offer comparison dashboard
- [ ] Counter-offer recommendations with justification
- [ ] Market demand indicator (multiple interested parties)
- [ ] Settlement term optimization
- [ ] Final approval remains with seller

---

#### US-SELL-005: Inspection Scheduling Automation
```
AS A seller
I WANT inspections scheduled efficiently around my availability
SO THAT I minimize disruption to my daily life
```

**Acceptance Criteria:**
- [ ] Calendar integration (Google, Outlook)
- [ ] Buyer self-booking within available slots
- [ ] Automated confirmations and reminders
- [ ] Post-inspection feedback collection
- [ ] Virtual tour option for interstate/overseas buyers

---

### Success Metrics

| Metric | Target |
|--------|--------|
| Average commission savings | $20,000+ |
| Days on market | Equal to or better than traditional agents |
| Sale price vs. AI valuation | 98-103% |
| Seller satisfaction | 4.5+/5 stars |

---

## 🏢 Persona 3: Tenant (Renter)

### Customer Profile

| Attribute | Description |
|-----------|-------------|
| **Who** | Young professionals, families, students, relocators |
| **Age Range** | 18-45 |
| **Tech Comfort** | High |
| **Urgency** | Often high (lease ending, job relocation) |

### The Pain

> *"I applied for 27 properties before getting approved. The application process is exhausting — uploading the same documents over and over, writing cover letters, and competing against 50 other applicants. Having a pet makes it nearly impossible. I never hear back; I just see it leased to someone else."*

**Key Frustrations:**
1. **Application Fatigue** — Repetitive data entry across multiple applications
2. **Low Approval Rates** — High competition, especially in desirable areas
3. **Pet Discrimination** — 80%+ of listings are "no pets" despite pet-friendly landlords
4. **Lack of Feedback** — No visibility into why applications are rejected
5. **Income Requirements** — Strict rent-to-income ratios exclude many qualified tenants

### User Stories

#### US-RENT-001: One-Click Application
```
AS A renter
I WANT to apply for properties with one click using my pre-verified profile
SO THAT I can apply quickly and compete effectively
```

**Acceptance Criteria:**
- [ ] Profile stores income verification, references, ID
- [ ] One-click application submission
- [ ] Auto-fill for any additional property manager forms
- [ ] Application tracking dashboard
- [ ] Mobile-optimized experience

---

#### US-RENT-002: Pre-Verification Badge
```
AS A renter
I WANT to be pre-verified as a quality tenant
SO THAT my applications stand out and get prioritized
```

**Acceptance Criteria:**
- [ ] Income verification (payslips, employment letter, bank statements)
- [ ] Identity verification (100-point check)
- [ ] Reference pre-collection and verification
- [ ] Credit check option
- [ ] "Aigents Verified" badge on applications

---

#### US-RENT-003: AI Pet Resume
```
AS A pet owner
I WANT to create an AI-generated pet resume
SO THAT I can demonstrate my pet is well-behaved and low-risk
```

**Acceptance Criteria:**
- [ ] Pet profile with photos, breed, age, temperament
- [ ] Vaccination and registration records
- [ ] Previous landlord pet references
- [ ] Pet insurance/bond offer
- [ ] Behavioral certifications (if applicable)

---

#### US-RENT-004: Smart Property Matching
```
AS A renter
I WANT AI to recommend properties that match my needs AND where I'm likely to be approved
SO THAT I don't waste time on applications that will fail
```

**Acceptance Criteria:**
- [ ] Match score based on preferences and approval likelihood
- [ ] Price negotiability indicator
- [ ] Pet-friendly probability score
- [ ] Application volume indicator (competition level)
- [ ] "Best chance" property highlights

---

#### US-RENT-005: Application Status Transparency
```
AS an applicant
I WANT real-time visibility into my application status
SO THAT I can make informed decisions about other properties
```

**Acceptance Criteria:**
- [ ] Status updates: Submitted → Viewed → Shortlisted → Approved/Declined
- [ ] Estimated response time
- [ ] Position in queue (if available)
- [ ] Suggestions if declined (alternative properties)
- [ ] Feedback request option

---

### Success Metrics

| Metric | Target |
|--------|--------|
| Time to complete application | < 60 seconds |
| Application approval rate | 35%+ (vs. 15% industry average) |
| Pet owner approval rate | 50%+ |
| Average applications before approval | 3 (vs. 15+ industry average) |

---

## 🤝 Persona 4: Real Estate Agent

### Customer Profile

| Attribute | Description |
|-----------|-------------|
| **Who** | Licensed real estate agents, agency principals |
| **Age Range** | 25-60 |
| **Tech Comfort** | Medium |
| **Primary Motivation** | Close more deals, scale operations, reduce admin |

### The Pain

> *"I spend 60% of my time on admin — answering the same questions, scheduling inspections, chasing paperwork. Only 40% actually generates revenue. I can physically only do so many inspections per week, which caps my income. I need to clone myself."*

> *"My phone is my business. I get 50 calls a day and forget half of them by the time I'm back at my desk. Leads slip through the cracks. I know I'm sitting on a goldmine of contacts but I never have time to properly follow up."*

**Key Frustrations:**
1. **Admin Overload** — Repetitive tasks consume selling time
2. **After-Hours Inquiries** — Best leads come at night; slow response = lost lead
3. **Inspection Capacity** — Physical limit on properties per day
4. **Inconsistent Follow-Up** — Leads fall through cracks during busy periods
5. **CRM Data Entry** — Manual logging is time-consuming and often skipped
6. **Phone Call Chaos** — No record of conversations, forgotten commitments
7. **Untapped Contact Base** — Years of contacts sitting dormant in phone
8. **Lost Inspection Attendees** — Sign-in sheets get lost; no digital capture

### User Stories

#### US-AGENT-001: 24/7 AI Lead Response
```
AS A real estate agent
I WANT AI to instantly respond to all inquiries
SO THAT I never lose a lead due to slow response time
```

**Acceptance Criteria:**
- [ ] Instant response to portal inquiries (REA, Domain)
- [ ] Intelligent Q&A about property features
- [ ] Lead qualification questions embedded in conversation
- [ ] Inspection booking prompts
- [ ] Seamless handoff to human agent when needed

---

#### US-AGENT-002: Automated Inspection Scheduling
```
AS an agent
I WANT AI to handle all inspection scheduling
SO THAT I can focus on higher-value activities
```

**Acceptance Criteria:**
- [ ] Calendar integration with automatic slot generation
- [ ] Buyer self-booking with confirmation
- [ ] Reminder sequence (24hr, 2hr before)
- [ ] No-show tracking and follow-up
- [ ] Open home RSVPs and headcount prediction

---

#### US-AGENT-003: AI Sales Assistant
```
AS an agent
I WANT an AI assistant that handles routine communications
SO THAT every buyer and seller feels attended to
```

**Acceptance Criteria:**
- [ ] Post-inspection feedback requests
- [ ] Comparable sales sharing
- [ ] Campaign update generation for vendors
- [ ] Offer receipt acknowledgment
- [ ] Settlement milestone communications

---

#### US-AGENT-004: CRM Auto-Sync
```
AS an agent
I WANT all AI interactions automatically logged to my CRM
SO THAT I have complete client history without manual data entry
```

**Acceptance Criteria:**
- [ ] Integration with major CRMs (VaultRE, AgentBox, etc.)
- [ ] Contact creation/update automation
- [ ] Interaction timeline logging
- [ ] Task creation based on AI conversations
- [ ] Deal stage advancement recommendations

---

#### US-AGENT-005: Performance Analytics
```
AS an agent/principal
I WANT visibility into AI performance and lead conversion
SO THAT I can optimize my campaigns and justify the investment
```

**Acceptance Criteria:**
- [ ] Lead response time metrics
- [ ] Conversion funnel visualization
- [ ] AI vs. human response comparison
- [ ] ROI calculator
- [ ] White-label reporting for vendors

---

#### US-AGENT-006: Phone Contact Import & Activation
```
AS an agent with years of contacts in my phone
I WANT to import my existing contacts and have AI identify opportunities
SO THAT I can reactivate dormant leads and uncover hidden pipeline
```

**Acceptance Criteria:**
- [ ] One-click phone contact sync (iOS/Android)
- [ ] AI classification: Buyer, Seller, Investor, Vendor, Other Agent
- [ ] Last interaction date detection
- [ ] "Dormant lead" identification (no contact >6 months)
- [ ] Suggested re-engagement message generation
- [ ] Property interest inference from historical data
- [ ] Privacy-compliant opt-out handling

**Technical Considerations:**
- Contact permissions must be clearly explained
- Data stored securely with encryption
- User controls which contacts are synced
- GDPR/Privacy Act compliance

---

#### US-AGENT-007: Call Transcription & Intelligence
```
AS an agent who takes 50+ calls per day
I WANT my business calls transcribed and analyzed by AI
SO THAT I never forget a commitment and all leads are captured
```

**Acceptance Criteria:**
- [ ] Call recording activation (with consent disclaimer)
- [ ] Real-time transcription of agent's side of conversation
- [ ] Post-call summary generation
- [ ] Action item extraction (callbacks, inspections, offers)
- [ ] Property mention detection and linking
- [ ] Sentiment analysis (buyer hot/cold)
- [ ] Auto-creation of follow-up tasks
- [ ] Searchable call history

**Privacy & Compliance:**
- [ ] Clear consent mechanism before recording
- [ ] Option for "listen-only" mode (captures agent's voice only)
- [ ] Australian Privacy Act compliant
- [ ] State-specific call recording laws addressed
- [ ] Easy deletion of recordings on request

---

#### US-AGENT-008: Conversation-to-Property Matching
```
AS an agent
I WANT AI to automatically identify which property a caller is asking about
SO THAT I have instant context without asking "which property?"
```

**Acceptance Criteria:**
- [ ] Address/suburb mention detection in call
- [ ] Caller ID cross-reference with inspection attendee lists
- [ ] Recent inquiry matching (caller asked about X yesterday)
- [ ] Property suggestion when multiple matches exist
- [ ] Integration with listing database for instant property card
- [ ] "Most likely property" confidence score
- [ ] Quick-select to associate call with correct property

---

#### US-AGENT-009: Digital Inspection Check-In
```
AS an agent running open homes
I WANT attendees to check in digitally via QR code
SO THAT I capture every lead without paper sign-in sheets
```

**Acceptance Criteria:**
- [ ] Unique QR code generated per property/inspection
- [ ] Mobile-friendly check-in form
- [ ] Pre-filled fields for returning visitors
- [ ] Pre-qualification questions (pre-approved? timeline? budget?)
- [ ] Instant lead notification to agent
- [ ] Automatic follow-up message post-inspection
- [ ] Integration with call intelligence (link caller to attendee)
- [ ] Attendee history across multiple properties

---

#### US-AGENT-010: Mobile Companion App
```
AS an agent always on the road
I WANT a mobile app that's my AI co-pilot
SO THAT I can access lead intelligence anywhere, anytime
```

**Acceptance Criteria:**
- [ ] Dashboard with today's inspections and hot leads
- [ ] Push notifications for new inquiries/offers
- [ ] Quick-dial with auto-call logging
- [ ] Voice note capture with transcription
- [ ] Property lookup by address
- [ ] Contact profile with full interaction history
- [ ] Offline mode for poor reception areas
- [ ] Apple Watch / Android Wear quick glance

---

### Agent Data Capture Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    AGENT'S EXISTING NETWORK                  │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  📱 Phone Contacts ──────┐                                  │
│                          │                                   │
│  📞 Call Recordings ─────┼──▶  🤖 AI INTELLIGENCE ENGINE    │
│                          │         │                         │
│  🏠 Inspection Sign-ins ─┘         │                         │
│                                    ▼                         │
│                          ┌─────────────────┐                │
│                          │  UNIFIED CRM    │                │
│                          │  - Lead scoring │                │
│                          │  - Property map │                │
│                          │  - Action items │                │
│                          │  - Follow-ups   │                │
│                          └─────────────────┘                │
│                                    │                         │
│                                    ▼                         │
│                          📊 AGENT DASHBOARD                  │
│                          "You have 12 hot leads today"       │
└─────────────────────────────────────────────────────────────┘
```

---

### Success Metrics

| Metric | Target |
|--------|--------|
| Lead response time | < 2 minutes (24/7) |
| Admin time reduction | 50%+ |
| Listings managed per agent | 2x current capacity |
| Lead-to-inspection conversion | +25% |
| Contacts imported per agent | 500+ |
| Dormant leads reactivated | 20% conversion to active |
| Calls captured and logged | 95%+ |
| Inspection attendee capture rate | 90%+ (vs. 60% paper) |

---

## 📊 Feature-to-Persona Matrix

| Feature | Buyer | Seller | Tenant | Agent |
|---------|:-----:|:------:|:------:|:-----:|
| AI Property Matching | ✅ | — | ✅ | — |
| Instant Valuation | ✅ | ✅ | — | ✅ |
| Smart Negotiation | ✅ | ✅ | — | ✅ |
| One-Click Application | — | — | ✅ | — |
| Pre-Verification | ✅ | — | ✅ | — |
| Pet Resume | — | — | ✅ | — |
| 24/7 Inquiry Response | — | ✅ | — | ✅ |
| Inspection Scheduling | ✅ | ✅ | ✅ | ✅ |
| CRM Integration | — | — | — | ✅ |
| Document Management | ✅ | ✅ | ✅ | ✅ |
| Settlement Tracking | ✅ | ✅ | — | ✅ |
| **Phone Contact Import** | — | — | — | ✅ |
| **Call Transcription & AI** | — | — | — | ✅ |
| **Conversation-Property Match** | — | — | — | ✅ |
| **Digital Inspection Check-in** | — | — | — | ✅ |
| **Mobile Companion App** | — | — | — | ✅ |

---

## 🚀 Implementation Priority

### Phase 1: MVP (Now)
1. Property search with AI matching (Buyer)
2. Commission-free listing flow (Seller)
3. One-click rental application (Tenant)
4. 24/7 lead response (Agent)
5. **Digital Inspection Check-in QR (Agent)**

### Phase 2: Growth (Q1 2025)
1. AI negotiation for buyers and sellers
2. Pre-verification badges
3. Pet resume generator
4. CRM integrations
5. **Phone Contact Import & Activation (Agent)**
6. **Call Transcription & Intelligence (Agent)**
7. **Conversation-to-Property Matching (Agent)**

### Phase 3: Scale (Q2 2025)
1. Virtual inspection attendance
2. Predictive market analytics
3. White-label agent platform
4. Settlement automation
5. **Mobile Companion App (Agent)**
6. **Apple Watch / Wearable Integration (Agent)**

---

## Appendix: Competitive Positioning

| Pain Point | Traditional Agent | For Sale By Owner | Aigents |
|------------|------------------|-------------------|---------|
| Commission | 2-3% ($15-50k) | $0 | Flat fee or success-based |
| Negotiation Expertise | ✅ Agent-led | ❌ DIY | ✅ AI-assisted |
| 24/7 Availability | ❌ | ❌ | ✅ |
| Data-Driven Pricing | ⚠️ Variable | ❌ | ✅ |
| Buyer Qualification | ⚠️ Manual | ❌ | ✅ Automated |
| Legal Support | ⚠️ Varies | ❌ | ✅ Templates + referrals |

---

*This document is a living artifact and should be updated as user research reveals new insights.*
