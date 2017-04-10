

http://nodatime.org/

https://github.com/exceptionless/Exceptionless.DateTimeExtensions

http://jeremybytes.blogspot.com/2015/05/fun-with-datetime-datetimekindunspecifi.html

https://github.com/sbstjn/timesheet.js

# Date/Time picker implementations

https://github.com/dbushell/Pikaday


Will be nice when all browsers support their own date time pickers for html 5 (I think only Opera has it).

For now we still need to implement our own date/time picker solutions.

## cloudscribe unobtrusive date picker implementation

/Scripts/cloudscribe-datepicker-unobtrusive.js
depends on jqueryui and trent richardson's time picker 

### Example Usage

@section Styles {
    @Styles.Render("~/Content/themes/base/all.css")
    @Styles.Render("~/Content/themes/base/dialog.css")
    @Styles.Render("~/Scripts/timepicker/jquery-ui-timepicker-addon.css")
}

@Html.TextBoxFor(model => model.TmpDate, 
	new { 
	@Value = Model.TmpDate.ToString("g"),
	@class = "form-control",
	@data_cs_datepicker="true",
	@data_cs_datepicker_show_time = "true",
	@data_cs_datepicker_show_time_only = "false",
	@data_cs_datepicker_dateformat = ViewBag.DateFormat,
	@data_cs_datepicker_timeformat = ViewBag.TimeFormat
	} )
	
@section Scripts {
    @Scripts.Render("~/bundles/jqueryajax")
    @Scripts.Render("~/bundles/jqueryui")
    @Scripts.Render("~/Scripts/timepicker/jquery-ui-timepicker-addon.js")
    @Scripts.Render("~/Scripts/jqueryui-i18n/datepicker-" + System.Globalization.CultureInfo.CurrentUICulture.Name + ".js")
    @Scripts.Render("~/Scripts/timepicker/i18n/jquery-ui-timepicker-" + System.Globalization.CultureInfo.CurrentUICulture.Name + ".js")
    @Scripts.Render("~/Scripts/cloudscribe-datepicker-unobtrusive.js")
}

using cloudscribe.Core.Web.Helpers;

string timeFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ToDatePickerWithTimeFormat();
ViewBag.TimeFormat = timeFormat;
string dateFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ToDatePickerFormat();
ViewBag.DateFormat = dateFormat;


**References**

https://github.com/chmln/flatpickr

https://jqueryui.com/datepicker/   **our implementation uses this**
http://api.jqueryui.com/datepicker/

http://trentrichardson.com/examples/timepicker/ **our implementation uses this**

http://adamalbrecht.com/2013/12/01/better-date-time-picker-in-angular-js/

https://github.com/adamalbrecht/ngQuickDate

https://github.com/g00fy-/angular-datepicker

https://github.com/telerik/kendo-ui-core apache 2 license

http://momentjs.com/docs/#/displaying/format/
http://eonasdan.github.io/bootstrap-datetimepicker/Options/#defaultdate

data-cs-datepicker="true"
data-cs-datepicker-show-time="false"
data-cs-datepicker-show-time-only="false"
data-cs-datepicker-show-month-list="true"
data-cs-datepicker-show-year-list="true"
data-cs-datepicker-year-range="c-100:c+0"
data-cs-datepicker-dateformat="@ViewBag.DateFormat"
data-cs-datepicker-timeformat="@ViewBag.TimeFormat" />