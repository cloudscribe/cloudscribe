// Author: J Audette 2015-05-15
$(function () {
    var $elems = $('input[data-cs-datepicker]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var showTime = $(ele).data('cs-datepicker-show-time');
            var showTimeOnly = $(ele).data('cs-datepicker-show-time-only');
            var dFormat = ($(ele).data('cs-datepicker-dateformat') || "mm/dd/yy");
            if (showTime) {
                var tFormat = ($(ele).data('cs-datepicker-timeformat') || "HH:mm");
                if (showTimeOnly) {
                    $(ele).timepicker({
                        timeFormat: tFormat,
                        dateFormat: dFormat
                    });
                }
                else {
                    $(ele).datetimepicker({
                        timeFormat: tFormat,
                        dateFormat: dFormat
                    });
                }  
            }
            else {

                $(ele).datepicker({
                    dateFormat: dFormat
                });
            }
            
        });
    }
});
