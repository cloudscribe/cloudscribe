jQuery.validator.addMethod("comparewhen", function (value, element, params) {

    alert(params);
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

    // debugger;
    /*
    var propelename = params.split(",")[0];
    var operName = params.split(",")[1];
    if (params == undefined || params == null || params.length == 0 ||
    value == undefined || value == null || value.length == 0 ||
    propelename == undefined || propelename == null || propelename.length == 0 ||
    operName == undefined || operName == null || operName.length == 0)
        return true;
    var valueOther = $(propelename).val();
    var val1 = (isNaN(value) ? Date.parse(value) : eval(value));
    var val2 = (isNaN(valueOther) ? Date.parse(valueOther) : eval(valueOther));

    if (operName == "GreaterThan")
        return val1 > val2;
    if (operName == "LessThan")
        return val1 < val2;
    if (operName == "GreaterThanOrEqual")
        return val1 >= val2;
    if (operName == "LessThanOrEqual")
        return val1 <= val2;
        */
});

jQuery.validator.unobtrusive.adapters.add("comparewhen",
["whenproperty", "whenvalue", "compareproperty"], function (options) {
    options.rules["comparewhen"] = "#" +
    options.params.whenproperty + "," + options.whenvalue + "," + options.params.compareproperty;
    options.messages["comparewhen"] = options.message;
});
