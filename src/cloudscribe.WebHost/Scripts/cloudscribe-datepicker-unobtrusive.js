// Author: J Audette 2015-05-15
$(function () {
    var $elems = $('input[data-cs-datepicker]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var showTime = $(ele).data('cs-datepicker-show-time');
            var showTimeOnly = $(ele).data('cs-datepicker-show-time-only');
            var dFormat = ($(ele).data('cs-datepicker-dateformat') || "mm/dd/yy");
            var showMonthList = $(ele).data('cs-datepicker-show-month-list');
            var showYearList = $(ele).data('cs-datepicker-show-year-list');
            var yrRange = ($(ele).data('cs-datepicker-year-range') || "c-10:c+10");
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
                        dateFormat: dFormat,
                        changeMonth: showMonthList,
                        changeYear: showYearList,
                        yearRange: yrRange
                    });
                }  
            }
            else {

                $(ele).datepicker({
                    dateFormat: dFormat,
                    changeMonth: showMonthList,
                    changeYear: showYearList,
                    yearRange: yrRange
                });
            }
            
        });
    }
});
