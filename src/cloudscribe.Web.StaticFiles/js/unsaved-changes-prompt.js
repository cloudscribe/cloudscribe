window.thisPage = window.thisPage || {};
window.thisPage.hasUnsavedChanges = false;
window.thisPage.closeEditorWarning = function (event) {
    if (window.thisPage.hasUnsavedChanges)
        return 'It looks like you have been editing something' +
            ' - if you leave before saving, then your changes will be lost.'
    else
        return undefined;
};
window.onbeforeunload = window.thisPage.closeEditorWarning;