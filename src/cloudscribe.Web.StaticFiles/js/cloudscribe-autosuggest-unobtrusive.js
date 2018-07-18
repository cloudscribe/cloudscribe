// Author: J Audette 2015-05-12
$(function () {
    var $elems = $('input[data-autosuggest-serviceurl]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var serviceUrl = $(ele).data('autosuggest-serviceurl')
            var labelProp = ($(ele).data('autosuggest-label-prop') || "label");
            var valueProp = ($(ele).data('autosuggest-value-prop') || "value");
            var parentId = $(ele).data('autosuggest-parentid');
            var parentDataName = $(ele).data('autosuggest-parent-data-name')
            $(ele).autocomplete({
                source: function (request, response) {
                    var d = { query: request.term };
                    if (parentId && parentDataName) {
                        var $parent = $('#' + parentId);
                        if($parent) {
                            d[parentDataName] = $parent.val();
                        }
                    }
                    $.ajax({
                        url: serviceUrl,
                        type: "POST",
                        dataType: "json",
                        data: d,
                        success: function (data, textStatus, jqXHR) {
                            response($.map(data, function (item) {
                                return { label: item[labelProp], value: item[valueProp] };
                            }))
                        },
                            error : function(jqXHR, textStatus, errorThrown) {
                                //alert(textStatus);
                            }
                    })
                }
            });
        });
    }
});
