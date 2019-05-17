$.validator.addMethod("dateminval", function (value, element, param) {
    //console.log(param);
    var otherPropId = $(element).data('val-dateminval-otherproperty');
    var otherProp = $(otherPropId);
    if (otherProp.val() !== '') {
        var StartDate = new Date(otherProp.val());
        var Enddate = new Date(value);
        if (StartDate !== '') {
            var result = Enddate >= StartDate;
            return result;
        }
    }
    return true;
});

$.validator.unobtrusive.adapters.add('dateminval', ['otherproperty'], function (options) {
    //console.log(options.element.id);
    options.rules['dateminval'] = { dateminval: options.params.otherproperty };
    var sel = '#' + options.element.id;
    var cultureCode = $(sel).data('val-dateminval-culture');
    var includeTime = $(sel).data('val-dateminval-includetime');
    var otherProp = $(options.params.otherproperty).val();
    var dt = new Date(otherProp);
    if (includeTime === true) {
        options.messages['dateminval'] = options.message.replace("MINVALUE", dt.toLocaleString(cultureCode));
    } else {
        options.messages['dateminval'] = options.message.replace("MINVALUE", dt.toLocaleDateString(cultureCode));
    }
    
});
