// Three-state cookie consent handler - CSP compliant
// Supports: Accept (consent), Decline (dismiss), and undecided states
document.addEventListener('DOMContentLoaded', function() {
    var consentDiv = document.getElementById('cookieConsent');
    if (!consentDiv) return;
    
    var buttons = consentDiv.querySelectorAll('button[data-cookie-action]');
    
    buttons.forEach(function(button) {
        button.addEventListener('click', function(e) {
            var action = e.target.dataset.cookieAction;
            
            if (action === 'accept') {
                // Set consent cookie (enables tracking)
                var cookieString = e.target.dataset.cookieString;
                if (cookieString) {
                    document.cookie = cookieString;
                }
            } else if (action === 'dismiss') {
                // Just dismiss banner (NO tracking enabled)
                // Set dismiss cookie for 1 year
                var expiryDate = new Date();
                expiryDate.setFullYear(expiryDate.getFullYear() + 1);
                
                // Set dismiss cookie with SameSite policy
                // Note: For SameSiteMode.Strict deployments, this should be 'samesite=strict'
                document.cookie = 'cookieconsent_dismissed=true; expires=' + 
                    expiryDate.toUTCString() + '; path=/; samesite=lax';
            }
            
            // Hide the banner
            consentDiv.classList.add('collapse');
        });
    });
});