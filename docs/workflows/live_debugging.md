---
description: Live Debugging & Log Monitoring
---

# Live Feed Debugging System

We have established a centralized logging system that pipes Frontend (Browser) logs directly to the Backend (Terminal) output. This allows for "Live Pulse" monitoring of the application state without needing to open the browser devtools.

## 1. How it Works

1.  **Client-Side Capture**:
    *   Script injected in `App.razor` overrides `console.error` and `console.log`.
    *   Buffers and sends critical logs (Errors, Map status, Orbit Logic) to `/api/clientsidelog`.
    *   Throttled to 10 logs/sec to prevent flooding.

2.  **Server-Side Ingestion**:
    *   Endpoint `/api/clientsidelog` receives the logs.
    *   Prints them to the active `dotnet run` terminal with color coding:
        *   **[CLIENT-ERROR]** (Red): Critical failures (JS exceptions, API blockers).
        *   **[CLIENT-INFO]** (Cyan): State changes (Map init, Cinematic View start).

## 2. Monitoring the Pulse

To check the system health, simply read the terminal output of the running `dotnet run` command.

**Look for:**
*   `[CLIENT-INFO] Initializing Cinematic Street View` -> Success path started.
*   `[CLIENT-INFO] Street View not found. Fallback to Satellite Orbit.` -> expected fallback behavior.
*   `[CLIENT-ERROR] ... 403 (Forbidden)` -> Maps API Key issue.

## 3. Reporting Issues

When reporting an issue, provide the **Approximate Time** (e.g., "12:47 PM"). We will cross-reference this with the `Timestamp` field in the logs.

**Example Report:**
> "At 12:48 PM, the map went black."

**Action:**
We check logs at `12:48:00` for `[CLIENT-ERROR]` entries.
