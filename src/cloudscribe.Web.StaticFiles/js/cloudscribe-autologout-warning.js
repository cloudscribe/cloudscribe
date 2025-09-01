function doCountdownPromise(secondsLeft, delay, alertThreshold, alerted) {
    delay = delay * 1000 || 10000;  // work in ms
    let startingTime = new Date();

    const resetTimer = () => {
        startingTime = new Date();  // 🔁 Reset countdown
        console.log("Session timer reset due to user activity");
    };

    // Add activity listeners to reset the timer
    ["mousemove", "keydown", "touchstart", "scroll"].forEach(event =>
        document.addEventListener(event, resetTimer)
    );

    const checkTimer = function (resolve, reject) {
        let secondsremaining = secondsLeft - ((new Date() - startingTime) / 1000.0);

        if (alertThreshold > 0 && secondsremaining > alertThreshold) {
            $("#sessionExpiryWarning").modal("hide");
            alerted = false;
        }

        if (alertThreshold > 0 && secondsremaining <= alertThreshold) {
            let dom = $("#sessionExpiryWarning");

            if (dom && parseInt(secondsremaining) >= 0) {
                $("#sessionExpiryWarningSeconds").text(parseInt(secondsremaining));
            }

            if (!alerted) {
                let backupDelay = delay;
                delay = 1000;
                dom.modal("show");

                $("#sessionKeepAlive").off().click(() => {
                    let source = dom[0].dataset.urlKeepAlive + "?t=" + Math.random();

                    pollForKeepAlive(getRemainingTimePromise, source, 5000, 800).then(result => {
                        if (result - secondsremaining > 10) {
                            secondsLeft = result;
                            startingTime = new Date();
                            delay = backupDelay;
                            dom.modal("hide");
                        } else {
                            pollForKeepAlive(getRemainingTimePromise, source, 5000, 800).then(result => {
                                if (result - secondsremaining > 10) {
                                    secondsLeft = result;
                                    startingTime = new Date();
                                    delay = backupDelay;
                                    dom.modal("hide");
                                }
                            }).catch(() => { });
                        }
                    }).catch(() => { });
                });

                hookupLogoutButton();
                alerted = true;
            }
        }

        if (secondsremaining < -2) {
            resolve();
        } else {
            setTimeout(checkTimer, delay, resolve, reject);
        }
    };

    return new Promise(checkTimer);
}

function getRemainingTimePromise(source) {
    return $.ajax({ url: source });
}

function pollForKeepAlive(retrievalFunction, source, timeout, interval) {
    let endTime = Date.now() + (timeout || 5000);

    const checkCondition = function (resolve, reject) {
        retrievalFunction(source).then(result => {
            if (result) {
                resolve(result);
            } else if (Date.now() < endTime) {
                setTimeout(checkCondition, interval, resolve, reject);
            } else {
                reject(new Error("session checker timed out"));
            }
        }).catch(() => {
            if (Date.now() < endTime) {
                setTimeout(checkCondition, interval, resolve, reject);
            } else {
                reject(new Error("session checker failed"));
            }
        });
    };

    return new Promise(checkCondition);
}

window.addEventListener("DOMContentLoaded", () => {
    let dom = $("#sessionExpiry")[0];
    let source = dom.dataset.urlKeepAlive;
    let target = dom.dataset.urlTarget;
    let jax = dom.dataset.urlJax;
    let userID = String(dom.dataset.userid);
    let alertThreshold = Number(dom.dataset.alertThreshold) || 60;
    let interval = Number(dom.dataset.pollingInterval) || 5;

    getRemainingTimePromise(source).then(result => {
        let secondsLeft = Number(dom.dataset.secondsLeft) || Number(result) || 0.0;

        if (window.location.href === target) {
            btnManualLogout();
        }

        if (secondsLeft > 0) {
            doCountdownPromise(secondsLeft, interval, alertThreshold, false).then(() => {
                //window.location.href = target + "?" +"userid=" + userID;
                AutoLogoutNotification(target,jax ,userID);
            }).catch(() => { });
        }
    });
});

function btnManualLogout(event) {
    let logoutForm = document.getElementById("logoutForm");
    if (logoutForm) logoutForm.submit();
    if (event) event.preventDefault();
}

function hookupLogoutButton() {
    let logoutBtn = document.getElementById("btnSessionLogOut");
    if (logoutBtn) logoutBtn.addEventListener("click", btnManualLogout, false);
}

window.addEventListener("load", () => {
    const dom = document.getElementById("sessionExpiryWarning");
    if (!dom || !dom.dataset.urlKeepAlive) return;

    const source = dom.dataset.urlKeepAlive;
    const timeout = 5000;  // total time to keep trying
    const interval = 800;  // time between attempts

    pollForKeepAlive(getRemainingTimePromise, source, timeout, interval)
        .then(result => {
            if (result && !isNaN(result)) {
                dom.dataset.secondsLeft = Number(result);
                console.log("Session secondsLeft populated via polling:", result);
            }
        })
        .catch(err => {
            console.warn("Polling failed to retrieve session time:", err);
        });
});

function AutoLogoutNotification(targetUrl,urlJax,userid) {
    $.ajax({
        url: urlJax, // Replace with your actual endpoint
        method: "GET",        
        data:{userid: userid},
        success: function (response) {
            if (response.redirect) {
                window.location.href = targetUrl;
            }
            console.log("User details received:", response);           
        },
        error: function (xhr, status, error) {
            console.error("Error fetching user details:", error);
        }
    });
}

