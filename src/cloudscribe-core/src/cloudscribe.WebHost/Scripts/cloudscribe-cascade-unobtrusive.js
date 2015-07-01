// Author: J Audette 2015-05-07
$(function () {
    var $elems = $('select[data-cascade-childof]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var $parent = $('#' + $(ele).data('cascade-childof'));
            var serviceUrl = $(ele).data('cascade-serviceurl');
            var origVal = $(ele).data('cascade-orig-val');
            var selectLabel = $(ele).data('cascade-select-label');
            $parent.change(function () {
                $.getJSON(serviceUrl + $parent.val(), function (data) {
                    var items = '<option>' + selectLabel + '</option>';
                    $.each(data, function (i, item) {
                        items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    });
                    $(ele).html(items);
                    if (origVal.length > 0) {
                        var found = $(ele).find("option[value=" + origVal + "]").length > 0;
                        if (found) {
                            $(ele).val(origVal);
                        }
                    }
                });
            });
        });
    }
});