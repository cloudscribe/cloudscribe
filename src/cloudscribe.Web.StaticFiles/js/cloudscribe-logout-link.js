document.addEventListener("DOMContentLoaded", function () {
    var logoutLink = document.getElementById("lnkLogout");
    if (logoutLink) {
        logoutLink.addEventListener('click', function (event) {
            var logoutForm = document.getElementById("logoutForm");
            if (logoutForm) {
                logoutForm.submit();
            }
            event.preventDefault();
        }, false);
    }
});