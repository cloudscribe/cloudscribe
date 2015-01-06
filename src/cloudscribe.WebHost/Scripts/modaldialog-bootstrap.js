function prepareModalDialog(dialogDivId) {
    var $wrapper = $("<div/>", { id: dialogDivId, class: "modal fade", tabindex: "-1" });
    $(document.body).append($wrapper);
}

function clearModalDialog(dialogDivId) {
    $("#" + dialogDivId).remove();
}

function setFormDataAjaxAttributes(dialogDivId) {
    //var div = $("#" + dialogDivId);
    //div.find("form").attr("data-ajax-update", "#" + dialogDivId);
    //div.find("form").attr("data-ajax-complete", "onModalDialogSubmitted('" + dialogDivId + "')");
}

function onModalDialogSubmitted(dialogDivId) {
    var div = $("#" + dialogDivId);
    var result = div.find("div[data-dialog-close='true']");
    if (result.length == 0)
        setFormDataAjaxAttributes(dialogDivId);
    else {
        $("#" + dialogDivId).dialog('close');
        var message = result.attr("data-dialog-result");
        if (message.length > 0)
            alert(message);
    }
}

function openModalDialog(dialogDivId) {
    var div = $("#" + dialogDivId);
    div.modal({ show: true, backdrop:true });

    div.on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
        clearModalDialog(dialogDivId);
    });

    setFormDataAjaxAttributes(dialogDivId);
}
