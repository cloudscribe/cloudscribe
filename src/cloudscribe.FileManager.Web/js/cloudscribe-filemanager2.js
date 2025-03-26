(function () {
    var fileManager2 = {
        headers: {
            'X-CSRFToken': $("#fmconfig2").data("anti-forgery-token")
        },
        treeDataApiUrl2: $("#fmconfig2").data("filetree-url"),
        canSelect2: $("#fmconfig2").data("can-select"),
        rootVirtualPath2: $("#fmconfig2").data("root-virtual-path"),
        fileSelectorButton2: $('#btnSelector'),
        fileSelectorButtonAlt2: $('#btnSelectorAlt'),
        moveFileButton: $('#btnMoveFile'),
        moveFilePromptButton: $('#btnMoveFilePrompt'),
        moveFileApiUrl: $("#fmconfig2").data("movefile-url"),
        selectedFileInput2: $("#fileSelection"),
        progressUI2: $('#progress'),
        refresh: $('#btnRefresh'),
        setCurrentDirectory2: function (virtualPath) {
            $("#folderToMoveTo").val(virtualPath);
        },
        clearCurrentDirectory2: function () {
            $("#folderToMoveTo").html('');
        },
        notify2: function (message, cssClass) {
            $('#alert_placeholder').html('<div class="alert ' + cssClass + '"><button type="button" data-bs-dismiss="alert" class="btn-close me-2" style="float:right" aria-label="Close"></button><span>' + message + '</span></div>');
        },
        moveFilePrompt: function () {
            var currentPath = $("#fileToDelete").val();

            if (currentPath === '') {
                return false;
            }

            $("#mdlMoveFile").modal('show');

            return false;
        },
        moveFile: function () {
            $("#mdlMoveFile").modal('hide');
            var currentPath = $("#fileToMove").val();

            if (currentPath === '') {
                return false;
            }
            if (currentPath === fileManager2.rootVirtualPath2) {
                return false;
            }

            var formData = $('#frmMoveFile').serializeArray();

            $.ajax({
                method: "POST",
                url: fileManager2.moveFileApiUrl,
                headers: fileManager2.headers,
                data: formData
            }).done(function (data) {
                if (data.succeeded) {
                    fileManager.backToRoot();
                } else {
                    fileManager2.notify2(data.message, 'alert-danger');
                }
            }).fail(function () {
                fileManager2.notify2('An error occured', 'alert-danger');
            });

            return false;
        },
        loadTree2: function () {
            $('#tree2').treeview({
                dataUrl: {
                    method: 'GET',
                    dataType: 'json',
                    url: fileManager2.treeDataApiUrl2,
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
                        url: fileManager2.treeDataApiUrl2 + '&virtualStartPath=' + node.virtualPath

                    })
                        .done(function (data) {
                            dataFunc(data);
                        });
                    node.lazyLoaded = true;
                },
                onNodeSelected: function (event, node) {
                    if (node.type === "d") {
                        fileManager2.setCurrentDirectory2(node.virtualPath);
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
            this.progressUI2.hide();
            this.loadTree2();
            this.fileSelectorButton2.on('click', fileManager2.selectfile);
            this.fileSelectorButtonAlt2.on('click', fileManager2.selectfile);
            this.moveFileButton.on('click', fileManager2.moveFile);
            this.moveFilePromptButton.on('click', fileManager2.moveFilePrompt);
            this.setCurrentDirectory2(this.rootVirtualPath2);

            if (fileManager2.canSelect2 === "false" || fileManager2.canSelect2 === false) {
                this.fileSelectorButton2.hide();
                this.fileSelectorButtonAlt2.hide();
            }
        }
    };

    fileManager2.init();
})();