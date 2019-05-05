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
