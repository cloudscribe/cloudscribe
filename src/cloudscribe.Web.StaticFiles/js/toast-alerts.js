document.addEventListener("DOMContentLoaded", function () {

    var toastAjax = function (method, url, contentType) {
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                var DONE = 4; // readyState 4 means the request is done.
                var OK = 200; // status 200 is a successful return.
                if (xhr.readyState === DONE) {
                    if (xhr.status === OK) {
                        resolve(xhr.responseText);
                    }
                    else {
                        reject("request failed for " + url + " " + xhr.status);
                    }
                }
            };
            xhr.open(method, url);
            if (contentType) {
                xhr.setRequestHeader('Content-Type', contentType);

            }
            xhr.responseType = "text";
            xhr.send();
        });
    };

    var toastConfig = document.querySelectorAll('[data-toast-config]')[0];
    if (toastConfig && toastConfig.dataset.toastsUrl) {
        toastAjax("GET", toastConfig.dataset.toastsUrl)
            .then(function (text) {
                //console.log(text);
                var j = JSON.parse(text);
                for (i = 0; i < j.length; ++i) {
                    $.toast({
                        title: j[i].message,
                        type: j[i].alertStyle,
                        delay: 6000
                    });
                }

            });
    }
    
});