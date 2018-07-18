(function () {

    var buttonManager = {

        enableButtons : function() {
            var buttons = $("form[data-submit-once='true']").find('button[type="submit"]');
            if (buttons) {
                buttons.each(function () {
                    var button = $(this);
                    //console.log('found button');
                    var enabledText = button.data("enabled-text");
                    if (enabledText) {
                        //console.log('set button enabled text');
                        window.setTimeout(function (button, buttonText) {
                            button.text(buttonText);
                        }, 500, button, enabledText);
                    }
                });
            }

            var inputs = $("form[data-submit-once='true']").find('input[type="submit"]');
            if (inputs) {
                inputs.each(function () {
                    var button = $(this);
                    //console.log('found button');
                    var enabledText = button.data("enabled-text");
                    //console.log(enabledText);
                    if (enabledText) {
                        //console.log('set button enabled text');
                        window.setTimeout(function (button, inputText) {
                            button.val(inputText);
                        }, 500, button, enabledText);
                    }
                });
            }
        },

         disableButtonsOnClick : function () {
            var buttons = $("form[data-submit-once='true']").find('button[type="submit"]');
            if (buttons) {
                buttons.each(function () {
                    var button = $(this);
                    button.on('click', function () {
                        var disabledText = button.data("disabled-text");
                        if (disabledText) {
                            button.text(disabledText);
                            //console.log('set button disabled text');  
                        }
                    });
                });
            }

            var inputs = $("form[data-submit-once='true']").find('input[type="submit"]');
            if (inputs) {
                inputs.each(function () {
                    var button = $(this);
                    button.on('click', function () {
                        var disabledText = button.data("disabled-text");
                        if (disabledText) {
                            button.val(disabledText);
                            //console.log('set button disabled text');  
                        }
                    });
                });
            }
        }
    };

    $(document).ready(function () {
        // running this overrides some jQuery Validate stuff so we can hook into its validations
        $("form[data-submit-once='true']").addTriggersToJqueryValidate();
        buttonManager.disableButtonsOnClick();
        $("form[data-submit-once='true']").formValidAndInvalid(function (valid) {
            //console.log('form was valid');
			// don't enable buttons form is about to submit
            //buttonManager.enableButtons()
        }, function (invalid) {
            //console.log('form was invalid');
            buttonManager.enableButtons()
            });
    });
})();
