jQuery.validator.addMethod("requiredwhen", function (value, element, param) {
    var otherPropId = $(element).data('val-other');
	var otherPropVal = $(element).data('val-otherval');
    if (otherPropId) {
        var otherProp = $(otherPropId);
        if (otherProp) {
            var otherVal = otherProp.val();
            if (otherVal === otherPropVal) {
                return element.value.length > 0;
            }
        }
    }
    return true;
});
jQuery.validator.unobtrusive.adapters.addBool("requiredwhen");
