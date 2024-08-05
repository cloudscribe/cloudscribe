// getting inline script stuff out of the UI - 
// so that a CSP preventing unsafe-inline js might be feasible

$(function () {

    // get rid of onClick references in the BS5 views
    var ele_delete  = document.getElementById("btn-delete-subscription");
    var deleteform  = document.getElementById("deleteform");
    var ele_confirm = document.getElementById("btn-confirm-subscription");
    var confirmform = document.getElementById("confirmform");
    var ele_send    = document.getElementById("btnSend");


    if (ele_delete != null & ele_delete != undefined) {
        ele_delete.addEventListener("click", function () {
            var message = ele_delete.dataset.message;
            if (confirm(message)) {
                deleteform.submit();
            }
        });
    }

    if (ele_confirm != null & ele_confirm != undefined) {
        ele_confirm.addEventListener("click", function () {
            var message = ele_confirm.dataset.message;
            if (confirm(message)) {
                confirmform.submit();
            }
        });
    }

    if (ele_send != null & ele_send != undefined) {
        ele_send.addEventListener("click", function () {
            var message = ele_send.dataset.sendprompt;
            return confirm(message);
        });
    }

})();





