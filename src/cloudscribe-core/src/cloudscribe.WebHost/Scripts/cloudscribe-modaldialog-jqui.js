function prepareModalDialog(dialogDivId) {
    $(document.body).append('<div id="' + dialogDivId + '"></div>');
}

function clearModalDialog(dialogDivId) {
    $("#" + dialogDivId).remove();
}

function setFormDataAjaxAttributes(dialogDivId) {
    var div = $("#" + dialogDivId);
    div.find("form").attr("data-ajax-update", "#" + dialogDivId);
    div.find("form").attr("data-ajax-complete", "onModalDialogSubmitted('" + dialogDivId + "')");
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

function openModalDialog(dialogDivId, title) {
    var div = $("#" + dialogDivId);
    div.dialog({
        title: title,
        close: new Function("clearModalDialog('" + dialogDivId + "');"),
        modal: true,
        width: "auto",
        height: "auto",
        resizable: false
    });

    setFormDataAjaxAttributes(dialogDivId);
}
