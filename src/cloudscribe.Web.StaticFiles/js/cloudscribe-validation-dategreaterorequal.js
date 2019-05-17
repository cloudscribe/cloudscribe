$.validator.addMethod("isgreaterorequal", function (value, element, param) {
    var otherPropId = $(element).data('val-otherproperty');
    var otherProp = $(otherPropId);
    if (otherProp.val() != '') {
        var StartDate = new Date(otherProp.val());
        var Enddate = new Date(value);
        if (StartDate != '') {
            var result = Enddate >= StartDate;
            return result;
        }
    }
    return true;
});

$.validator.unobtrusive.adapters.add('isgreaterorequal', ['otherproperty'], function (options) {
    options.rules['isgreaterorequal'] = { isgreaterorequal: options.params.otherproperty };
    options.messages['isgreaterorequal'] = options.message;
});
