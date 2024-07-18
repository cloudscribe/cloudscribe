//$(function () {
//    // get rid of onClick references in the BS5 views
//    // no this isn't working and too obscure to trace its usages across everything right now - jk
//    var elements = $(".btn-modal-userselect");

//    elements.each(function () {
//        $(this).on('click', function() {
//            if (window.UserSelectedCallback) {
//                var userInfo = {
//                    "id": $(this).data.userId,
//                    "email": $(this).data.userEmail,
//                    "displayName": $(this).data.userDisplayName,
//                    "firstName": $(this).data.userFirstName,
//                    "lastName": $(this).data.userLastName
//                };
//                window.UserSelectedCallback(userInfo);
//            }
//            $('#userLookupModal').closest(".modal").modal("hide");
//        })
//    })
//});


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


