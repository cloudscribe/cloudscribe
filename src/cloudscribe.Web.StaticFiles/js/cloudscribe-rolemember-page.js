$('#confirmAddRoleModal').on('show.bs.modal', function (e) {
    var message = $(e.relatedTarget).data('message');
    var formId = $(e.relatedTarget).data('form-id');
    $(e.currentTarget).find('span[id="spnMessage"]').html(message);
    $("#btnAddUser").click(function () {
        //alert(formId);
        $("#" + formId).submit();
    });
});
