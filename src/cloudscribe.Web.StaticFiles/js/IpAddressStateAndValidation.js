function EditItem(id) {
    $('#EditButton' + id).hide();
    $('#EditingButtons' + id).show();
    $('#EditingButtons' + id).closest('.row').find('input, textarea').prop('disabled', false);
    $('#ipRangeSwitch' + id).show();
};
function CancelEditing(id) {
    $('#EditButton' + id).show();
    $('#EditingButtons' + id).hide();
    $('#EditingButtons' + id).closest('.row').find('input, textarea').prop('disabled', true);
    $('#ipRangeSwitch' + id).hide();
};
$('#singleIpAddress').click(function () {
    $('#addNewSingleIp').show();
    $('#addNewIpRange').hide();
    $('#ipTypeNextButton').prop('disabled', false);
});
$('#ipAddressRange').click(function () {
    $('#addNewSingleIp').hide();
    $('#addNewIpRange').show();
    $('#ipTypeNextButton').prop('disabled', false);
});
$('#ipAddressEntryBackButton').click(function () {
    $('#saveNew').prop('disabled', true);
    $('#IpAddressNew').val('');
    $('#IpAddressNew').removeClass('is-valid');
    $('#IpAddressNew').removeClass('is-invalid');
    $('#errorMsg').css('display', 'none');
    $('#errorMsg').html('');
});

document.getElementById('DeleteConfirmation').addEventListener('show.bs.modal', (e) => {
    $(".modal-body #ipAddressId").val(e.relatedTarget.dataset.id);
});

$(document).on('focusout', '#IpAddressNew', function () {
    const ipv4 = /^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$/;
    const ipv6 = /(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))/;

    if ($('#singleIpAddress').is(':checked')) {
        if (ipv4.test($('#IpAddressNew').val()) || ipv6.test($('#IpAddressNew').val())) {
            $('#saveNew').prop('disabled', false);
            $('#IpAddressNew').addClass('is-valid');
            $('#IpAddressNew').removeClass('is-invalid');
            $('#errorMsg').css('display', 'none');
            $('#errorMsg').html('');

            return true;
        } else {
            $('#saveNew').prop('disabled', true);
            $('#IpAddressNew').addClass('is-invalid');
            $('#IpAddressNew').removeClass('is-valid');
            $('#errorMsg').css('display', 'block');
            $('#errorMsg').html('<div class="col" style="color: red">Invalid IP Address.</div>');

            return false;
        }
    }
    else {
        $('#saveNew').prop('disabled', false);
        $('#errorMsg').css('display', 'none');
        $('#errorMsg').html('');
    }
});