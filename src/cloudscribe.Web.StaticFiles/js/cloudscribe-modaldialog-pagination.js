// handles selected checkboxes not getting persisted when paginating across a list
// by collating them into any dom element #savedCheckboxes

$(document).ready(function () {
    $('#savedCheckboxes').find('p').each(function () {
        var savedCheckbox = $(this).text();
        $("input#" + savedCheckbox).prop("checked", true);
    });

    $(document).on('click', '.PersistedCheckbox', function (e) {
        if (e.target.checked) {
            if ($('#savedCheckboxes > #p_' + e.target.id).length == 0) {
                $('#savedCheckboxes').append('<p id=\'p_' + e.target.id + '\'>' + e.target.id + '</p>');
            }
        }
        else {
            $('#savedCheckboxes > #p_' + e.target.id).remove();
        }
    });

    $(document).on('click', '#SubmitPagedCheckboxes', function (e) {
        var collation = '';
        $('#savedCheckboxes').find('p').each(function () {
            var savedCheckbox = $(this).text() + ",";
            collation += savedCheckbox;
        });
        $('#SelectedCheckboxesCSV').val(collation);
    });

});
