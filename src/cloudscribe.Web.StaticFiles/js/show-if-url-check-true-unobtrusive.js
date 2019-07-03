
document.addEventListener("DOMContentLoaded", function () {

    var myajax = function (method, url, contentType) {
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

    var elements = document.querySelectorAll('[data-show-if-url-check]');
    for (i = 0; i < elements.length; ++i) {
        var ele = elements[i];
        var urlToCheck = ele.dataset.urlCheckUrl;
        if (urlToCheck) {

            myajax("GET", urlToCheck)
                .then(function (text) {
                    if (text === "true") {
                        ele.style.display = 'block';
                    }
                });
        }

    }

});