// Author: J Audette 2015-05-07,2017-12-14
$(function () {
    var $elems = $('select[data-cascade-childof]');
    if ($elems) {
        //console.log("found cascade child");
        $elems.each(function (index, ele) {
            var $parent = $('#' + $(ele).data('cascade-childof'));
            var serviceUrl = $(ele).data('cascade-serviceurl');
            var origVal = $(ele).data('cascade-orig-val');
            var selectLabel = $(ele).data('cascade-select-label');
            var disableOnEmptyParent = $(ele).data('cascade-disableonemptyparent');
            var emptyParentValue = $(ele).data('cascade-parent-emptyvalue');
            var triggerChangeOnDataLoad = $(ele).data('cascade-trigger-change-after-load');
            $parent.change(function () {
                //console.log("cascade parent changed to " + $parent.val());
                $.getJSON(serviceUrl + $parent.val(), function (data) {
                    var items = "<option value=''>" + selectLabel + "</option>";
                    $.each(data, function (i, item) {
                        items += "<option value='" + item.value + "'>" + item.text + "</option>";
                    });
                    $(ele).html(items);
                    if (origVal && origVal.length > 0) {
                        var found = $(ele).find("option[value=" + origVal + "]").length > 0;
                        if (found) {
                            $(ele).val(origVal);
                        }
                    }
                    
                    if (triggerChangeOnDataLoad) {
                        $(ele).change();
                    }
                   
                });
                if (disableOnEmptyParent) {
                    var emptyParent = ($parent.val() === emptyParentValue);
                    if (emptyParent) {
                        $(ele).prop("disabled", true);
                    }
                    else {
                        $(ele).prop("disabled", false);
                    }
                }

            });
        });
    }
});
