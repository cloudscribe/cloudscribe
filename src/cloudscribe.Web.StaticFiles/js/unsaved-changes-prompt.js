window.thisPage = window.thisPage || {};
window.thisPage.hasUnsavedChanges = false;
window.thisPage.formSubmitting = false;
window.thisPage.closeEditorWarning = function (event) {
    if (window.thisPage.formSubmitting) {
        return undefined;
    }
    if (window.thisPage.hasUnsavedChanges)
        return 'It looks like you have been editing something' +
            ' - if you leave before saving, then your changes will be lost.'
    else
        return undefined;
};
window.onbeforeunload = window.thisPage.closeEditorWarning;
document.addEventListener("DOMContentLoaded", function () {
    [].forEach.call(document.querySelectorAll("input"), function (input) {
        input.addEventListener("change", function () {
            window.thisPage = window.thisPage || {};
            window.thisPage.hasUnsavedChanges = true;
        }, false);
    });
});