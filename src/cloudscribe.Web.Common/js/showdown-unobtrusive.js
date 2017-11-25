// Author: Joe Audette
// Created 2017-10-27
// Last Modified: 2017-10-27
$(function () {
    var $elems = $('div[data-mdurl]');
    if ($elems) {
        var converter = new showdown.Converter();
        $elems.each(function (index, ele) {
            var $url = $(ele).data('mdurl');
            $.get($url, function (data) {
                ele.innerHTML = converter.makeHtml(data);
            }, 'text');
        });
     }
});