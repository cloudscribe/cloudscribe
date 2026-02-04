// Simplified server-side session tracking approach
// The server maintains the authoritative session state in MemoryCache

let sessionState = {
    expiresAt: null,
    warningShown: false,
    alertThreshold: 60,
    autoLogoutEnabled: false,
    lastServerCheck: 0,
    checkOnlyWhenClose: true  // Only poll server when close to expiry
};

function startSessionMonitoring() {
    setInterval(() => {
        if (!sessionState.autoLogoutEnabled) {
            return;
        }

        const now = Date.now();
        const remainingMs = sessionState.expiresAt - now;
        const remainingSeconds = Math.round(remainingMs / 1000);

        // When getting close to expiry (< 2 minutes), check actual server state
        if (remainingSeconds < 120 && remainingSeconds > 0) {
            // Throttle checks - don't check more than once per 10 seconds
            if (now - sessionState.lastServerCheck > 10000) {
                checkActualSessionTime();
                sessionState.lastServerCheck = now;
            }
        }

        // Show warning if needed
        if (remainingSeconds <= sessionState.alertThreshold && 
            remainingSeconds > 0 && 
            !sessionState.warningShown) {
            $("#sessionExpiryWarning").modal("show");
            sessionState.warningShown = true;
            console.log('Session expiry warning shown');
        }

        // Update countdown if modal is visible
        if (sessionState.warningShown && remainingSeconds > 0) {
            $("#sessionExpiryWarningSeconds").text(remainingSeconds);
        }

        // Redirect if expired
        if (remainingSeconds <= 0) {
            console.log('Session expired - redirecting to logout');
            window.location.href = $("#sessionExpiry").data("url-target");
        }

    }, 1000); // Check every second for smooth countdown
}

function checkActualSessionTime() {
    // This is the ONLY server call we make for checking
    // It checks the cached time without extending the session
    $.ajax({
        url: '/Account/GetActualSessionTime',
        cache: false,
        success: function(data) {
            if (data.expired) {
                // Session actually expired on server
                console.log('Server reports session expired');
                window.location.href = $("#sessionExpiry").data("url-target");
            } else if (data.remainingSeconds > sessionState.alertThreshold) {
                // Session was extended by other activity (AJAX, other tabs, etc)
                var currentExpected = Math.round((sessionState.expiresAt - Date.now()) / 1000);
                if (data.remainingSeconds > currentExpected + 5) {
                    // Significant extension - real server activity happened
                    sessionState.expiresAt = Date.now() + (data.remainingSeconds * 1000);
                    sessionState.warningShown = false;
                    $("#sessionExpiryWarning").modal("hide");
                    console.log('Session extended by server activity - remaining: ' + data.remainingSeconds + 's');
                    
                    // Update localStorage for cross-tab sync (only when extended)
                    localStorage.setItem('sessionExtended', JSON.stringify({
                        expiresAt: sessionState.expiresAt,
                        timestamp: Date.now()
                    }));
                } else {
                    // Just normal countdown, update our local state to match server
                    sessionState.expiresAt = Date.now() + (data.remainingSeconds * 1000);
                    console.log('Session time confirmed from server - remaining: ' + data.remainingSeconds + 's');
                    // Don't update localStorage for normal countdown
                }
            }
            // If remaining time is still low, keep showing warning
        },
        error: function() {
            // Network error - be conservative, log error but don't redirect
            console.error('Failed to check session status - network error');
        }
    });
}

function initializeSessionTracking() {
    const dom = $("#sessionExpiry")[0];
    if (!dom) {
        // No session expiry element - auto-logout not configured for this page
        console.log('Auto-logout element not found - feature not enabled');
        return;
    }

    const initialSeconds = Number(dom.dataset.secondsLeft) || 0;

    // IMPORTANT: Only enable auto-logout if we have a valid timeout > 0
    // Zero or negative means auto-logout is disabled
    if (initialSeconds > 0) {
        sessionState.autoLogoutEnabled = true;
        sessionState.expiresAt = Date.now() + (initialSeconds * 1000);
        sessionState.alertThreshold = Number(dom.dataset.alertThreshold) || 60;

        console.log('Auto-logout enabled - expires in ' + initialSeconds + ' seconds');
        startSessionMonitoring();
    } else {
        // Auto-logout is disabled - don't start any monitoring
        console.log('Auto-logout is disabled (timeout is zero or null)');
        sessionState.autoLogoutEnabled = false;
    }
}

// Cross-tab synchronization via localStorage
window.addEventListener('storage', function(e) {
    if (e.key === 'sessionExtended' && e.newValue) {
        // Another tab extended the session
        var data = JSON.parse(e.newValue);
        if (data.expiresAt > sessionState.expiresAt) {
            sessionState.expiresAt = data.expiresAt;
            sessionState.warningShown = false;
            $("#sessionExpiryWarning").modal("hide");
            console.log('Session state updated from another tab');
        }
    }
});

window.addEventListener("DOMContentLoaded", () => {
    
    // Initialize the session tracking
    initializeSessionTracking();

    // Fix for arriving at the 'timed out' page whilst still being logged in
    // Only auto-logout if we were actually tracking a session
    const dom = $("#sessionExpiry")[0];
    if (dom) {
        const target = dom.dataset.urlTarget;
        if (window.location.href.includes(target.split('/').pop()) && 
            sessionState.autoLogoutEnabled && 
            sessionState.expiresAt) {
            console.log('Auto-logout triggered - user reached timeout page while session was being tracked');
            btnManualLogout();
        }
    }

    // Setup button event handlers
    $("#sessionKeepAlive").click(function() {
        $.ajax({
            url: '/Account/RemainingSessionTime', // This DOES extend session <<- should be renamed really - jk
            cache: false,
            success: function(data) {
                if (typeof data === 'number' && data > 0) {
                    sessionState.expiresAt = Date.now() + (data * 1000);
                    sessionState.warningShown = false;
                    $("#sessionExpiryWarning").modal("hide");
                    console.log('Session manually extended - remaining: ' + data + 's');
                    
                    // Update localStorage for cross-tab sync
                    localStorage.setItem('sessionExtended', JSON.stringify({
                        expiresAt: sessionState.expiresAt,
                        timestamp: Date.now()
                    }));
                }
            },
            error: function() {
                console.error('Failed to extend session');
            }
        });
    });

    // Setup logout button
    hookupLogoutButton();
});

function btnManualLogout(event) {
    var logoutForm = document.getElementById("logoutForm");
    if (logoutForm) {
        logoutForm.submit();
    }
    // Only prevent default if event exists (when called from click handler)
    if (event) {
        event.preventDefault();
    }
}

function hookupLogoutButton() {
    var logoutBtn = document.getElementById("btnSessionLogOut");
    if (logoutBtn) {
        logoutBtn.addEventListener('click', btnManualLogout, false);
    }
}

