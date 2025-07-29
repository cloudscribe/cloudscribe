(function () {
    var fileMover = {
        headers: {
            'X-CSRFToken': $("#fileMoverConfig").data("anti-forgery-token")
        },
        treeDataApiUrl: $("#fileMoverConfig").data("filetree-url"),
        canSelect: $("#fileMoverConfig").data("can-select"),
        rootVirtualPath: $("#fileMoverConfig").data("root-virtual-path"),
        fileSelectorButton: $('#btnSelector'),
        fileSelectorButtonAlt: $('#btnSelectorAlt'),
        moveFileButton: $('#btnMoveFile'),
        moveFilePromptButton: $('#btnMoveFilePrompt'),
        moveFileApiUrl: $("#fileMoverConfig").data("movefile-url"),
        selectedFileInput: $("#fileSelection"),
        progressUI: $('#progress'),
        refresh: $('#btnRefresh'),
        setFolderToMoveTo: function (virtualPath) {
            $("#folderToMoveTo").val(virtualPath);
        },
        clearFolderToMoveTo: function () {
            $("#folderToMoveTo").html('');
        },
        notify: function (message, cssClass) {
            $('#alert_placeholder').html('<div class="alert ' + cssClass + '"><button type="button" data-bs-dismiss="alert" class="btn-close me-2" style="float:right" aria-label="Close"></button><span>' + message + '</span></div>');
        },
        moveFilePrompt: function () {
            var currentPath = $("#fileToDelete").val();

            if (currentPath === '') {
                return false;
            }

            // bug that if new folders have been added in the main FM tree, 
            // they won't show in the file mover tree unless we reload.
            this.loadTree();

            $("#mdlMoveFile").modal('show');

            return false;
        },
        moveFile: function () {
            $("#mdlMoveFile").modal('hide');
            var currentPath = $("#fileToMove").val();

            if (currentPath === '') {
                return false;
            }
            if (currentPath === fileMover.rootVirtualPath) {
                return false;
            }

            var formData = $('#frmMoveFile').serializeArray();

            $.ajax({
                method: "POST",
                url: fileMover.moveFileApiUrl,
                headers: fileMover.headers,
                data: formData
            }).done(function (data) {
                if (data.succeeded) {
                    fileManager.backToRoot();
                } else {
                    fileMover.notify(data.message, 'alert-danger');
                }
            }).fail(function () {
                fileMover.notify('An error occured', 'alert-danger');
            });

            return false;
        },
        loadTree: function () {
            $('#fileMoverTree').treeview({
                dataUrl: {
                    method: 'GET',
                    dataType: 'json',
                    url: fileMover.treeDataApiUrl,
                    cache: false
                },
                nodeIcon: 'far fa-folder mr-1',
                collapseIcon: 'fas fa-minus',
                emptyIcon: 'fa',
                expandIcon: 'fas fa-plus',
                loadingIcon: 'far fa-hourglass',
                levels: 2,
                onhoverColor: '#F5F5F5',
                highlightSelected: true,
                showBorder: true,
                showCheckbox: false,
                showIcon: true,
                wrapNodeText: false,
                lazyLoad: function (node, dataFunc) {
                    $.ajax({
                        dataType: "json",
                        url: fileMover.treeDataApiUrl + '&virtualStartPath=' + node.virtualPath

                    })
                        .done(function (data) {
                            dataFunc(data);
                        });
                    node.lazyLoaded = true;
                },
                onNodeSelected: function (event, node) {
                    if (node.type === "d") {
                        fileMover.setFolderToMoveTo(node.virtualPath);
                        $('#btnMoveFile').prop('disabled', false);
                    }
                    else
                    {
                        $('#btnMoveFile').prop('disabled', true);
                    }
                },
                onNodeUnselected: function (event, node) {
                    if (node.lazyLoaded) {
                        node.lazyLoaded = false;
                    }
                    if (node.type === "d") {
                        $('#btnMoveFile').prop('disabled', true);
                    }
                    else {
                        $('#btnMoveFile').prop('disabled', true);
                    }
                }
            });
        },
        init: function () {
            $(document).bind('drop dragover', function (e) { e.preventDefault(); });
            this.progressUI.hide();
            this.loadTree();
            this.fileSelectorButton.on('click', fileMover.selectfile);
            this.fileSelectorButtonAlt.on('click', fileMover.selectfile);
            this.moveFileButton.on('click', fileMover.moveFile);

            // NB - arrow func assignment here preserves the context of 'this' = the fileMover itself
            // not the bound button that is clicked - so we can call other fileMover funcs from inside moveFilePrompt()
            this.moveFilePromptButton.on('click', () => fileMover.moveFilePrompt());

            this.setFolderToMoveTo(this.rootVirtualPath);

            if (fileMover.canSelect === "false" || fileMover.canSelect === false) {
                this.fileSelectorButton.hide();
                this.fileSelectorButtonAlt.hide();
            }
        }
    };

    fileMover.init();
})();