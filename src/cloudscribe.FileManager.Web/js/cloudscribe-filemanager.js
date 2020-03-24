(function () {
    var fileManager = {
        headers: {
            'X-CSRFToken': $("#fmconfig").data("anti-forgery-token")
           
        },
        treeDataApiUrl: $("#fmconfig").data("filetree-url"),
        //fileType: $("#fmconfig").data("file-type"),
        allowedFiles: $("#fmconfig").data("allowed-file-extensions"),
        uploadApiUrl: $("#fmconfig").data("upload-url"),
        downloadFileApiUrl: $("#fmconfig").data("file-download-url"),
        createFolderApiUrl: $("#fmconfig").data("create-folder-url"),
        deleteFolderApiUrl: $("#fmconfig").data("delete-folder-url"),
        renameFolderApiUrl: $("#fmconfig").data("rename-folder-url"),
        deleteFileApiUrl: $("#fmconfig").data("delete-file-url"),
        renameFileApiUrl: $("#fmconfig").data("rename-file-url"),
        canDelete: $("#fmconfig").data("can-delete"),
        canSelect: $("#fmconfig").data("can-select"),
        canDownload: $("#fmconfig").data("can-download"),
        emptyPreviewUrl:$("#fmconfig").data("empty-preview-url"),
        rootVirtualPath: $("#fmconfig").data("root-virtual-path"),
        rootButton: $('#btnRoot'),
        fileSelectorButton: $('#btnSelector'),
        fileSelectorButtonAlt: $('#btnSelectorAlt'),
        deleteFolderPromptButton: $('#btnDeleteFolderPrompt'),
        deleteFolderButton: $('#btnDeleteFolder'),
        renameFolderPromptButton: $('#btnRenameFolderPrompt'),
        renameFolderButton: $('#btnRenameFolder'),
        selectForCropButton: $('#btnSelectForCrop'),
        deleteFilePromptButton: $('#btnDeleteFilePrompt'),
        deleteFileButton: $('#btnDeleteFile'),
        renameFilePromptButton: $('#btnRenameFilePrompt'),
        renameFileButton: $('#btnRenameFile'),
        selectedFileInput: $("#fileSelection"),
        newFolderButton: $('#btnCreateFolder'),
        progressUI: $('#progress'),
        uploadTab: $('#tab2'),
        cropTab: $('#tab3'),
        treeData: [],
        selectedFileList: [],
        setCropImageFromServer: function () {
            var url = fileManager.selectedFileInput.val();
            //console.log(url);
            $("#image").attr("src", url);
            $("#cropCurrentDirLabel").html(url.substring(0, url.lastIndexOf("/")));
            $("#cropCurrentDir").val(url.substring(0, url.lastIndexOf("/")));
            var name = url.substring(url.lastIndexOf("/"));
            $("#croppedFileName").val(name);
            $('#origFileName').val(name);
            $('#lnkCrop').trigger('click');
            //alert('ready to crop');
        },
        clearServerCropImage: function () {
            $("#image").attr("src", fileManager.emptyPreviewUrl);
            $("#croppedFileName").val('');
            $('#origFileName').val('');
        },
        //fileManager.setPreview(node.virtualPath, node.text, node.mediaType, node.mimeType); 
        setPreview: function (node) {
            fileManager.clearPreview();
            switch (node.mediaType) {

                case "audio":
                    $("#divAudioPreview").show();
                    $("#audio-source").attr("src", node.virtualPath);
                    $("#audio-source").attr("type", node.mimeType);
                    fileManager.audioPlayer = new Plyr('#audio-player', {
                        /* options */
                    });

                    break;

                case "video":
                    $("#divVideoPreview").show();
                    $("#video-source").attr("src", node.virtualPath);
                    $("#video-source").attr("type", node.mimeType);
                    fileManager.videoPlayer = new Plyr('#video-player', {
                        /* options */
                    });

                    break;
                case "image":
                    $("#divPreview").show();
                    $("#filePreview").attr("src", node.virtualPath);

                    break;

            }
            
            fileManager.uploadTab.hide();
            fileManager.selectForCropButton.show();

            var sizeWarning = document.getElementById("divFileSizeWarning");
            sizeWarning.style.display = 'none';

            $('#divFileSize').removeClass('alert-danger');

            var sizeTest;
            if (Number.isInteger(node.size)) {
                var sizeInBytes = new Number(node.size);
                if (sizeInBytes > 1000000) {
                    var sizeMb = sizeInBytes / 1000000;
                    sizeTest = sizeMb + " MB";
                    $('#divFileSize').addClass('alert-danger');
                    if (node.mediaType == "image") {
                        sizeWarning.style.display = 'block';
                    }

                } else {
                    var sizeKb = sizeInBytes / 1000;
                    sizeTest = sizeKb + " KB";
                }
            }


            $('#divFileSize').text(sizeTest);

            //console.log(mediaType);
            //console.log(mimeType);

        },
        clearPreview: function () {
            $("#filePreview").attr("src", fileManager.emptyPreviewUrl);
            $("#fileCropPreview").attr("src", fileManager.emptyPreviewUrl);
            $("#croppedFileName").val('');
            fileManager.uploadTab.show();
            fileManager.selectForCropButton.hide();
            fileManager.clearServerCropImage();

            if (fileManager.videoPlayer) {
                fileManager.videoPlayer.destroy();
            }
            if (fileManager.audioPlayer) {
                fileManager.audioPlayer.destroy();
            }

            $("#divVideoPreview").hide();
            $("#video-source").attr("src", "");
            $("#video-source").attr("type", "");

            $("#divAudioPreview").hide();
            $("#audio-source").attr("src", "");
            $("#audio-source").attr("type", "");
            $('#divFileSize').text('');

            var sizeWarning = document.getElementById("divFileSizeWarning");
            sizeWarning.style.display = 'none';


        },
        setCurrentDirectory: function (virtualPath) {
            //console.log(virtualPath);
            $("#newFolderCurrentDir").val(virtualPath);
            $("#hdnCurrentVirtualPath").val(virtualPath);
            $("#uploadCurrentDir").val(virtualPath);
            $("#cropCurrentDir").val(virtualPath);
            $("#cropCurrentDirLabel").html(virtualPath + "/");
            $("#currentFolder").html(virtualPath);
            $("#folderToDelete").val(virtualPath);
            $("#folderToRename").val(virtualPath);
            fileManager.showFolderTools();
            

        },
        clearCurrentDirectory: function () {
            $("#newFolderCurrentDir").val(fileManager.rootVirtualPath);
            $("#hdnCurrentVirtualPath").val(fileManager.rootVirtualPath);
            $("#uploadCurrentDir").val(fileManager.rootVirtualPath);
            $("#currentFolder").html(fileManager.rootVirtualPath);
            //$("#cropCurrentDir").val('');
            $("#folderToDelete").val('');
            $("#folderToRename").val('');
            fileManager.hideFolderTools();

        },
        setCurrentFile: function (virtualPath, fileName) {
            fileManager.selectedFileInput.val(virtualPath);
            $("#newFolderCurrentDir").val(virtualPath.substring(0,virtualPath.lastIndexOf("/")));
            $("#fileToRename").val(virtualPath);
            $("#fileToDelete").val(virtualPath);
            if (fileName) {
                $("#newFileNameSegment").val(fileName);
                if (fileManager.downloadFileApiUrl && fileManager.downloadFileApiUrl.length > 0) {
                    $("#lnkDownloadFile").attr("href", fileManager.downloadFileApiUrl + "?fileToDownload=" + virtualPath);
                }
            }
            
            fileManager.showFileTools();

        },
        clearCurrentFile: function () {
            fileManager.selectedFileInput.val('');
            $("#fileToRename").val('');
            $("#fileToDelete").val('');
            $("#newFileNameSegment").val('');
            fileManager.hideFileTools();
            fileManager.clearPreview();
            

        },
        backToRoot: function () {
            fileManager.clearCurrentFile();
            fileManager.clearCurrentDirectory();
            fileManager.clearPreview();
            fileManager.setCurrentDirectory(fileManager.rootVirtualPath);
            //var tree = $('#tree').treeview(true);
            //tree.unselectNode();
            fileManager.loadTree();
            
        },
        showFolderTools: function () {
            if (fileManager.canDelete) {
                var currentFolder = $("#hdnCurrentVirtualPath").val();
                if (currentFolder != fileManager.rootVirtualPath) {
                    $('#frmDeleteFolder').show();
                    $("#frmRenameFolder").show();
                }
                
            }
            $('#frmNewFolder').show();
        },
        hideFolderTools: function () {
            $('#frmDeleteFolder').hide();
            $("#frmRenameFolder").hide();
            $('#frmNewFolder').hide();
        },
        showFileTools: function () {
            if (fileManager.canDelete) {
                $('#frmDeleteFile').show();
                $("#frmRenameFile").show();
                if (fileManager.downloadFileApiUrl) {
                    $("#lnkDownloadFile").show();
                }
                
            }
        },
        hideFileTools: function () {
            $('#frmDeleteFile').hide();
            $("#frmRenameFile").hide();
            $("#lnkDownloadFile").hide();

        },
        notify: function (message, cssClass) {
            $('#alert_placeholder').html('<div class="alert ' + cssClass + '"><a class="close" data-dismiss="alert">×</a><span>' + message + '</span></div>');
        },
        addFileToList: function (data, fileList, index, file) {
            var d = $("<span class='far fa-trash-alt' aria-role='button' title='Remove'></span>").click(function () {
                data.files.splice(index, 1);
                fileList = data.files;
                $('#fileList li').eq(index).remove();
                if (fileList.length === 0) {
                    $('#fileList').html('');
                }
            });
            var item = $("<li>", { text: file.name }).append("&nbsp;").append(d);
            $('#fileList ul').append(item);
        },
        addErrorToList: function (index, file) {
            var item = $("<li>", { text: file.ErrorMessage });
            $('#fileList ul').append(item);
        },

        createFolder: function () {
            var formData = $('#frmNewFolder').serializeArray();
            //alert(JSON.stringify(formData));
            //console.log(fileManager.headers);
            $.ajax({
                method: "POST",
                url: fileManager.createFolderApiUrl,
                headers: fileManager.headers,
                data: formData
            }).done(function (data) {
                // alert(JSON.stringify(data));
                if (data.succeeded) {
                    var currentPath = $("#newFolderCurrentDir").val();
                    if (currentPath === fileManager.rootVirtualPath) {
                        fileManager.loadTree();
                    }
                    else {
                        fileManager.reloadSubTree();
                    }
                    
                    $("#newFolderName").val('');
                    //fileManager.notify('Folder created', 'alert-success');
                }
                else {
                    fileManager.notify(data.message, 'alert-danger');

                }

            })
            .fail(function () {
                fileManager.notify('An error occured', 'alert-danger');
            });

            return false; //cancel form submit
        },
        deleteFolderPrompt: function () {
            var currentPath = $("#folderToDelete").val();
            if (currentPath === fileManager.rootVirtualPath) {
                return false;
            }
            var message = "Are you sure you want to permanently delete the folder " + currentPath + " and any files or folders below it?";
            $("#deleteFolderModalBody").html(message);
            $("#mdlDeleteFolder").modal('show');
            return false;
        },
        deleteFolder: function () {
            $("#mdlDeleteFolder").modal('hide');
            var currentPath = $("#folderToDelete").val();
            if (currentPath === fileManager.rootVirtualPath) {
                return false;
            }
            
            var formData = $('#frmDeleteFolder').serializeArray();
            //alert(JSON.stringify(formData));
            $.ajax({
                method: "POST",
                url: fileManager.deleteFolderApiUrl,
                headers: fileManager.headers,
                data: formData
            }).done(function (data) {
                if (data.succeeded) {
                    fileManager.removeNode(currentPath);
                    fileManager.clearCurrentDirectory();
                        
                }
                else {
                    fileManager.notify(data.message, 'alert-danger');

                }

            })
            .fail(function () {
                fileManager.notify('An error occured', 'alert-danger');
            });
          
            return false; //cancel form submit
        },
        renameFolderPrompt: function () {
            var currentPath = $("#folderToRename").val();
            if (currentPath === fileManager.rootVirtualPath) {
                return false;
            }
            var message = "Are you sure you want to rename the folder " + currentPath + "?";
            $("#renameFolderModalBody").html(message);
            $("#mdlRenameFolder").modal('show');

            return false;
        },
        renameFolder: function () {
            $("#mdlRenameFolder").modal('hide');
            var currentPath = $("#folderToRename").val();
            if (currentPath === fileManager.rootVirtualPath) {
                return false;
            }
           
            var formData = $('#frmRenameFolder').serializeArray();
            //alert(JSON.stringify(formData));
            $.ajax({
                method: "POST",
                url: fileManager.renameFolderApiUrl,
                headers: fileManager.headers,
                data: formData
            }).done(function (data) {
                if (data.succeeded) {
                    var tree = $('#tree').treeview(true);
                    var matchingNodes = tree.findNodes(currentPath, 'id');
                    if (matchingNodes) {
                        var parents = tree.getParents(matchingNodes);
                        if (parents && parents.length > 0) {
                            fileManager.reloadSubTree(parents[0].id);
                        }
                        else {
                            fileManager.loadTree();
                        }

                    }
                    $('#newNameSegment').val('');
                            
                    fileManager.clearCurrentDirectory();

                }
                else {
                    fileManager.notify(data.message, 'alert-danger');

                }

            })
            .fail(function () {
                fileManager.notify('An error occured', 'alert-danger');
            });
           
            return false; //cancel form submit
        },
        deleteFilePrompt: function () {
            var currentPath = $("#fileToDelete").val();
            if (currentPath === '') {
                return false;
            }
            var message = "Are you sure you want to permanently delete the file " + currentPath + "?";
            $("#deleteFileModalBody").html(message);
            $("#mdlDeleteFile").modal('show');
            return false;
        },
        deleteFile: function () {
            $("#mdlDeleteFile").modal('hide');
            var currentPath = $("#fileToDelete").val();
            if (currentPath === '') {
                return false;
            }
            if (currentPath === fileManager.rootVirtualPath) {
                return false;
            }
            
            var formData = $('#frmDeleteFile').serializeArray();
            //alert(JSON.stringify(formData));
            $.ajax({
                method: "POST",
                url: fileManager.deleteFileApiUrl,
                headers: fileManager.headers,
                data: formData
            }).done(function (data) {
                if (data.succeeded) {
                    fileManager.removeNode(currentPath);
                    fileManager.clearCurrentFile();
                }
                else {
                    fileManager.notify(data.message, 'alert-danger');
                }
            })
            .fail(function () {
                fileManager.notify('An error occured', 'alert-danger');
            });
            
            return false; //cancel form submit
        },
        renameFilePrompt: function () {
            var currentPath = $("#fileToRename").val();
            if (currentPath === '') {
                return false;
            }
            var message = "Are you sure you want to rename the file " + currentPath + "?";
            $("#renameFileModalBody").html(message);
            $("#mdlRenameFile").modal('show');

            return false;

        },
        renameFile: function () {
            $("#mdlRenameFile").modal('hide');
            var currentPath = $("#fileToRename").val();
            if (currentPath === '') {
                return false;
            }
            if (currentPath === fileManager.rootVirtualPath) {
                return false;
            }

            var formData = $('#frmRenameFile').serializeArray();
            //alert(JSON.stringify(formData));
            $.ajax({
                method: "POST",
                url: fileManager.renameFileApiUrl,
                headers: fileManager.headers,
                data: formData
            }).done(function (data) {
                if (data.succeeded) {
                    var tree = $('#tree').treeview(true);
                    var matchingNodes = tree.findNodes(currentPath, 'id');
                    if (matchingNodes) {
                        var parents = tree.getParents(matchingNodes);
                        if (parents && parents.length > 0) {
                            fileManager.reloadSubTree(parents[0].id);
                        }

                    }

                    fileManager.clearCurrentFile();

                }
                else {
                    fileManager.notify(data.message, 'alert-danger');

                }

            })
            .fail(function () {
                fileManager.notify('An error occured', 'alert-danger');
            });
       
            return false; //cancel form submit
        },
        selectfile: function () {
            var funcNum = $("#fmconfig").data("ckfunc");
            var fileUrl = fileManager.selectedFileInput.val();
            //alert(funcNum);
            if (fileUrl.length === 0) {
                fileManager.notify('Please select a file in the browse tab', 'alert-danger');
            }
            else {
                if (window.parent && window.parent.FileSelectCallback) {
                    window.parent.FileSelectCallback(fileUrl);
                }
                else {
                    window.opener.CKEDITOR.tools.callFunction(funcNum, fileUrl);
                    window.close();
                }

                
            }
        },
        removeNode: function (id) {
            var tree = $('#tree').treeview(true);
            var matchingNodes = tree.findNodes(id, 'id');
            tree.removeNode(matchingNodes, { silent: true });
        },
        reloadSubTree: function (folderIdToReload) {
            var tree = $('#tree').treeview(true);
            var currentFolderId = folderIdToReload || $("#uploadCurrentDir").val();
            //alert(currentFolderId);
            if (currentFolderId.length === 0 || currentFolderId === fileManager.rootVirtualPath) {
                fileManager.loadTree();
                return;
            }
            var matchingNodes = tree.findNodes(currentFolderId, 'id');
            if (matchingNodes.length > 0) {

                try {
                    tree.collapseNode(matchingNodes, { silent: true, ignoreChildren: false });
                } catch (err) {

                }

                
                var theNode = matchingNodes[0];
                //alert(JSON.stringify(theNode));
                //alert(theNode.id)
                var newNode = {
                    text: theNode.text,
                    id: theNode.id,
                    type: theNode.type,
                    icon: theNode.icon,
                    expandedIcon: theNode.expandedIcon,
                    virtualPath: theNode.virtualPath,
                    nodes: [],
                    lazyLoad: true //this makes it load child nodes on expand
                };
                
                try {
                    tree.updateNode(theNode, newNode, { silent: true });
                    matchingNodes = tree.findNodes(currentFolderId, 'id');
                    tree.expandNode(matchingNodes, { silent: true, ignoreChildren: false });
                }
                catch (err) {

                }
                

            }
            else {
                alert('node not found');
            }
        },
        loadTree: function () {
            $('#tree').treeview({
                dataUrl: {
                    method: 'GET',
                    dataType: 'json',
                    url: fileManager.treeDataApiUrl,
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
                    //alert(node.text + ' lazyload');
                    $.ajax({
                        dataType: "json",
                        url: fileManager.treeDataApiUrl + '&virtualStartPath=' + node.virtualPath

                    })
                      .done(function (data) {
                          dataFunc(data);
                          
                      })
                    ;
                    node.lazyLoaded = true;

                },
                onNodeSelected: function (event, node) {
                    //alert(node.virtualPath + ' selected');
                    //console.log(node);
                    $("#divPreview").hide();
                    if (node.canPreview) {
                        //fileManager.setPreview(node.virtualPath, node.text, node.mediaType, node.mimeType);   
                        fileManager.setPreview(node); 
                    }
                    else {
                        fileManager.clearPreview();    
                    }
                    if (node.type === "d") {
                        fileManager.setCurrentDirectory(node.virtualPath);
                        fileManager.clearCurrentFile();
                        fileManager.hideFileTools();
                    }
                    else {
                        fileManager.hideFolderTools();
                        fileManager.setCurrentFile(node.virtualPath, node.text);
                    }
                },
                onNodeUnselected: function (event, node) {
                    //alert(node.virtualPath + ' unselected');
                    //alert(node.state.selected);
                    // a hack because for some reason lazy load expand leads to unselected event
                    //alert(JSON.stringify(node));
                    if (node.lazyLoaded) {
                        node.lazyLoaded = false;
                    }
                    else {
                        //fileManager.clearCurrentDirectory();
                    }
                    if (node.type === "d") {
                        fileManager.hideFolderTools();
                    }
                    else
                    {
                        fileManager.hideFileTools();
                    }
                    
                }
            });

        },
        setupFileLoader: function () {
            $('#pnlFiles').fileupload({
                fileInput: $('#fileupload'),
                url: fileManager.uploadApiUrl,
                headers: fileManager.headers,
                dataType: 'json',
                autoUpload: true,
                singleFileUploads: true,
                //limitMultiFileUploads: 2,
                //limitConcurrentUploads: 3,
                dropZone: $('#dropZone'),
                pasteZone: $('#dropZone'),
                add: function (e, data) {
                    //console.log('add called with data ');
                    //console.log(data);
                    //$('#fileList').html('');
                    $('#fileList').empty();
                    $('#fileList').append($("<ul class='filelist'></ul>"));
                   
                    var allowedFiles = fileManager.allowedFiles.split('|');
                    var regx = new RegExp("([a-zA-Z0-9\u0600-\u06FF\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
                    var j = 0;
                    var k = data.files.length;
                    //alert(k);
                    while (j < k) {
                        if ((regx.test(data.files[j].name.toLowerCase())) === false) {
                            fileManager.notify(data.files[j].name + ' not allowed', 'alert-danger');
                            //console.log(data.files[j].name + " rejected by regex");
                            //alert('false');
                            data.files.splice(j, 1);
                            k = data.files.length;
                            j = -1;
                        }
                        j++;
                    }
                    fileManager.selectedFileList = fileManager.selectedFileList.concat(data.files);
                    //var maxAllowed = 10;
                    //while (fileManager.selectedFileList.length > maxAllowed) {
                    //    fileManager.selectedFileList.pop();
                    //}
                    //fileManager.notify('An error occured', 'alert-danger');
                    //data.files = fileManager.selectedFileList;
                    //if (data.files.length > 0) {
                    //    var btnSend = $("<button id='btnSend' class='btn btn-success'><i class='fas fa-cloud-upload-alt' aria-hidden='true'></i> Upload</button>");
                    //    btnSend.appendTo($('#fileList'));
                    //}
                    //$.each(data.files, function (index, file) { fileManager.addFileToList(data, fileManager.selectedFileList, index, file); });
                    //$('#btnSend').click(function () {
                    //    data.context = $('<p/>').text('Uploading...').replaceAll($(this));
                    //    data.submit();


                    //});
                    //data.submit();
                    if (data.files.length > 0) {
                        data.process().done(function () {
                            data.submit();
                        });
                    }

                },
                done: function (e, data) {
                    //console.log('done');
                    data.files = [];
                    fileManager.selectedFileList = [];
                    //console.log(data);
                    $('#fileupload').val(null);
                    $('#progress').hide();
                    //$('#fileList').html('');
                    $('#fileList').empty();
                    fileListuploader = [];
                    $('#fileList').append($("<ul class='filelist file-errors'></ul>"));
                    var j = 0;
                    var errorsOccurred = false;
                    while (j < data.length) {
                        if (data[j].errorMessage) {
                            errorsOccurred = true;
                            addErrorToList(j, data[j]);
                        }
                        j++;
                    }

                    //try {
                    //    fileManager.reloadSubTree();
                    //} catch (err) {}
                    
                },
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    fileManager.progressUI.show();
                    $('#progress .progress-bar').css(
                        'width',
                        progress + '%'
                    );
                    //console.log(progress);
                    if (progress === 100) {
                        fileManager.notify('File upload success.', 'alert-success');
                        setTimeout(function () {
                            fileManager.reloadSubTree();
                            
                        }, 3000);
                       
                    }
                },
                fail: function (e, data) {
                    //console.log(data);
                    $('#progress .progress-bar').css('width','0%');

                    fileManager.progressUI.hide();
                    fileManager.notify('Something went wrong, possibly the file is larger than allowed by server configuration.', 'alert-danger');
                }

            }).prop('disabled', !$.support.fileInput)
              .parent().addClass($.support.fileInput ? undefined : 'disabled');

            $('#fileupload').bind('fileuploadsubmit', function (e, data) {
                data.formData = $('#frmUpload').serializeArray();
                //alert(data.formData);
                return true;
            });

        },
        
        init: function () {
            $(document).bind('drop dragover', function (e) { e.preventDefault(); });
            this.progressUI.hide();
            //this.cropTab.hide();
            this.loadTree();
            this.setupFileLoader();
            this.newFolderButton.on('click', fileManager.createFolder);
            this.fileSelectorButton.on('click', fileManager.selectfile);
            this.fileSelectorButtonAlt.on('click', fileManager.selectfile);
            this.deleteFolderPromptButton.on('click', fileManager.deleteFolderPrompt);
            this.deleteFolderButton.on('click', fileManager.deleteFolder);
            this.renameFolderPromptButton.on('click', fileManager.renameFolderPrompt);
            this.renameFolderButton.on('click', fileManager.renameFolder);
            this.deleteFilePromptButton.on('click', fileManager.deleteFilePrompt);
            this.deleteFileButton.on('click', fileManager.deleteFile);
            this.renameFilePromptButton.on('click', fileManager.renameFilePrompt);
            this.renameFileButton.on('click', fileManager.renameFile);
            this.selectForCropButton.on('click', fileManager.setCropImageFromServer);
            this.setCurrentDirectory(this.rootVirtualPath);
            this.rootButton.on('click', fileManager.backToRoot);
            if (fileManager.canSelect === "false" || fileManager.canSelect === false) {
                this.fileSelectorButton.hide();
                this.fileSelectorButtonAlt.hide();
            }

            //alert('init');
        }


    };

    fileManager.init();
    // TODO: this needs refactoring
    var cropManager = {
        uploadUrl: $("#fmconfig").data("upload-url"),
        URL: window.URL || window.webkitURL,
        console: window.console || { log: function () { } },
        image: $('#image'),
        saveLocalButton: $('#btnSaveLocal'),
        uploadCropButton: $('#btnUploadCrop'),
        croppedFileName: $('#croppedFileName'),
        chkConstrainCrop: $('#chkContrainWidthOfCrop'),
        cropMaxWidthInput: $('#cropMaxWidth'),
        dataX: $('#dataX'),
        dataY: $('#dataY'),
        dataHeight: $('#dataHeight'),
        dataWidth: $('#dataWidth'),
        dataRotate: $('#dataRotate'),
        dataScaleX: $('#dataScaleX'),
        dataScaleY: $('#dataScaleY'),
        outputHeight: $('#dataNewHeight'),
        outputWidth : $('#dataNewWidth'),

        setup: function () {
            var options = {
                aspectRatio: 16 / 9,
                preview: '.img-preview',
                crop: function (e) {
                    cropManager.dataX.val(Math.round(e.x));
                    cropManager.dataY.val(Math.round(e.y));
                    var height = Math.round(e.height);
                    var width = Math.round(e.width);
                    cropManager.dataHeight.val(height);
                    cropManager.dataWidth.val(width);
                    cropManager.dataRotate.val(e.rotate);
                    cropManager.dataScaleX.val(e.scaleX);
                    cropManager.dataScaleY.val(e.scaleY);
                    var maxWidth = Math.round(cropManager.cropMaxWidthInput.val());
                    if (cropManager.chkConstrainCrop.is(':checked') && width > maxWidth) {
                        cropManager.outputWidth.val(maxWidth);
                        var aspect = cropManager.getCropAspectRatio();
                        var newHeight = parseInt(maxWidth / aspect);
                        cropManager.outputHeight.val(newHeight);
                        cropManager.setCroppedFileName(maxWidth, newHeight);
                    }
                    else
                    {
                        cropManager.outputHeight.val(height);
                        cropManager.outputWidth.val(width);
                        cropManager.setCroppedFileName(Math.round(e.width), Math.round(e.height));
                    }

                    if ($.isFunction(document.createElement('canvas').getContext)) {
                        
                        var currentSrc = cropManager.image.attr("src");
                        if (currentSrc === fileManager.emptyPreviewUrl) {
                            cropManager.uploadCropButton.prop('disabled', true);
                        }
                        else
                        {
                            cropManager.uploadCropButton.prop('disabled', false);
                        }
                    }
                    

                }
            };
            var originalImageURL = cropManager.image.attr('src');
            var uploadedImageURL;


            // Tooltip
            $('[data-toggle="tooltip"]').tooltip();


            // Cropper
            cropManager.image.on({
                ready: function (e) {
                    //console.log(e.type);
                },
                cropstart: function (e) {
                    //console.log(e.type, e.action);
                },
                cropmove: function (e) {
                   // console.log(e.type, e.action);
                },
                cropend: function (e) {
                    //console.log(e.type, e.action);
                },
                crop: function (e) {
                    //console.log(e.type, e.x, e.y, e.width, e.height, e.rotate, e.scaleX, e.scaleY);
                },
                zoom: function (e) {
                   // console.log(e.type, e.ratio);
                }
            }).cropper(options);


            // Buttons
            if (!$.isFunction(document.createElement('canvas').getContext)) {
                cropManager.uploadCropButton.prop('disabled', true);
                //$('button[data-method="getCroppedCanvas"]').prop('disabled', true);
            }

            if (typeof document.createElement('cropper').style.transition === 'undefined') {
                $('button[data-method="rotate"]').prop('disabled', true);
                $('button[data-method="scale"]').prop('disabled', true);
            }


            // Download
            if (typeof cropManager.saveLocalButton[0].download === 'undefined') {
                cropManager.saveLocalButton.addClass('disabled');
            }


            // Options
            $('.docs-toggles').on('change', 'input', function () {
                var $this = $(this);
                var name = $this.attr('name');
                var type = $this.prop('type');
                var cropBoxData;
                var canvasData;

                if (!cropManager.image.data('cropper')) {
                    return;
                }

                if (type === 'checkbox') {
                    options[name] = $this.prop('checked');
                    cropBoxData = cropManager.image.cropper('getCropBoxData');
                    canvasData = cropManager.image.cropper('getCanvasData');

                    options.ready = function () {
                        cropManager.image.cropper('setCropBoxData', cropBoxData);
                        cropManager.image.cropper('setCanvasData', canvasData);
                    };
                } else if (type === 'radio') {
                    options[name] = $this.val();
                }

                cropManager.image.cropper('destroy').cropper(options);
            });


            // Methods
            //uploadCroppedImage
            $("#btnUploadCropped").on('click', cropManager.uploadCroppedImage);

            cropManager.outputWidth.on('blur', function () {
                var aspect = cropManager.getCropAspectRatio();
                //alert(aspect);
                var newWidth = parseInt(cropManager.outputWidth.val())
                var currentHeight = parseInt(cropManager.outputHeight.val());
                var newHeight = parseInt(newWidth / aspect);
                //alert(newHeight);
                var dif = Math.abs(newHeight - currentHeight);
                if (dif > 1) {
                    cropManager.outputHeight.val(newHeight);
                    cropManager.setCroppedFileName(newWidth, newHeight)
                }
                
            });

            cropManager.outputHeight.on('blur', function () {
                var aspect = cropManager.getCropAspectRatio();
                var newHeight = parseInt(cropManager.outputHeight.val());
                var currentWidth = parseInt(cropManager.outputWidth.val());
                var newWidth = parseInt(newHeight * aspect);
                var dif = Math.abs(newWidth - currentWidth);
                if (dif > 1) {
                    cropManager.outputWidth.val(newWidth);
                    cropManager.setCroppedFileName(newWidth, newHeight)
                }
                
            });

            cropManager.chkConstrainCrop.change(function () {
                if ($(this).is(":checked")) {
                    var maxWidth = Math.round(cropManager.cropMaxWidthInput.val());
                    var currentWidth = parseInt(cropManager.dataWidth.val());
                    if (currentWidth > maxWidth) {
                        cropManager.outputWidth.val(maxWidth);
                        var currentHeight = parseInt(cropManager.dataHeight.val());
                        var aspect = cropManager.getCropAspectRatio();
                        var newHeight = parseInt(maxWidth / aspect);
                        cropManager.outputHeight.val(newHeight);
                    }

                }
                else
                {
                    cropManager.outputHeight.val(cropManager.dataHeight.val());
                    cropManager.outputWidth.val(cropManager.dataWidth.val());
                }
            });

            $('.docs-buttons').on('click', '[data-method]', function () {
                var $this = $(this);
                var data = $this.data();
                var $target;
                var result;

                if ($this.prop('disabled') || $this.hasClass('disabled')) {
                    return;
                }

                if (cropManager.image.data('cropper') && data.method) {
                    data = $.extend({}, data); // Clone a new one

                    if (typeof data.target !== 'undefined') {
                        $target = $(data.target);

                        if (typeof data.option === 'undefined') {
                            try {
                                data.option = JSON.parse($target.val());
                            } catch (e) {
                                console.log(e.message);
                            }
                        }
                    }

                    if (data.method === 'rotate') {
                        cropManager.image.cropper('clear');
                    }
                    if ((typeof data.option === 'undefined') && (data.method === 'getCroppedCanvas')) {
                        //alert('need size');
                        data.option = { width: cropManager.outputWidth.val(), height: cropManager.outputHeight.val()};
                    }
                    

                    result = cropManager.image.cropper(data.method, data.option, data.secondOption);

                    if (data.method === 'rotate') {
                        cropManager.image.cropper('crop');
                    }

                    switch (data.method) {
                        case 'scaleX':
                        case 'scaleY':
                            $(this).data('option', -data.option);
                            break;

                        case 'getCroppedCanvas':
                            if (result) {

                                // Bootstrap's Modal
                                $('#getCroppedCanvasModal').modal().find('.modal-body').html(result);

                                if (!cropManager.saveLocalButton.hasClass('disabled')) {
                                    cropManager.saveLocalButton.attr('href', result.toDataURL('image/jpeg'));
                                }
                            }

                            break;

                        case 'destroy':
                            if (uploadedImageURL) {
                                URL.revokeObjectURL(uploadedImageURL);
                                uploadedImageURL = '';
                                cropManager.image.attr('src', originalImageURL);
                            }

                            break;
                    }

                    if ($.isPlainObject(result) && $target) {
                        try {
                            $target.val(JSON.stringify(result));
                        } catch (e) {
                            console.log(e.message);
                        }
                    }

                }

            });


            // Keyboard
            $(document.body).on('keydown', function (e) {

                if (!cropManager.image.data('cropper') || this.scrollTop > 300) {
                    return;
                }

                switch (e.which) {
                    case 37:
                        e.preventDefault();
                        cropManager.image.cropper('move', -1, 0);
                        break;

                    case 38:
                        e.preventDefault();
                        cropManager.image.cropper('move', 0, -1);
                        break;

                    case 39:
                        e.preventDefault();
                        cropManager.image.cropper('move', 1, 0);
                        break;

                    case 40:
                        e.preventDefault();
                        cropManager.image.cropper('move', 0, 1);
                        break;
                }

            });


            // Import image
            var $inputImage = $('#inputImage');

            if (cropManager.URL) {
                $inputImage.change(function () {
                    var files = this.files;
                    var file;

                    if (!cropManager.image.data('cropper')) {
                        return;
                    }

                    if (files && files.length) {
                        file = files[0];

                        if (/^image\/\w+$/.test(file.type)) {
                            if (uploadedImageURL) {
                                URL.revokeObjectURL(uploadedImageURL);
                            }

                            uploadedImageURL = cropManager.URL.createObjectURL(file);
                            $('#origFileName').val(file.name.toLowerCase());
                            cropManager.croppedFileName.val(file.name.toLowerCase());
                            //alert(file.name);
                            cropManager.image.cropper('destroy').attr('src', uploadedImageURL).cropper(options);
                            $inputImage.val('');
                        } else {
                            window.alert('Please choose an image file.');
                        }
                    }
                });
            } else {
                $inputImage.prop('disabled', true).parent().addClass('disabled');
            }
        },
        tearDown: function () {
            if (($('#image').data('cropper'))) {
                $('#image').cropper('destroy');
            }
        },
        setCroppedFileName: function (width, height) {
            var origName = $('#origFileName').val();
            var nameWithoutExtension = origName.substring(0, origName.lastIndexOf('.'));
            var ext = origName.substring(origName.lastIndexOf('.'));
            cropManager.croppedFileName.val(nameWithoutExtension + "-" + width + "x" + height + ext);
            cropManager.saveLocalButton.attr("download", cropManager.croppedFileName.val());
        },
        getCropAspectRatio: function ()
        {
            var cropbox = $('#image').cropper('getCropBoxData');
            return cropbox.width / cropbox.height;

        },
        uploadCroppedImage: function () {

            var opts = { width: cropManager.outputWidth.val(), height: cropManager.outputHeight.val() };
            $('#image').cropper('getCroppedCanvas', opts).toBlob(function (blob) {

                var formData = new FormData();
                formData.append(cropManager.croppedFileName.val(), blob);

                var otherData = $('#frmUploadCropped').serializeArray();
                $.each(otherData, function (key, input) {
                    formData.append(input.name, input.value);
                })
                
                $.ajax({
                    method: "POST",
                    url: fileManager.uploadApiUrl,
                    headers: fileManager.headers,
                    data: formData,
                    processData: false,
                    contentType:false
                }).done(function (data, textStatus, jqXHR) {
                    // alert(JSON.stringify(data));
                    if (data[0].errorMessage) {
                        fileManager.notify(data[0].errorMessage, 'alert-danger');
                    }
                    else {
                        
                        var currentPath = $("#cropCurrentDir").val();
                        if (currentPath === fileManager.rootVirtualPath) {
                            fileManager.loadTree();
                        }
                        else {
                            fileManager.reloadSubTree(currentPath);
                        }

                        $('#getCroppedCanvasModal').modal('hide');
                        fileManager.notify('Cropped image upload succeeded', 'alert-success');

                    }

                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    fileManager.notify(errorThrown, 'alert-danger');
                });

            });

            

            return false; //cancel form submit
        }
    };

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("href"); // activated tab

        if (target === "#tabCrop") {
            cropManager.setup();
        }
        else {
            var related = $(e.relatedTarget).attr("href"); //previous tab
            if (related) {
                var hash = related.replace(/^.*?(#|$)/, '');
                //alert(hash);
                if (hash === "tabCrop") {
                    cropManager.tearDown();
                }
            }
            
        }
    });

})();
