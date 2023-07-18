
function doCountdownPromise(secondsLeft, delay, alertThreshold, alerted) {

    delay = delay * 1000 || 10000;  // work in ms for setTimeout
    
    var startingTime = new Date();

    var checkTimer = function (resolve, reject) {

        var secondsremaining = Number(secondsLeft) - (Number(new Date() - Number(startingTime)) / 1000.0);
        // console.log("remains " + secondsremaining);

        if (alertThreshold > 0 && secondsremaining > alertThreshold) {
            let dom = $("#sessionExpiryWarning");
            dom.hide();
            alerted = false;
        }

        if (alertThreshold > 0 && secondsremaining <= alertThreshold) {

            let dom = $("#sessionExpiryWarning");
            if (dom != null && parseInt(secondsremaining) >=0) {
                $("#sessionExpiryWarningSeconds").text(parseInt(secondsremaining));
            }

            if (!alerted) {
                var backupDelay = delay;
                delay = 1000;
                dom.show();

                $("#sessionKeepAlive").off();  // prevent binding this multiple times

                $("#sessionKeepAlive").click(() => {
                    let domKeepAlive = $("#sessionExpiryWarning")[0];
                    let source = domKeepAlive.dataset.urlKeepAlive;
                    source = source + "?t=" + Math.random();  // prevent cache

                    // for reasons unknown the server often fails to refresh the cookie on first polling request 
                    pollForKeepAlive(getRemainingTimePromise, source, 5000, 800).then(function (result) {
                        if (Number(result) - Number(secondsremaining) > 10) {
                            secondsLeft  = result;
                            startingTime = new Date();
                            delay        = backupDelay;
                            dom.hide();
                        }
                        else {   // so brute force a second attempt
                            pollForKeepAlive(getRemainingTimePromise, source, 5000, 800).then(function (result) {
                                if (Number(result) - Number(secondsremaining) > 10) {
                                    secondsLeft  = result;
                                    startingTime = new Date();
                                    delay        = backupDelay;
                                    dom.hide();
                                }
                            }).catch(function () {
                            });
                        }
                    }).catch(function () {
                    });
                });

                hookupLogoutButton();

                alerted = true;
            }
        }

        if (secondsremaining < -2) {  // build in a small delay for safety if the timer here has got here quicker than the server
            resolve();
        }
        else {
            setTimeout(checkTimer, delay, resolve, reject);
        }
    }

    return new Promise(checkTimer);
}


function getRemainingTimePromise(source) {
    return $.ajax({
        url: source
    });
}

function pollForKeepAlive(retrievalFunction, source, timeout, interval) {
    var endTime = Number(new Date()) + (timeout || 5000);

    var checkCondition = function (resolve, reject) {

        var result = retrievalFunction(source);

        // got a value for session time remaining from server
        if (result) {
            resolve(result);
        }

        //  failure to get a session value back fromn the endpoint 
        //  keep trying until endTime
        else if (Number(new Date()) < endTime) {
            setTimeout(checkCondition, interval, resolve, reject);
        }

        // timeout of the poller in general
        else {
            reject(new Error('session checker timed out'));
        }
    };

    return new Promise(checkCondition);
}


window.addEventListener("DOMContentLoaded", () => {

    let dom            = $("#sessionExpiry")[0];
    let target         = dom.dataset.urlTarget;
    let alertThreshold = Number(dom.dataset.alertThreshold) || 60;
    let interval       = Number(dom.dataset.pollingInterval) || 5;
    var secondsLeft    = Number(dom.dataset.secondsLeft) || Number(getRemainingTime(source)) || 0.0;

    // fix for arriving at the 'timed out' page whilst still being logged in
    if (window.location.href == target) {
        btnManualLogout();
    }

    if (secondsLeft > 0) {

        doCountdownPromise(secondsLeft, interval, alertThreshold, false).then(function () {
            window.location.href = target;
        }).catch(function () { 
        });
    }
});


function btnManualLogout(event) {
    var logoutForm = document.getElementById("logoutForm");
    if (logoutForm) {
        logoutForm.submit();
    }
    event.preventDefault();
}


function hookupLogoutButton() {
    var logoutBtn = document.getElementById("btnSessionLogOut");
    if (logoutBtn) {
        logoutBtn.addEventListener('click', btnManualLogout, false);
    }
}

