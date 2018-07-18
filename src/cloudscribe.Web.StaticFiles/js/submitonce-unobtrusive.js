(function () {
    var forms = document.getElementsByTagName('form');
    for (var i = forms.length; i--;) {
        var form = forms[i];
        if (form.dataset.submitOnce) {
            form.addEventListener('submit', function (e) {
                //e.preventDefault();
                var buttons = this.querySelectorAll('button[type="submit"]');
                [].forEach.call(buttons, function (button) {
                    button.setAttribute('disabled', 'disabled');
                    if (button.dataset.disabledText) {
                        button.innerHTML = button.dataset.disabledText;
                    }
                });
                var inputs = this.querySelectorAll('input[type="submit"]');
                [].forEach.call(inputs, function (button) {
                    button.setAttribute('disabled', 'disabled');
                    if (button.dataset.disabledText) {
                        button.setAttribute('value', button.dataset.disabledText);
                    }

                });
            }, false);
        }  
    }
})();
