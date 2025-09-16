$(async function () {
	let $elems = $('textarea[data-summernote-unobtrusive]').toArray();
	let dropFileUploadUrl = [];
	let summernoteNumber = 0;
    let summerNumber = 0;
	let summerInst;
	let dropFileXsrfToken;
    
	if ($elems) {
		for (elem of $elems) {
			let summernoteInstance = $(elem).data('summernote-unobtrusive');
			let configPath = $(elem).data('summernote-config-url');
			let configToolbarPath = $(elem).data('summernote-toolbar-config-url');
			let configLanguage = $(elem).data('summernote-config-language');
			dropFileUploadUrl.push({ number: summerNumber, url: $(elem).data('summernote-config-dropfileuploadurl') });
			dropFileXsrfToken = $('[name="__RequestVerificationToken"]:first').val();

			if (summernoteInstance) {
				summernoteInstance = 'textarea[data-summernote-unobtrusive="' + summernoteInstance + '"]';
			} else {
				summernoteInstance = 'textarea[data-summernote-unobtrusive=""]';
			}

			if (configPath) {
				let summernoteConfig;				
				let toolbarConfig;
						
				await getConfigSettings(configPath);
				await getToolbarSettings(configToolbarPath);			
				await setupSummernote();
				
				async function getToolbarSettings(file) {
					await fetch(file, { headers: { 'Cache-Control': 'no-cache, no-store' } })
					.then((response) => response.json())
					.then((json) => {
						toolbarConfig = json;
					});
				}
				
				async function getConfigSettings(file) {
					await fetch(file, { headers: { 'Cache-Control': 'no-cache, no-store' } })
					.then((response) => response.json())
					.then((json) => {
						summernoteConfig = json;

						if (configLanguage) {
							summernoteConfig.lang = configLanguage;
						} else {
							summernoteConfig.lang = "en-US";
						}
					});
				}							

				function setupSummernote() {
					$(summernoteInstance).each(function (i) {
						$(summernoteInstance).eq(i).summernote({
							callbacks: {
								onImageUpload: function (files) {
									summernoteNumber = i;
									summerInst = summernoteInstance;
									onDropped(files);
								},
								onDialogShown: function(dialog) {
									// Only uncheck "Open in new window" for NEW links, not when editing existing ones
									requestAnimationFrame(function() {
										var $modal = $('.modal:visible');
										var $checkbox = $modal.find('.sn-checkbox-open-in-new-window input[type=checkbox]');
										var $urlField = $modal.find('.note-link-url');
										
										// Check if we're editing an existing link
										// If URL field has a value other than "http://", we're likely editing
										var isEditing = $urlField.length > 0 && $urlField.val() && $urlField.val() !== 'http://';
										
										if ($checkbox.length > 0 && !isEditing) {
											// Only uncheck for new links
											$checkbox.prop('checked', false);
										} else if ($checkbox.length === 0) {
											// If not found immediately, try again with minimal delay
											setTimeout(function() {
												var $modal = $('.modal:visible');
												var $checkbox = $modal.find('.sn-checkbox-open-in-new-window input[type=checkbox]');
												var $urlField = $modal.find('.note-link-url');
												var isEditing = $urlField.length > 0 && $urlField.val() && $urlField.val() !== 'http://';
												
												if ($checkbox.length > 0 && !isEditing) {
													$checkbox.prop('checked', false);
												}
											}, 10);
										}
										// If editing existing link, leave checkbox as-is (Summernote will set it correctly)
									});
								}
							},
							toolbar: toolbarConfig,
							...summernoteConfig
						});

						$(summernoteInstance).on('summernote.codeview.change', function (we, contents, $editable) {

							if (!$(summernoteInstance).summernote('codeview.isActivated')) {
								// This is not code view, so ignore
								return;
							}

                            // Update the original textarea and editable content
							var $textarea = $(this);
							var $editor = $textarea.nextAll('.note-editor.note-frame.card.codeview');
							var $editablee = $editor.find('.note-editable.card-block');
							var $originalTextarea = $editor.prev(summernoteInstance);

							$editablee.html(contents);
							$originalTextarea.val(contents);
						});
					});
				}
			}
			summerNumber++;
		}

		function onDropped(contents) {
			content = contents;

			if (contents && contents.length > 0) {
				for (var i = 0; i < contents.length; i++) {
					var name = contents[i].name; //usually this will always be image.png
					var baseName = name.substring(0, name.lastIndexOf('.')) || name;
					var ext = name.substring(name.lastIndexOf('.') + 1).toLowerCase();
					var date = new Date().toISOString().replace(/:|\./g, '-').substring(0, 21);
					var newName = baseName + '_' + date + '.' + ext;
					var file = renameFile(contents[i], newName);

					uploadFile(file);
				}
			}
		};

		function renameFile(originalFile, newName) {
			return new File([originalFile], newName, {
				type: originalFile.type,
				lastModified: originalFile.lastModified,
			});
		}

		function uploadFile(file) {
			switch (file.type) {
				case "image/jpeg":
				case "image/jpg":
				case "image/gif":
				case "image/png":
				case "image/svg+xml":

					var formData = new FormData();
					formData.append("__RequestVerificationToken", dropFileXsrfToken);
					formData.append(file.name, file);
					let summInstance = dropFileUploadUrl.find(x => x.number === summernoteNumber);

					$.ajax({
						type: "POST",
						processData: false,
						contentType: false,
						dataType: "json",
						url: summInstance.url,
						data: formData,
						success: uploadSuccess
					});

					break;
			}
		}

		function uploadSuccess(data, textStatus, jqXHR) {
			if (data[0].errorMessage) { alert(data[0].errorMessage); return; }

			if (data[0].resizedUrl) {
				if (summerInst) {
					$(summerInst).eq(summernoteNumber).summernote('insertImage', data[0].resizedUrl, data[0].name);
				} else {
					$(summernoteInstance).eq(summernoteNumber).summernote('insertImage', data[0].resizedUrl, data[0].name);
				}
			} else {
				if (summerInst) {
					$(summerInst).eq(summernoteNumber).summernote('insertImage', data[0].originalUrl, data[0].name);
				} else {
					$(summernoteInstance).eq(summernoteNumber).summernote('insertImage', data[0].originalUrl, data[0].name);
				}
			}
		}
    }
});

window.handleMessageFromChild = function(message) {
    var modal = document.querySelectorAll('.note-modal');

    $("#" + message.instance + "").summernote('insertImage', message.url, message.filename);
	
	if (modal) {
		$(modal).modal('hide');
		$(document.body).removeAttr('style');
		$(document.body).removeAttr('padding-right');
		$('.modal-backdrop').toggle();
	}
};


