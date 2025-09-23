# Auto-Logout AJAX Activity Tracking - Implementation Plan

## Executive Summary
Implement a solution that tracks ALL user activity (including AJAX/API calls) and properly synchronizes session state between server and client, without modifying external applications.

**Key Decision: HttpContext.Session is NOT needed** - We'll use response headers to communicate session state directly.

## Phase 1: Create Activity Tracking Middleware

### 1.1 Create `SessionActivityTrackingMiddleware.cs`
Location: `cloudscribe.Core.Web/Middleware/SessionActivityTrackingMiddleware.cs`

**CRITICAL:** We must NOT call `AuthenticateAsync()` as it will extend the session. Instead, we'll calculate the new expiry time based on the sliding window configuration.

```csharp
public class SessionActivityTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SessionActivityTrackingMiddleware> _logger;
    private readonly IOptionsMonitor<CookieAuthenticationOptions> _cookieOptions;
    private readonly ISiteContextResolver _siteResolver;

    public SessionActivityTrackingMiddleware(
        RequestDelegate next, 
        ILogger<SessionActivityTrackingMiddleware> logger,
        IOptionsMonitor<CookieAuthenticationOptions> cookieOptions,
        ISiteContextResolver siteResolver)
    {
        _next = next;
        _logger = logger;
        _cookieOptions = cookieOptions;
        _siteResolver = siteResolver;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture authentication state BEFORE processing request
        // This uses the already-authenticated principal without re-authenticating
        var wasAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;
        DateTimeOffset? calculatedExpiry = null;
        
        if (wasAuthenticated)
        {
            // Calculate what the new expiry WILL BE after this request
            // based on the sliding window configuration
            var site = await _siteResolver.ResolveSite(context.Request.Host.Host, context.Request.Path);
            if (site != null && !string.IsNullOrEmpty(site.MaximumInactivityInMinutes))
            {
                if (double.TryParse(site.MaximumInactivityInMinutes, out var minutes))
                {
                    // The cookie WILL be extended to this time after this request completes
                    calculatedExpiry = DateTimeOffset.UtcNow.AddMinutes(minutes);
                }
            }
        }
        
        await _next(context);

        // After request processing, add headers with the calculated expiry
        // Only for successful authenticated requests
        if (wasAuthenticated && 
            calculatedExpiry.HasValue &&
            context.Response.StatusCode >= 200 && 
            context.Response.StatusCode < 300)
        {
            try
            {
                var remainingSeconds = (calculatedExpiry.Value - DateTimeOffset.UtcNow).TotalSeconds;
                
                // Add custom headers with session info
                context.Response.Headers["X-Session-Remaining"] = Math.Round(remainingSeconds).ToString();
                context.Response.Headers["X-Session-Expires"] = calculatedExpiry.Value.ToUnixTimeSeconds().ToString();
                
                _logger.LogDebug($"Session activity tracked. Remaining: {remainingSeconds}s");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding session headers");
                // Don't break the request, just log and continue
            }
        }
    }
}
```

### 1.2 Register Middleware in `Program.cs` or `Startup.cs`
```csharp
// Add AFTER authentication middleware but BEFORE MVC/Razor Pages
app.UseAuthentication();
app.UseMiddleware<SessionActivityTrackingMiddleware>();
app.UseAuthorization();
```

## Phase 2: Modify JavaScript to Use Response Headers

### 2.1 Update `cloudscribe-autologout-warning.js`

Replace the existing countdown logic with a more intelligent system:

```javascript
// Global state for session management
let sessionState = {
    expiresAt: null,  // Unix timestamp when session expires
    lastActivity: Date.now(),
    warningShown: false,
    alertThreshold: 60 // seconds before expiry to show warning
};

// Function to update session state from server response
function updateSessionFromResponse(xhr) {
    const remaining = xhr.getResponseHeader('X-Session-Remaining');
    const expires = xhr.getResponseHeader('X-Session-Expires');
    
    if (expires) {
        sessionState.expiresAt = parseInt(expires) * 1000; // Convert to JS timestamp
        sessionState.lastActivity = Date.now();
        sessionState.warningShown = false; // Reset warning if session extended
        
        // Hide modal if it's showing and we have plenty of time
        if (remaining && parseInt(remaining) > sessionState.alertThreshold) {
            $("#sessionExpiryWarning").modal("hide");
        }
        
        // Update localStorage for cross-tab sync
        localStorage.setItem('sessionState', JSON.stringify(sessionState));
        
        console.log(`Session updated: ${remaining} seconds remaining`);
    }
}

// Intercept ALL AJAX responses
$(document).ajaxComplete(function(event, xhr, settings) {
    updateSessionFromResponse(xhr);
});

// For fetch API (if used in the application)
const originalFetch = window.fetch;
window.fetch = function(...args) {
    return originalFetch.apply(this, args).then(response => {
        const remaining = response.headers.get('X-Session-Remaining');
        const expires = response.headers.get('X-Session-Expires');
        if (expires) {
            updateSessionFromResponse({
                getResponseHeader: (name) => response.headers.get(name)
            });
        }
        return response;
    });
};

// Main countdown logic - much simpler now
function runSessionCountdown() {
    setInterval(() => {
        if (!sessionState.expiresAt) return;
        
        const now = Date.now();
        const remainingMs = sessionState.expiresAt - now;
        const remainingSeconds = Math.round(remainingMs / 1000);
        
        if (remainingSeconds <= 0) {
            // Session expired, redirect to logout
            window.location.href = $("#sessionExpiry").data("url-target");
        } else if (remainingSeconds <= sessionState.alertThreshold && !sessionState.warningShown) {
            // Show warning modal
            $("#sessionExpiryWarning").modal("show");
            sessionState.warningShown = true;
        }
        
        // Update countdown display if modal is visible
        if (sessionState.warningShown) {
            $("#sessionExpiryWarningSeconds").text(remainingSeconds);
        }
    }, 1000); // Check every second
}

// Initialize on page load
window.addEventListener("DOMContentLoaded", () => {
    // Get initial session time from server
    const dom = $("#sessionExpiry")[0];
    const initialSeconds = Number(dom.dataset.secondsLeft) || 0;
    
    if (initialSeconds > 0) {
        sessionState.expiresAt = Date.now() + (initialSeconds * 1000);
        sessionState.alertThreshold = Number(dom.dataset.alertThreshold) || 60;
        
        runSessionCountdown();
    }
    
    // Setup "Stay logged in" button
    $("#sessionKeepAlive").click(() => {
        $.ajax({
            url: $("#sessionExpiryWarning").data("url-keep-alive"),
            cache: false
        });
    });
});
```

## Phase 3: Cross-Tab Synchronization

### 3.1 Add localStorage Event Listener
```javascript
// Listen for session updates from other tabs
window.addEventListener('storage', (e) => {
    if (e.key === 'sessionState' && e.newValue) {
        const newState = JSON.parse(e.newValue);
        
        // Only update if other tab has more recent info
        if (newState.lastActivity > sessionState.lastActivity) {
            sessionState = newState;
            console.log('Session state updated from another tab');
        }
    }
});

// Handle tab becoming active again
document.addEventListener('visibilitychange', () => {
    if (!document.hidden) {
        // Tab became visible, check localStorage for updates
        const stored = localStorage.getItem('sessionState');
        if (stored) {
            const storedState = JSON.parse(stored);
            if (storedState.lastActivity > sessionState.lastActivity) {
                sessionState = storedState;
            }
        }
    }
});
```

## Phase 4: Fix the Observer Effect

### 4.1 Alternative: Lightweight Activity Ping Endpoint
```csharp
// In AccountController.cs
// This endpoint exists just to trigger the middleware which will add headers
[HttpGet]
[Authorize]
public IActionResult ActivityPing()
{
    // This endpoint does nothing but return success
    // The authentication middleware has already extended the cookie
    // Our tracking middleware will add the session headers
    return Ok(new { status = "active" });
}
```

## Phase 5: Optional Enhancements

### 5.1 Throttled Activity Updates (Optional)
If you want mouse/keyboard activity to extend the session:

```javascript
// Throttle function to limit server calls
function throttle(func, delay) {
    let timeoutId;
    let lastExecTime = 0;
    return function (...args) {
        const currentTime = Date.now();
        if (currentTime - lastExecTime > delay) {
            func.apply(this, args);
            lastExecTime = currentTime;
        }
    };
}

// Extend session on activity (throttled to once per minute)
const extendSession = throttle(() => {
    $.ajax({
        url: '/Account/RemainingSessionTime',
        cache: false,
        success: (data) => {
            console.log('Session extended due to user activity');
        }
    });
}, 60000); // Max once per minute

// Optional: Track user activity
['click', 'keypress'].forEach(event => {
    document.addEventListener(event, () => {
        // Only extend if session is active and not in warning period
        const remaining = (sessionState.expiresAt - Date.now()) / 1000;
        if (remaining > sessionState.alertThreshold && remaining < 600) {
            // If less than 10 minutes remaining, extend it
            extendSession();
        }
    });
});
```

## Implementation Steps

1. **Week 1: Middleware Implementation**
   - Create and test `SessionActivityTrackingMiddleware`
   - Verify headers are added to all authenticated responses
   - Test with various API endpoints

2. **Week 1-2: JavaScript Refactoring**
   - Replace countdown logic with header-based approach
   - Test AJAX interception (both jQuery and fetch)
   - Verify modal shows/hides correctly

3. **Week 2: Cross-Tab Sync**
   - Implement localStorage synchronization
   - Test with multiple tabs
   - Handle edge cases (tab switching, browser back/forward)

4. **Week 2: Testing & Refinement**
   - Test with various session timeout values
   - Test with heavy AJAX applications
   - Performance testing (ensure middleware doesn't slow responses)
   - Edge case testing (network issues, clock skew)

## Testing Checklist

- [ ] AJAX calls reset countdown timer
- [ ] Multiple tabs stay synchronized
- [ ] Warning modal appears at correct time
- [ ] "Stay logged in" button works reliably
- [ ] No false logouts during active API usage
- [ ] No warnings suppressed when they should appear
- [ ] Performance impact is minimal
- [ ] Works with both jQuery AJAX and fetch API
- [ ] Handles network interruptions gracefully

## Why This Solution Works

1. **No HttpContext.Session needed** - Response headers communicate state directly
2. **No observer effect** - We calculate the new expiry time WITHOUT calling `AuthenticateAsync()`
3. **Real-time updates** - Every API response includes current session state
4. **Cross-tab aware** - localStorage keeps all tabs in sync
5. **No external app changes** - Everything happens in middleware and JavaScript
6. **Accurate countdown** - Based on sliding window configuration, matches what auth cookie will do

## Potential Issues to Watch

1. **Response Header Size** - Keep headers small (just timestamps)
2. **CORS** - May need to expose custom headers for cross-origin requests
3. **Caching** - Ensure responses with session headers aren't cached
4. **Clock Skew** - Client/server time differences could cause issues

## Alternative Approach (If Headers Don't Work)

If adding response headers proves problematic, use a lightweight polling approach:

```javascript
// Poll every 30 seconds, but only if user is active
setInterval(() => {
    const timeSinceLastActivity = Date.now() - lastUserActivity;
    if (timeSinceLastActivity < 30000) { // Active in last 30 seconds
        $.ajax({
            url: '/Account/CheckSessionStatus',
            cache: false,
            success: (data) => {
                if (!data.expired) {
                    sessionState.expiresAt = data.expiresAt * 1000;
                }
            }
        });
    }
}, 30000);
```

This plan provides a complete, implementable solution without requiring HttpContext.Session or modifications to external applications.