jQuery.validator.addMethod("enforcetrue", function (value, element, param) {  
    return element.checked;
});
jQuery.validator.unobtrusive.adapters.addBool("enforcetrue");
