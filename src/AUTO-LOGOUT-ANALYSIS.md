# CloudScribe Auto-Logout Feature Analysis

## Current Implementation Overview

The CloudScribe auto-logout feature automatically logs users out after a period of inactivity for security purposes.

### Key Components

1. **Configuration** (`MaximumInactivityInMinutes`)
   - Set via Security Settings admin page
   - Configures authentication cookie with `ExpireTimeSpan` and `SlidingExpiration`
   - Located in: `/mnt/d/development2/cloudscribe/src/cloudscribe.Core.Identity/SiteCookieAuthenticationOptions.cs:89-90`

2. **Client-Side Monitoring** (`cloudscribe-autologout-warning.js`)
   - Initializes with session timeout configuration
   - Runs countdown timer based on initial server-provided time
   - Shows warning modal when < 60 seconds remain
   - Cannot re-check server without extending session

3. **Server-Side Components**
   - `RemainingSessionTimeResolver.cs`: Calculates remaining time via `AuthenticateAsync()`
   - `AccountController.RemainingSessionTime()`: Returns seconds remaining (extends session as side-effect)
   - `AccountController.AutoLogoutNotification()`: Shows timeout page

4. **Warning Modal** (`_AutoLogoutWarningPartial.cshtml`)
   - Displays countdown
   - "Stay logged in" button - refreshes session
   - "Log out" button - immediate logout

## Critical Architecture Limitation

### The Catch-22 Problem
The `RemainingSessionTimeResolver` calls `AuthenticateAsync()` which automatically refreshes the sliding expiration cookie. This creates an impossible situation:
- **Need**: Check remaining session time to show accurate warnings
- **Problem**: Checking the time extends the session
- **Result**: Can only check once at page load, then rely on client-side countdown

## Current Issues

### Issue 1: AJAX Activity Not Recognized
- **Problem**: AJAX API calls extend the server-side session, but the client-side countdown timer doesn't know this
- **Result**: Users get logged out despite being actively using the application via AJAX
- **Root Cause**: Cannot poll server for updated session time without extending it

### Issue 2: Multi-Tab Logout Problem  
- **Problem**: Each browser tab has its own isolated countdown timer
- **Result**: An inactive tab will redirect to logout even if user is active in another tab
- **Root Cause**: Tabs cannot check real server-side session status without extending it

## Why Current Architecture Is Poorly Suited

The fundamental assumption that "checking session status = activity" makes it extremely difficult to:
1. Track actual user activity across different types of requests
2. Coordinate session state across multiple browser tabs
3. Maintain accurate client-side representation of server-side session state

## Potential Solutions

### Solution 1: Separate Read-Only Session Check
Create an endpoint that checks session expiry WITHOUT triggering authentication:
```csharp
public async Task<double> GetRemainingTimeWithoutRefresh()
{
    // Read authentication cookie directly
    var cookie = Request.Cookies[".AspNetCore.Identity.Application"];
    // Decrypt and read expiry without calling AuthenticateAsync()
    // Return remaining time without extending session
}
```

### Solution 2: Activity Tracking Middleware
Decouple activity tracking from authentication cookie mechanics:
```csharp
public class ActivityTrackingMiddleware
{
    // Store in distributed cache or session
    // Key: UserId, Value: LastActivityTimestamp
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await cache.SetAsync($"activity:{userId}", DateTime.UtcNow);
        }
    }
}
```

Then check inactivity separately from cookie expiry:
- Track all requests (including AJAX) as activity
- Check: `CurrentTime - LastActivityTime > InactivityTimeout`
- Logout based on inactivity, not cookie expiry

### Solution 3: Cross-Tab Communication
Use localStorage/sessionStorage events for tab coordination:
```javascript
// Broadcast activity to all tabs
window.addEventListener('storage', (e) => {
    if (e.key === 'lastServerActivity') {
        // Update local countdown based on activity from other tabs
        updateCountdownFromSharedState(e.newValue);
    }
});

// On any AJAX success
$(document).ajaxSuccess(() => {
    // Store timestamp and new expiry time
    localStorage.setItem('lastServerActivity', JSON.stringify({
        timestamp: Date.now(),
        expiresIn: getExpiryFromResponse() // Server includes this in response headers
    }));
});
```

### Solution 4: WebSocket/SignalR Push Updates
Instead of polling, push session status to all tabs:
```csharp
public class SessionHub : Hub
{
    public async Task BroadcastSessionUpdate(string userId, int remainingSeconds)
    {
        await Clients.User(userId).SendAsync("SessionUpdate", remainingSeconds);
    }
}
```

### Solution 5: Different Cookie Strategy
Use absolute expiration with explicit refresh:
```csharp
options.Cookie.MaxAge = TimeSpan.FromMinutes(30);  // Fixed expiry
options.SlidingExpiration = false;  // No automatic refresh

// Explicit refresh endpoint
public async Task<IActionResult> RefreshSession()
{
    if (ShouldRefreshBasedOnActivity())
    {
        await SignInManager.RefreshSignInAsync(user);
        return Json(new { refreshed = true, expiresIn = 1800 });
    }
    return Json(new { refreshed = false });
}
```

## Recommended Approach

For minimal changes with maximum benefit:

1. **Implement Solution 2** (Activity Tracking Middleware) to properly track all user activity including AJAX
2. **Implement Solution 3** (Cross-Tab Communication) to coordinate between browser tabs
3. **Modify JavaScript** to trust the shared state from localStorage over its local countdown

This would address both main criticisms while keeping most of the existing architecture intact.

## Implementation Priority

1. **High Priority**: Fix AJAX activity recognition (Solution 2)
2. **High Priority**: Fix multi-tab coordination (Solution 3)  
3. **Medium Priority**: Add read-only session check (Solution 1)
4. **Low Priority**: Consider WebSocket/SignalR for real-time updates (Solution 4)
5. **Optional**: Evaluate different cookie strategies for specific use cases (Solution 5)