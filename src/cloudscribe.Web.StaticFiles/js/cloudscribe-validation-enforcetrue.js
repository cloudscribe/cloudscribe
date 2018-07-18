jQuery.validator.addMethod("enforcetrue", function (value, element, param) {
    var otherPropId = $(element).data('val-other');
    if (otherPropId) {
        var otherProp = $(otherPropId);
        if (otherProp) {
            var otherVal = otherProp.val();
            if (otherVal === 'True') {
                return element.checked;
            }
        }
    }
    return true;
});
jQuery.validator.unobtrusive.adapters.addBool("enforcetrue");
