// Author: J Audette 2016-06-22
$(function () {
    var $elems = $('input[data-bs-datepicker]');
    if ($elems) {
        $elems.each(function (index, ele) {
		    var debugMode = ($(ele).data('bs-datepicker-debug') || false);
		    var dFormat = ($(ele).data('bs-datepicker-format') || false);
            var dviewMode = ($(ele).data('bs-datepicker-viewMode') || 'days');
            var ddayViewHeaderFormat = ($(ele).data('bs-datepicker-dayViewHeaderFormat') || 'days');
            var dstepping = ($(ele).data('bs-datepicker-stepping') || 1);
            var dminDate = ($(ele).data('bs-datepicker-minDate') || false);
			var dmaxDate = ($(ele).data('bs-datepicker-maxDate') || false);
			var duseCurrent = ($(ele).data('bs-datepicker-useCurrent') || true);
			var dcollapse = ($(ele).data('bs-datepicker-collapse') || true);
			var dlocale = ($(ele).data('bs-datepicker-locale') || moment.locale());
			var ddefaultDate = ($(ele).data('bs-datepicker-defaultDate') || false);
			var ddisabledDates = ($(ele).data('bs-datepicker-disabledDates') || false);
			var denabledDates = ($(ele).data('bs-datepicker-enabledDates') || false);
			//var dicons
			var duseStrict = ($(ele).data('bs-datepicker-useStrict') || false);
			var dsideBySide = ($(ele).data('bs-datepicker-sideBySide') || false);
			var ddaysOfWeekDisabled = ($(ele).data('bs-datepicker-daysOfWeekDisabled') || []);
			var dcalendarWeeks = ($(ele).data('bs-datepicker-calendarWeeks') || false);
			var dtoolbarPlacement = ($(ele).data('bs-datepicker-toolbarPlacement') || 'default');
			var dshowTodayButton = ($(ele).data('bs-datepicker-showTodayButton') || false);
			var dshowClear = ($(ele).data('bs-datepicker-showClear') || false);
			var dshowClose = ($(ele).data('bs-datepicker-showClose') || false);
			var dwidgetPositioning = ($(ele).data('bs-datepicker-widgetPositioning') || {horizontal: 'auto',vertical: 'auto'});
			var dkeepOpen = ($(ele).data('bs-datepicker-keepOpen') || false);
			var dinline = ($(ele).data('bs-datepicker-inline') || false);
			var dkeepInvalid = ($(ele).data('bs-datepicker-keepInvalid') || false);
			var ddisabledTimeIntervals = ($(ele).data('bs-datepicker-disabledTimeIntervals') || false);
			var dallowInputToggle = ($(ele).data('bs-datepicker-allowInputToggle') || false);
			var dfocusOnShow = ($(ele).data('bs-datepicker-focusOnShow') || true);
			var denabledHours = ($(ele).data('bs-datepicker-enabledHours') || false);
			var ddisabledHours = ($(ele).data('bs-datepicker-disabledHours') || false);
			var dviewDate = ($(ele).data('bs-datepicker-viewDate') || false);
            
			$(ele).datetimepicker({
			    debug: debugMode,
				widgetPositioning: dwidgetPositioning,
				keepOpen: dkeepOpen,
                allowInputToggle:dallowInputToggle
				format: dFormat,
				viewMode : dviewMode,
				dayViewHeaderFormat : ddayViewHeaderFormat,
				minDate : dminDate,
				maxDate : dmaxDate,
				useCurrent : duseCurrent,
				collapse : dcollapse,
				locale : dlocale,
				defaultDate : ddefaultDate,
				disabledDates : ddisabledDates,
				enabledDates : denabledDates,
				useStrict : duseStrict,
				sideBySide : dsideBySide,
				toolbarPlacement : dtoolbarPlacement,
				showTodayButton : dshowTodayButton,
				showClear : dshowClear,
				showClose : dshowClose,
				inline : dinline,
				keepInvalid : dkeepInvalid,
				disabledTimeIntervals : ddisabledTimeIntervals,
				focusOnShow : dfocusOnShow,
				enabledHours : denabledHours,
				disabledHours : ddisabledHours,
				viewDate : dviewDate
			});      
        });
    }
});
