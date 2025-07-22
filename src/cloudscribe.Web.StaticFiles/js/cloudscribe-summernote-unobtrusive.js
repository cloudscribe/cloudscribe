$(async function () {
	let $elems = $('textarea[data-summernote-unobtrusive]').toArray();
	let dropFileUploadUrl;
	let summernoteNumber = 0;
	let summerInst;
	let dropFileXsrfToken;
    
	if ($elems) {
		for (elem of $elems) {
			let summernoteInstance = $(elem).data('summernote-unobtrusive');
			let configPath = $(elem).data('summernote-config-url');
			let configToolbarPath = $(elem).data('summernote-toolbar-config-url');
			let configLanguage = $(elem).data('summernote-config-language');
			dropFileUploadUrl = $(elem).data('summernote-config-dropfileuploadurl');
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
					await fetch(file)
					.then((response) => response.json())
					.then((json) => {
						toolbarConfig = json;
					});
				}
				
				async function getConfigSettings(file) {
					await fetch(file)
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



				function setupSummernoteCallback() {

					$(summernoteInstance).each(function (i) {
						$(summernoteInstance).eq(i).summernote({
							callbacks: {
								onImageUpload: function (files) {
									summernoteNumber = i;
									summerInst = summernoteInstance;
									onDropped(files);
								},
								onChange: function (contents, $editable) {
									if (typeof DOMPurify !== 'undefined') {
										const clean = DOMPurify.sanitize(contents, {
											// we'll need a lot more than these... - jk
											ALLOWED_TAGS: ['style', 'b', 'i', 'u', 'span', 'p', 'div', 'h1', 'h2', 'h3', 'ul', 'li', 'ol', 'a', 'img', 'blockquote', 'table', 'tr', 'td'],
											ALLOWED_ATTR: ['style', 'href', 'src', 'alt', 'class']
										});

										if (clean !== contents) {   // recursive madness if we don't include this... Only update if different - jk
											$(this).summernote('code', clean); 
										}
									} else {
										console.warn('DOMPurify not available — skipping sanitation.');
									}
								}
							},
							toolbar: toolbarConfig,
							...summernoteConfig
						});
					});
				}

				function loadDOMPurify(callback) {
					const script = document.createElement('script');
					script.src = '/cr/js/purify.min.js';
					script.onload = () => {
						console.log('DOMPurify loaded');
						if (callback) callback();
					};
					script.onerror = () => {
						console.error('Failed to load DOMPurify');
					};
					document.head.appendChild(script);
				}

				function setupSummernote() {

					loadDOMPurify(function () {
						// Now it's safe to use DOMPurify in setupSummernote()
						setupSummernoteCallback();
					});
				}

			}
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

					$.ajax({
						type: "POST",
						processData: false,
						contentType: false,
						dataType: "json",
						url: dropFileUploadUrl,
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


