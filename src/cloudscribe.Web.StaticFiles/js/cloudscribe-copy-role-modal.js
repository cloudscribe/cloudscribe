// cloudscribe Copy Role Modal Handler
// Handles populating the copy role modal with source role data

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        var copyRoleModal = document.getElementById('copyRoleModal');
        if (copyRoleModal) {
            copyRoleModal.addEventListener('show.bs.modal', function (event) {
                var button = event.relatedTarget;
                var roleId = button.getAttribute('data-role-id');
                var roleName = button.getAttribute('data-role-name');

                // Set hidden fields
                document.getElementById('sourceRoleId').value = roleId;
                document.getElementById('sourceRoleName').value = roleName;

                // Set default new role name
                document.getElementById('newRoleName').value = roleName + '-Copy';

                // Select the text for easy editing
                setTimeout(function () {
                    document.getElementById('newRoleName').select();
                }, 100);
            });
        }
    });
})();
