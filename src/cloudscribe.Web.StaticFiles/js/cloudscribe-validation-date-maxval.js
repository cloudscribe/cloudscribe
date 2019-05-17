$.validator.addMethod("datemaxval", function (value, element, param) {
    var otherPropId = $(element).data('val-datemaxval-otherproperty');
    var otherProp = $(otherPropId);
    if (otherProp.val() !== '') {
        var StartDate = new Date(otherProp.val());
        var Enddate = new Date(value);
        if (StartDate !== '') {
            var result = Enddate <= StartDate;
            return result;
        }
    }
    return true;
});

$.validator.unobtrusive.adapters.add('datemaxval', ['otherproperty'], function (options) {
    options.rules['datemaxval'] = { datemaxval: options.params.otherproperty };
    var sel = '#' + options.element.id;
    var cultureCode = $(sel).data('val-datemaxval-culture');
    var includeTime = $(sel).data('val-datemaxval-includetime');
    var otherProp = $(options.params.otherproperty).val();
    var dt = new Date(otherProp);
    if (includeTime === true) {
        options.messages['datemaxval'] = options.message.replace("MAXVALUE", dt.toLocaleString(cultureCode));
    } else {
        options.messages['datemaxval'] = options.message.replace("MAXVALUE", dt.toLocaleDateString(cultureCode));
    }
    
});
