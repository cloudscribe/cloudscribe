jQuery.validator.addMethod("comparewhen", function (value, element, param) {

    var whenPropId = $(element).data('val-comparewhen-whenproperty');
    var whenValue = $(element).data('val-comparewhen-whenvalue');
    if ((whenPropId) && (whenValue))
    {
        var whenProp = $(whenPropId);
        if (whenProp)
        {
            if (whenProp.val() == whenValue) { 
                // from jquery.validate equalTo method
                // bind to the blur event of the target in order to revalidate whenever the target field is updated
                // TODO find a way to bind the event just once, avoiding the unbind-rebind overhead
                var target = $(param);
                if (this.settings.onfocusout) {
                    target.unbind(".validate-equalTo").bind("blur.validate-equalTo", function () {
                        $(element).valid();
                    });
                }
                return value === target.val();
            }
        } 
    }
    
    return true;
});

jQuery.validator.unobtrusive.adapters.add("comparewhen",
["compareproperty"], function (options) {
    options.rules["comparewhen"] = options.params.compareproperty ;
    options.messages["comparewhen"] = options.message;
});
