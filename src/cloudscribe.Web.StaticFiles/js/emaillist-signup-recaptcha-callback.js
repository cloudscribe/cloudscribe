// getting inline script junk out of the UI - 
// though I'm not sure if this is ever actually getting called - jk
function onSignupSubmit(token) {
    var subForm = $("#subscribeForm");
    subForm.validate();
    if (subForm.valid()) {
        subForm.submit();
    }
    else {
        grecaptcha.reset();
    }
}