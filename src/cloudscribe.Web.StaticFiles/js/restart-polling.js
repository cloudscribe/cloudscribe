/**
 * Application Restart Polling Script
 * Polls the server to detect when the application comes back online after restart
 */
(function() {
    'use strict';
    
    var attempts = 0;
    var maxAttempts = 60; // Try for about 60 seconds
    var checkInterval = null;
    
    console.log('[Restart] Page loaded, will start checking in 3 seconds...');
    
    function checkIfOnline() {
        attempts++;
        console.log('[Restart] Attempt ' + attempts + ' of ' + maxAttempts + ' - Checking /siteadmin...');
        
        // Try to fetch the admin page
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/siteadmin', true);
        xhr.timeout = 3000; // 3 second timeout
        
        xhr.onreadystatechange = function() {
            if (xhr.readyState === 4) {
                console.log('[Restart] Response received - Status: ' + xhr.status + ', ReadyState: ' + xhr.readyState);
                
                if (xhr.status === 200) {
                    // Site is back online!
                    console.log('[Restart] SUCCESS! Site is back online. Redirecting to /siteadmin...');
                    clearInterval(checkInterval);
                    window.location.href = '/siteadmin';
                } else if (xhr.status === 503) {
                    console.log('[Restart] Site still unavailable (503). Will keep trying...');
                } else if (xhr.status > 0) {
                    console.log('[Restart] Unexpected status: ' + xhr.status + '. Will keep trying...');
                }
                
                if (attempts >= maxAttempts) {
                    // Timeout - show manual link
                    console.log('[Restart] Max attempts reached. Showing manual link.');
                    clearInterval(checkInterval);
                    var messageEl = document.getElementById('restart-message');
                    if (messageEl) {
                        messageEl.innerHTML = 'Restart is taking longer than expected.<br><a href="/siteadmin" class="btn btn-primary btn-sm mt-2">Click here to continue</a>';
                    }
                }
                // Otherwise, keep trying (interval will call this again)
            }
        };
        
        xhr.onerror = function() {
            // Connection error - site is down, keep trying
            console.log('[Restart] Connection error (site is down or restarting). Attempt ' + attempts);
            
            if (attempts >= maxAttempts) {
                console.log('[Restart] Max attempts reached after errors. Showing manual link.');
                clearInterval(checkInterval);
                var messageEl = document.getElementById('restart-message');
                if (messageEl) {
                    messageEl.innerHTML = 'Restart is taking longer than expected.<br><a href="/siteadmin" class="btn btn-primary btn-sm mt-2">Click here to continue</a>';
                }
            }
        };
        
        xhr.ontimeout = function() {
            console.log('[Restart] Request timed out after 3 seconds. Attempt ' + attempts);
        };
        
        try {
            xhr.send();
        } catch(e) {
            // Error sending - site is probably down
            console.log('[Restart] Error sending request: ' + e.message);
        }
    }
    
    // Start checking after 3 seconds, then every second
    setTimeout(function() {
        console.log('[Restart] Starting polling...');
        checkIfOnline(); // First check
        checkInterval = setInterval(checkIfOnline, 1000); // Then check every second
    }, 3000);
    
})();