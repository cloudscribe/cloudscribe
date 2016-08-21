var roleSelector = {
    modalSelector: "#roledialog",
    targetSelector:"#hdnSelected",
    prepareModal: function (dialogDivId, targetId) {
        this.modalSelector = "#" + dialogDivId;
        this.targetSelector = "#" + targetId;
        var $wrapper = $("<div/>", { id: dialogDivId, class: "modal fade", tabindex: "-1" });
        $(document.body).append($wrapper);

        $(this.modalSelector).on('show.bs.modal', function (e) {
            roleSelector.syncChanges();
        });
    },
    clearModal: function() {
        $(this.modalSelector).remove();
    },
    openModal:function() {
        var div = $(this.modalSelector);
        div.modal({ show: true, backdrop: true });

        div.on('hidden.bs.modal', function () {
            $(this).removeData('bs.modal');
            roleSelector.clearModal();
        });

        roleSelector.bindClose();
    },

    bindClose: function()
    {
        $(".closeModal").click(function () {
            $(this).closest(".modal").modal("hide")
        });
    },
    syncChanges: function() {
        //alert('sync it');
        var csvValue = $(roleSelector.targetSelector).val();
        var roles;
        if (csvValue) {
            roles = csvValue.split(',');
        }
        else
        {
            roles = [];
        }
        // check the boxes for any in the array
        for (var i = 0; i < roles.length; i++) {
            var role = roles[i];
            $(roleSelector.modalSelector + " input:checkbox[data-rolename='" + role + "']").attr('checked', 'checked');
        }

        $(roleSelector.modalSelector + " input:checkbox").change(function () {
            var unchecked = $(":checkbox:not(checked)").map(function () {
                return $(this).data("rolename");
            }).get();
        
            var filtered = $(roles).not(unchecked).get();
            roles = filtered;
            var newSelection = $(":checkbox:checked").map(function () {
                return $(this).data("rolename");
            }).get();
            var result = roleSelector.uniqueMerge(roles, newSelection);
            $(roleSelector.targetSelector).val(result.join(","));
        });

    },
    uniqueMerge: function (a1, a2) {
        var hash = {};
        var arr = [];
        for (var i = 0; i < a1.length; i++) {
            if (hash[a1[i]] !== true) {
                hash[a1[i]] = true;
                arr[arr.length] = a1[i];
            }
        }
        for (var i = 0; i < a2.length; i++) {
            if (hash[a2[i]] !== true) {
                hash[a2[i]] = true;
                arr[arr.length] = a2[i];
            }
        }
        return arr;
    }

};
