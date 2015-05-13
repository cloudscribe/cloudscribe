// Author: J Audette 2015-05-13
$(function () {
    var $elems = $('input[data-cs-datepicker]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var showTime = $(ele).data('cs-datepicker-show-time');
            

            if (showTime) {
                var tFormat = ($(ele).data('cs-datepicker-timeformat') || "HH:mm");
                alert(tFormat);

                $(ele).datetimepicker({
                    timeFormat: tFormat
                });
            }
            else {

                $(ele).datepicker({

                });
            }
            
        });
    }
});
