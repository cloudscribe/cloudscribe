$(function () {
    // get rid of onClick references in the BS5 views
    // this is used in some hideous ajax user selection grid in obscure other places scattered around elsewhere - jk
    var elements = $(".btn-modal-userselect");

    elements.each(function () {
        $(this).on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            if (window.UserSelectedCallback) {
                var userInfo = {
                    "id":          $(this).data('userId'),
                    "email":       $(this).data('userEmail'),
                    "displayName": $(this).data('userDisplayName'),
                    "firstName":   $(this).data('userFirstName'),
                    "lastName":    $(this).data('userLastName')
                };
                window.UserSelectedCallback(userInfo);
            }
            $('#userLookupModal').closest(".modal").modal("hide");
        })
    })
});


// this is the older version of the same
function userSelected(ele) {
    if (window.UserSelectedCallback) {
        var userInfo = {
            "id": ele.dataset.userId,
            "email": ele.dataset.userEmail,
            "displayName": ele.dataset.userDisplayName,
            "firstName": ele.dataset.userFirstName,
            "lastName": ele.dataset.userLastName
        };
        window.UserSelectedCallback(userInfo);
    }
    $('#userLookupModal').closest(".modal").modal("hide");
}


