// Author: J Audette 2016-06-22, 2018-07-27
$(function () {
    var $elems = $('input[data-bs-datepicker]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var debugMode = ($(ele).data('bs-datepicker-debug') || false);
            var dFormat = ($(ele).data('bs-datepicker-format') || false);
            var isoTargetId = $(ele).data('bs-datepicker-iso-target-id');
            var isoFormat = ($(ele).data('bs-datepicker-iso-format') || 'YYYY-MM-DD');
            var toggleTargetId = $(ele).data('bs-datepicker-toggle-target-id');
            var dviewMode = ($(ele).data('bs-datepicker-viewmode') || 'days');
            var ddayViewHeaderFormat = ($(ele).data('bs-datepicker-dayviewheaderformat') || 'MMMM YYYY');
            var dstepping = ($(ele).data('bs-datepicker-stepping') || 1);
            var dminDate = ($(ele).data('bs-datepicker-mindate') || false);
            var dmaxDate = ($(ele).data('bs-datepicker-maxdate') || false);
            var duseCurrent = ($(ele).data('bs-datepicker-usecurrent') == false ? false : true);
            var dcollapse = ($(ele).data('bs-datepicker-collapse') == false ? false : true);
            var dlocale = ($(ele).data('bs-datepicker-locale') || moment.locale());
            var ddefaultDate = ($(ele).data('bs-datepicker-defaultdate') || false);
            var ddisabledDates = ($(ele).data('bs-datepicker-disableddates') || false);
            var denabledDates = ($(ele).data('bs-datepicker-enableddates') || false);
            //var dicons
            var duseStrict = ($(ele).data('bs-datepicker-usestrict') || false);
            var dsideBySide = ($(ele).data('bs-datepicker-sidebyside') || false);
            var ddaysOfWeekDisabled = ($(ele).data('bs-datepicker-daysofweekdisabled') || []);
            var dcalendarWeeks = ($(ele).data('bs-datepicker-calendarWeeks') || false);
            var dtoolbarPlacement = ($(ele).data('bs-datepicker-toolbarplacement') || 'default');
            var dshowTodayButton = ($(ele).data('bs-datepicker-showtodaybutton') || false);
            var dshowClear = ($(ele).data('bs-datepicker-showclear') || false);
            var dshowClose = ($(ele).data('bs-datepicker-showclose') || false);
            
            var dkeepOpen = ($(ele).data('bs-datepicker-keepopen') || false);
            var dinline = ($(ele).data('bs-datepicker-inline') || false);
            var dkeepInvalid = ($(ele).data('bs-datepicker-keepinvalid') || false);
            var ddisabledTimeIntervals = ($(ele).data('bs-datepicker-disabledtimeintervals') || false);
            var dallowInputToggle = ($(ele).data('bs-datepicker-allowinputtoggle') || false);
            var dfocusOnShow = ($(ele).data('bs-datepicker-focusonshow') == false ? false : true);
            var denabledHours = ($(ele).data('bs-datepicker-enabledhours') || false);
            var ddisabledHours = ($(ele).data('bs-datepicker-disabledhours') || false);
            var dviewDate = ($(ele).data('bs-datepicker-viewdate') || false);
            var dUseFontAwesome = ($(ele).data('bs-datepicker-use-fontawesome') || true);
            //var posit = { horizontal: 'left', vertical: 'bottom' };
            
            var iconConfig = {
                time: "fa fa-clock-o",
                    today: "fa fa-crosshairs",
                    date: "fa fa-calendar",
                    up: "fa fa-arrow-up",
                    down: "fa fa-arrow-down",
                    previous: "fa fa-chevron-left",
                    next: "fa fa-chevron-right",
                    clear: "fa fa-trash",
                    close: "fa fa-times"
            };

            if (dUseFontAwesome) {
                iconConfig = {
                    time: "far fa-clock",
                    today: "fas fa-crosshairs",
                    date: "far fa-calendar-alt",
                    up: "fas fa-arrow-up",
                    down: "fas fa-arrow-down",
                    previous: "fas fa-chevron-left",
                    next: "fas fa-chevron-right",
                    clear: "far fa-trash-alt",
                    close: "fas fa-times"
                };
            }

            $(ele).datetimepicker({
                debug: debugMode,
                //widgetPositioning: posit,
                keepOpen: dkeepOpen,
                allowInputToggle: dallowInputToggle,
                format: dFormat,
                viewMode: dviewMode,
                dayViewHeaderFormat: ddayViewHeaderFormat,
                minDate: dminDate,
                maxDate: dmaxDate,
                useCurrent: duseCurrent,
                collapse: dcollapse,
                locale: dlocale,
                defaultDate: ddefaultDate,
                disabledDates: ddisabledDates,
                enabledDates: denabledDates,
                useStrict: duseStrict,
                sideBySide: dsideBySide,
                toolbarPlacement: dtoolbarPlacement,
                showTodayButton: dshowTodayButton,
                showClear: dshowClear,
                showClose: dshowClose,
                inline: dinline,
                keepInvalid: dkeepInvalid,
                disabledTimeIntervals: ddisabledTimeIntervals,
                focusOnShow: dfocusOnShow,
                enabledHours: denabledHours,
                disabledHours: ddisabledHours,
                viewDate: dviewDate,
                icons: iconConfig
            });

            if (isoTargetId !== undefined) {
                $(ele).on("dp.change", function (e) {
                    if (e.date) {
                        $('#' + isoTargetId).val(e.date.format(isoFormat));
                    }
                    else {
                        $('#' + isoTargetId).val('');
                    }
                    
                });
            }

            if (toggleTargetId) {
                $('#' + toggleTargetId).click(function () {
                    $(ele).data("DateTimePicker").toggle();
                });
            }


        });
    }
});
