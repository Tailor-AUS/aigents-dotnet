---
description: Maintain System Pulse (Debug & Diagnostics)
---

# System Pulse Check

This workflow is designed to provide an instant "health check" of the Aigents application, specifically focusing on the frontend state, console errors, and critical feature functionality (Map, Wizard, etc.).

## 1. Diagnostics Routine

When asked to "check the pulse" or when diagnosing issues proactively:

1.  **Launch Browser Agent**:
    *   Navigate to `http://localhost:5000/list` (or relevant page).
    *   Open DevTools (simulate by capturing console logs).
    *   Perform specific interactions (e.g., enter address, click buttons).

2.  **Capture Vital Signs (Console & Visuals)**:
    *   **Console Errors**: Look for red text, specifically 401/403 (API Auth), 404 (Missing Resources), or JS Exceptions.
    *   **Visual State**: Capture screenshots of key components (Map, Form, Results).
    *   **Network Health**: Check for failed API calls (Google Maps, Backend Endpoints).

3.  **Analyze & Report**:
    *   Compare expected behavior vs. actual.
    *   Identify "Blockers" (e.g., Map black screen).
    *   Identify "Warnings" (e.g., Slow load times).

## 2. Common Issues & Fixes

*   **Google Maps Black Screen**:
    *   *Symptom*: Black box where map should be.
    *   *Check*: API Key restrictions (Maps JS, Places, Street View).
    *   *Check*: CSS height/width (is it 0px?).
    *   *Check*: JS Initialization errors.

*   **"Magic Loading" Stuck**:
    *   *Symptom*: Spinner never stops.
    *   *Check*: JS Interop failure (C# waiting for JS or vice versa).
    *   *Check*: Backend timeout.

## 3. Persistent Logging (Future Implementation)

*   *Goal*: Ship logs to a local file or temporary endpoint for distinct observation.
*   *Current*: Rely on Browser Console content for frontend.

---
**Status**: ACTIVE
**Last Updated**: 2025-12-19
