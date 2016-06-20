
Remote Validation - example to check if a username is available
client side code calls a server action method that returns json
[Remote("ValidateUserName", "UserAdmin")] action method, controller name

see page 736 in PRO ASP.NET MVC 5

http://www.codeproject.com/Articles/674288/Remote-Validation-in-MVC-Simple-Way-to-Pass-the-F

http://www.tugberkugurlu.com/archive/asp-net-mvc-remote-validation-for-multiple-fields-with-additionalfields-property


http://www.prideparrot.com/blog/archive/2012/9/creating_custom_modelvalidatorprovider


conditional data annotations

https://github.com/JaroslawWaliszko/ExpressiveAnnotations

http://www.codeproject.com/Articles/541244/Conditional-ASP-NET-MVC-Validation-using-Data-Anno

http://xhalent.wordpress.com/2011/05/12/custom-unobstrusive-jquery-validation-in-asp-net-mvc-3-using-dataannotationsmodelvalidatorprovider/



example js for RequiredIfAttribute.cs
http://mvcdiary.com/2013/02/28/conditional-required-validation-or-field-mandatory-depends-on-another-field-mvc-4/


$.validator.addMethod('requiredif',
    function (value, element, parameters) {
        var id = '#' + parameters['dependentproperty'];

        // get the target value (as a string, 
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue = (targetvalue == null ? '' : targetvalue).toString();

        // get the actual value of the target control
        // note - this probably needs to cater for more 
        // control types, e.g. radios
        var control = $(id);
        var controltype = control.attr('type');
        var actualvalue =
            (controltype === 'checkbox' ||  controltype === 'radio')  ?
            control.attr('checked').toString() :
            control.val();

        // if the condition is true, reuse the existing 
        // required field validator functionality
        if ($.trim(targetvalue) === $.trim(actualvalue) || ($.trim(targetvalue) === '*' && $.trim(actualvalue) !== ''))
            return $.validator.methods.required.call(
              this, value, element, parameters);

        return true;
    });

$.validator.unobtrusive.adapters.add(
    'requiredif',
    ['dependentproperty', 'targetvalue'],
    function (options) {
        options.rules['requiredif'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredif'] = options.message;
    });
