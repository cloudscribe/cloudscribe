(function ($) {

    $.fn.addTriggersToJqueryValidate = function () {

        // Loop thru the elements that we jQuery validate is attached to
        // and return the loop, so jQuery function chaining will work.
        return this.each(function () {
            var form = $(this);

            // Grab this element's validator object (if it has one)
            var validator = form.data('validator');

            // Only run this code if there's a validator associated with this element
            if (!validator)
                return;

            // Only add these triggers to each element once
            if (form.data('jQueryValidateTriggersAdded'))
                return;
            else
                form.data('jQueryValidateTriggersAdded', true);

            // Override the function that validates the whole form to trigger a 
            // formValidation event and either formValidationSuccess or formValidationError
            var oldForm = validator.form;
            validator.form = function () {
                var result = oldForm.apply(this, arguments);
                var form = this.currentForm;
                $(form).trigger((result == true) ? 'formValidationSuccess' : 'formValidationError', form);
                $(form).trigger('formValidation', [form, result]);
                return result;
            };

            // Override the function that validates the whole element to trigger a 
            // elementValidation event and either elementValidationSuccess or elementValidationError
            var oldElement = validator.element;
            validator.element = function (element) {
                var result = oldElement.apply(this, arguments);
                $(element).trigger((result == true) ? 'elementValidationSuccess' : 'elementValidationError', element);
                $(element).trigger('elementValidation', [element, result]);
                return result;
            };
        });
    };

    /* Below here are helper methods for calling .bind() for you */

    $.fn.extend({

        // Wouldn't it be nice if, when the full form's validation runs, it triggers the 
        // element* validation events?  Well, that's what this does!
        //
        // NOTE: This is VERY coupled with jquery.validation.unobtrusive and uses its 
        //       element attributes to figure out which fields use validation and 
        //       whether or not they're currently valid.
        //
        triggerElementValidationsOnFormValidation: function () {
            return this.each(function () {
                $(this).bind('formValidation', function (e, form, result) {
                    $(form).find('*[data-val=true]').each(function (i, field) {
                        if ($(field).hasClass('input-validation-error')) {
                            $(field).trigger('elementValidationError', field);
                            $(field).trigger('elementValidation', [field, false]);
                        } else {
                            $(field).trigger('elementValidationSuccess', field);
                            $(field).trigger('elementValidation', [field, true]);
                        }
                    });
                });
            });
        },

        formValidation: function (fn) {
            return this.each(function () {
                $(this).bind('formValidation', function (e, element, result) { fn(element, result); });
            });
        },

        formValidationSuccess: function (fn) {
            return this.each(function () {
                $(this).bind('formValidationSuccess', function (e, element) { fn(element); });
            });
        },

        formValidationError: function (fn) {
            return this.each(function () {
                $(this).bind('formValidationError', function (e, element) { fn(element); });
            });
        },

        formValidAndInvalid: function (valid, invalid) {
            return this.each(function () {
                $(this).bind('formValidationSuccess', function (e, element) { valid(element); });
                $(this).bind('formValidationError', function (e, element) { invalid(element); });
            });
        },

        elementValidation: function (fn) {
            return this.each(function () {
                $(this).bind('elementValidation', function (e, element, result) { fn(element, result); });
            });
        },

        elementValidationSuccess: function (fn) {
            return this.each(function () {
                $(this).bind('elementValidationSuccess', function (e, element) { fn(element); });
            });
        },

        elementValidationError: function (fn) {
            return this.each(function () {
                $(this).bind('elementValidationError', function (e, element) { fn(element); });
            });
        },

        elementValidAndInvalid: function (valid, invalid) {
            return this.each(function () {
                $(this).bind('elementValidationSuccess', function (e, element) { valid(element); });
                $(this).bind('elementValidationError', function (e, element) { invalid(element); });
            });
        }

    });

})(jQuery);
