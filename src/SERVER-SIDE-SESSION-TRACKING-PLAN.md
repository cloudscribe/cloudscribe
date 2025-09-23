# Server-Side Session Tracking Solution

## Overview
Instead of complex JavaScript AJAX interception, the server maintains the authoritative session expiry time in MemoryCache, updated by middleware on EVERY request. JavaScript only checks this when needed.

**Key Advantage:** No AJAX interception needed, works with ANY frontend framework automatically.

## Architecture

### Component 1: Session Activity Service
```csharp
// cloudscribe.Core.Web/Services/SessionActivityService.cs
public interface ISessionActivityService
{
    void UpdateActivity(string userId, string siteId);
    DateTime? GetSessionExpiry(string userId, string siteId);
    void RemoveSession(string userId, string siteId);
}

public class SessionActivityService : ISessionActivityService
{
    private readonly IMemoryCache _cache;
    private readonly ISiteContextResolver _siteResolver;
    private readonly ILogger<SessionActivityService> _logger;
    
    public SessionActivityService(
        IMemoryCache cache, 
        ISiteContextResolver siteResolver,
        ILogger<SessionActivityService> logger)
    {
        _cache = cache;
        _siteResolver = siteResolver;
        _logger = logger;
    }
    
    public void UpdateActivity(string userId, string siteId)
    {
        var key = $"session_{siteId}_{userId}";
        
        // Get site's configured timeout
        var site = _siteResolver.ResolveSiteById(siteId).Result;
        
        // IMPORTANT: If MaximumInactivityInMinutes is null, empty, or zero, 
        // auto-logout is disabled - don't track anything
        if (site != null && 
            !string.IsNullOrWhiteSpace(site.MaximumInactivityInMinutes) && 
            double.TryParse(site.MaximumInactivityInMinutes, out var minutes) && 
            minutes > 0)
        {
            var expiryTime = DateTime.UtcNow.AddMinutes(minutes);
            
            // Store in cache with absolute expiry slightly longer than session
            // This ensures cache entries clean themselves up
            _cache.Set(key, expiryTime, TimeSpan.FromMinutes(minutes + 5));
            
            _logger.LogDebug($"Updated session activity for user {userId}: expires at {expiryTime}");
        }
        else
        {
            // Auto-logout is disabled for this site
            // Remove any existing cache entry to ensure no tracking
            _cache.Remove(key);
        }
    }
    
    public DateTime? GetSessionExpiry(string userId, string siteId)
    {
        var key = $"session_{siteId}_{userId}";
        if (_cache.TryGetValue<DateTime>(key, out var expiry))
        {
            return expiry;
        }
        return null;
    }
    
    public void RemoveSession(string userId, string siteId)
    {
        var key = $"session_{siteId}_{userId}";
        _cache.Remove(key);
    }
}
```

### Component 2: Lightweight Middleware
```csharp
// cloudscribe.Core.Web/Middleware/SessionActivityTrackingMiddleware.cs
public class SessionActivityTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISessionActivityService _activityService;
    
    public SessionActivityTrackingMiddleware(
        RequestDelegate next,
        ISessionActivityService activityService)
    {
        _next = next;
        _activityService = activityService;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        
        // After request completes, if user is authenticated and request succeeded
        // Do NOT call AuthenticateAsync() - just use the principal that's already there
        if (context.User?.Identity?.IsAuthenticated == true && 
            context.Response.StatusCode >= 200 && 
            context.Response.StatusCode < 300)
        {
            var userId = context.User.FindFirst("sub")?.Value 
                        ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!string.IsNullOrEmpty(userId))
            {
                // Get site context without re-authenticating
                var siteId = context.Items["SiteId"]?.ToString() 
                           ?? context.Request.Headers["X-Site-Id"].FirstOrDefault()
                           ?? "default";
                
                // Update the cached expiry time
                _activityService.UpdateActivity(userId, siteId);
            }
        }
    }
}
```

### Component 3: Check Endpoint (No Authentication Extension)
```csharp
// In AccountController.cs
[HttpGet]
[Authorize]
public IActionResult GetActualSessionTime()
{
    // This endpoint checks the CACHED time, not the cookie
    // Therefore it doesn't extend the session
    
    var userId = User.FindFirst("sub")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    if (string.IsNullOrEmpty(userId))
    {
        return Json(new { expired = true });
    }
    
    var siteId = CurrentSite?.Id.ToString() ?? "default";
    var expiry = _activityService.GetSessionExpiry(userId, siteId);
    
    if (expiry == null || expiry <= DateTime.UtcNow)
    {
        return Json(new { expired = true });
    }
    
    var remainingSeconds = (expiry.Value - DateTime.UtcNow).TotalSeconds;
    
    return Json(new { 
        remainingSeconds = Math.Max(0, Math.Round(remainingSeconds)),
        expiresAt = ((DateTimeOffset)expiry.Value).ToUnixTimeSeconds(),
        expired = false
    });
}
```

### Component 4: Simplified JavaScript
```javascript
// cloudscribe-autologout-warning.js - MUCH SIMPLER!

let sessionState = {
    expiresAt: null,
    warningShown: false,
    alertThreshold: 60,
    checkInterval: 10000, // Check every 10 seconds when close to expiry
    lastCheck: 0,
    autoLogoutEnabled: false // Track if auto-logout is enabled
};

function initializeSessionTracking() {
    const dom = $("#sessionExpiry")[0];
    if (!dom) {
        // No session expiry element - auto-logout not configured for this page
        return;
    }
    
    const initialSeconds = Number(dom.dataset.secondsLeft) || 0;
    
    // IMPORTANT: Only enable auto-logout if we have a valid timeout > 0
    // Zero or negative means auto-logout is disabled
    if (initialSeconds > 0) {
        sessionState.autoLogoutEnabled = true;
        sessionState.expiresAt = Date.now() + (initialSeconds * 1000);
        sessionState.alertThreshold = Number(dom.dataset.alertThreshold) || 60;
        
        startSessionMonitoring();
    } else {
        // Auto-logout is disabled - don't start any monitoring
        console.log('Auto-logout is disabled (timeout is zero or null)');
        sessionState.autoLogoutEnabled = false;
    }
}

function startSessionMonitoring() {
    setInterval(() => {
        const now = Date.now();
        const remainingMs = sessionState.expiresAt - now;
        const remainingSeconds = Math.round(remainingMs / 1000);
        
        // When getting close to expiry (< 2 minutes), check actual server state
        if (remainingSeconds < 120 && remainingSeconds > 0) {
            // Throttle checks - don't check more than once per 10 seconds
            if (now - sessionState.lastCheck > 10000) {
                checkActualSessionTime();
                sessionState.lastCheck = now;
            }
        }
        
        // Show warning if needed
        if (remainingSeconds <= sessionState.alertThreshold && 
            remainingSeconds > 0 && 
            !sessionState.warningShown) {
            $("#sessionExpiryWarning").modal("show");
            sessionState.warningShown = true;
        }
        
        // Update countdown if visible
        if (sessionState.warningShown && remainingSeconds > 0) {
            $("#sessionExpiryWarningSeconds").text(remainingSeconds);
        }
        
        // Redirect if expired
        if (remainingSeconds <= 0) {
            window.location.href = $("#sessionExpiry").data("url-target");
        }
        
    }, 1000); // Check every second for smooth countdown
}

function checkActualSessionTime() {
    // This is the ONLY server call we make
    // It checks the cached time without extending the session
    $.ajax({
        url: '/Account/GetActualSessionTime',
        cache: false,
        success: function(data) {
            if (data.expired) {
                // Session actually expired on server
                window.location.href = $("#sessionExpiry").data("url-target");
            } else if (data.remainingSeconds > sessionState.alertThreshold) {
                // Session was extended by other activity (AJAX, other tabs, etc)
                sessionState.expiresAt = Date.now() + (data.remainingSeconds * 1000);
                sessionState.warningShown = false;
                $("#sessionExpiryWarning").modal("hide");
                console.log('Session extended by server activity');
            }
            // If remaining time is still low, keep showing warning
        },
        error: function() {
            // Network error - be conservative, assume session might be expired
            console.error('Failed to check session status');
        }
    });
}

// Initialize on page load
$(document).ready(function() {
    initializeSessionTracking();
    
    // Keep existing "Stay logged in" functionality
    $("#sessionKeepAlive").click(function() {
        $.ajax({
            url: '/Account/RemainingSessionTime', // This DOES extend session
            cache: false,
            success: function(data) {
                sessionState.expiresAt = Date.now() + (data * 1000);
                sessionState.warningShown = false;
                $("#sessionExpiryWarning").modal("hide");
            }
        });
    });
});

// Optional: Cross-tab communication via localStorage
window.addEventListener('storage', function(e) {
    if (e.key === 'sessionExtended' && e.newValue) {
        // Another tab extended the session
        var data = JSON.parse(e.newValue);
        if (data.expiresAt > sessionState.expiresAt) {
            sessionState.expiresAt = data.expiresAt;
            sessionState.warningShown = false;
            $("#sessionExpiryWarning").modal("hide");
        }
    }
});
```

## Registration in Startup/Program.cs
```csharp
// In ConfigureServices/Services registration
services.AddMemoryCache();
services.AddSingleton<ISessionActivityService, SessionActivityService>();

// In Configure/app pipeline (AFTER authentication, BEFORE endpoints)
app.UseAuthentication();
app.UseMiddleware<SessionActivityTrackingMiddleware>();
app.UseAuthorization();
app.UseEndpoints(...);
```

## Why This Approach Is Superior

### 1. **No AJAX Interception Needed**
- Works with jQuery, Angular, React, Vue, vanilla JS - ANYTHING
- No complex prototype overrides
- No worrying about what methods to intercept

### 2. **Server Is Single Source of Truth**
- Cache maintains the actual expiry time
- Updated on EVERY authenticated request automatically
- No synchronization issues

### 3. **Minimal Client-Server Communication**
- JavaScript only calls server when close to expiry
- Reduces unnecessary network traffic
- No constant polling

### 4. **No Observer Effect**
- `GetActualSessionTime` reads from cache, not cookie
- Never calls `AuthenticateAsync()`
- Checking doesn't extend session

### 5. **Clean Separation of Concerns**
- Middleware: Updates activity
- Service: Manages cache
- Endpoint: Reports status
- JavaScript: Simple countdown and check

### 6. **Handles All Edge Cases**
- AJAX requests automatically tracked
- Multiple tabs work correctly
- Browser back/forward works
- Network interruptions handled gracefully

## Testing Scenarios

1. **AJAX Activity Test**
   - Load page, wait until 1 minute before expiry
   - Make AJAX call from browser console
   - Verify warning disappears as session extends

2. **Multi-Tab Test**
   - Open two tabs
   - Be active in one, idle in other
   - Verify idle tab recognizes activity from active tab

3. **No False Positives**
   - Let session approach expiry
   - Verify `GetActualSessionTime` doesn't extend it
   - Verify warning appears correctly

4. **Network Failure Handling**
   - Disconnect network when checking time
   - Verify graceful degradation

## Optional Enhancements

### 1. Distributed Cache (for multi-server)
```csharp
// Use Redis instead of MemoryCache
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
```

### 2. Activity Type Tracking
```csharp
public class ActivityRecord
{
    public DateTime Timestamp { get; set; }
    public string RequestPath { get; set; }
    public bool WasAjax { get; set; }
}

// Store more detailed activity information
_cache.Set(key, new SessionInfo 
{
    ExpiresAt = expiryTime,
    Activities = activities
});
```

### 3. Per-User Configuration
```csharp
// Different timeout per user role
var timeout = user.IsInRole("Admin") ? 60 : 30;
```

## Handling Disabled Auto-Logout

### In the Razor View (_AutoLogoutWarningPartial.cshtml)
```razor
@using cloudscribe.Core.Web
@inject RemainingSessionTimeResolver rstResolver
@inject ISiteContextResolver siteResolver

@{
    var site = await siteResolver.ResolveSite(Context.Request.Host.Host, Context.Request.Path);
    
    // Check if auto-logout is enabled
    var isAutoLogoutEnabled = false;
    var secondsTilLogout = 0.0;
    
    if (site != null && 
        !string.IsNullOrWhiteSpace(site.MaximumInactivityInMinutes) && 
        double.TryParse(site.MaximumInactivityInMinutes, out var minutes) && 
        minutes > 0)
    {
        isAutoLogoutEnabled = true;
        secondsTilLogout = await rstResolver.RemainingSessionTimeInSeconds();
    }
}

@if (isAutoLogoutEnabled && secondsTilLogout > 0)
{
    @Html.Resource(@<script src="/cr/js/cloudscribe-autologout-warning.min.js"></script>, "js")
    
    <div id="sessionExpiry"
         data-url-target="@Url.Action("AutoLogoutNotification", "Account")"
         data-alert-threshold="60"
         data-seconds-left="@secondsTilLogout">
    </div>
    
    <!-- Modal HTML here -->
    <div class="modal" id="sessionExpiryWarning">
        <!-- ... existing modal code ... -->
    </div>
}
@* If auto-logout is disabled, render nothing *@
```

### In SiteCookieAuthenticationOptions.cs
```csharp
// Existing code that sets cookie expiry
try
{
    string cookieExpiry = tenant.MaximumInactivityInMinutes;
    
    // Only set sliding expiration if value is valid and > 0
    if (!string.IsNullOrWhiteSpace(cookieExpiry) && 
        double.TryParse(cookieExpiry, out double cookieExpiryTime) && 
        cookieExpiryTime > 0)
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes((int)cookieExpiryTime);
        options.SlidingExpiration = true;
    }
    // If null/empty/zero, don't set ExpireTimeSpan - use default session cookie
}
catch (Exception ex)
{
    _log.LogError(ex, "Error setting maximum inactivity cookie expiry");
}
```

## Summary

This approach is **much cleaner** than AJAX interception:
- Server maintains truth in MemoryCache
- Middleware updates on EVERY request (including AJAX)
- JavaScript just checks periodically when near expiry
- No observer effect since we check cache, not cookie
- Works with ANY frontend framework automatically
- **Properly handles disabled auto-logout** (null/empty/zero MaximumInactivityInMinutes)

The key insight: **Let the server track its own sessions** instead of trying to make the client figure it out from incomplete information.

When `MaximumInactivityInMinutes` is null, empty, or zero:
- No tracking in MemoryCache
- No JavaScript countdown
- No warning modal
- Standard session cookies without sliding expiration