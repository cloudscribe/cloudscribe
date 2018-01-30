(function () {
    var inputs = document.querySelectorAll('input[data-remote-validate-onblur="true"]');
    [].forEach.call(inputs, function (input) {
        input.addEventListener("blur", function (event) {
            //console.log("blur");
            var inp = this;
            var testval = inp.value;
            var warnSpan = document.getElementById(inp.dataset.errorTargetId);
            var validationUrl = inp.dataset.validationUrl
            var query = "?" + inp.name + "=" + testval;
            if (inp.dataset.additionalInputIds) {
                var ids = inp.dataset.additionalInputIds.split(',');
                for (i = 0; i < ids.length; i++) {
                    var ele = document.getElementById(ids[i]);
                    if (ele) {
                        query += "&" + ele.name + "=" + ele.value;
                    }
                }
            }

            if (validationUrl) {
                var xhr = new XMLHttpRequest();
                xhr.onreadystatechange = function () {
                    var DONE = 4; // readyState 4 means the request is done.
                    var OK = 200; // status 200 is a successful return.
                    if (xhr.readyState === DONE) {
                        if (xhr.status === OK) {
                            try {
                                //console.log(xhr.responseText);
                                if (xhr.responseText !== "true") {
                                    if (warnSpan) {
                                        warnSpan.innerHTML = warnSpan.dataset.errorMessage;
                                    }

                                    inp.focus();
                                }
                                else {
                                    warnSpan.innerHTML = ''
                                }
                            }
                            catch (err) {
                                //console.log(err);
                            }
                            finally { }
                        }
                        else {
                            //console.log('Error: ' + xhr.statusText);
                        }
                    }
                };

                xhr.open('POST', validationUrl + query);
                xhr.send(null);
            }
          
        }, true);

    });

    

})();