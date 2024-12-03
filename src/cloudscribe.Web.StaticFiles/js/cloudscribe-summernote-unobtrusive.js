$(async function () {
    let $elems = $('textarea[data-summernote-unobtrusive]').toArray();
    
	if ($elems) {
		for (elem of $elems) {
			let summernoteInstance = $(elem).data('summernote-unobtrusive');
			let configPath = $(elem).data('summernote-config-url');
			let configToolbarPath = $(elem).data('summernote-toolbar-config-url');
			let dropFileUploadUrl = $(elem).data('summernote-config-dropfileuploadurl');
			let dropFileXsrfToken = $('[name="__RequestVerificationToken"]:first').val();
			let configLanguage = $(elem).data('summernote-config-language');
			
			if (summernoteInstance) {
				summernoteInstance = 'textarea[data-summernote-unobtrusive=' + summernoteInstance + ']';
			} else {
				summernoteInstance = 'textarea[data-summernote-unobtrusive=""]';
			}

			if (configPath) {
				let configSummernote;
				let jsonStyles;
				let summerStyle = [["style"],["font"],["color"],["para"],["table"],["insert"],["view"],["custom"],["serverimagebutton"]];
						
				await getConfigSettings(configPath);
				await getToolbarSettings(configToolbarPath);
				await setupToolbar();
				await setupSummernote();
				
				async function getToolbarSettings(file) {
					await fetch(file)
					.then((response) => response.json())
					.then((json) => {
						jsonStyles = json;
					});
				}
				
				async function getConfigSettings(file) {
					await fetch(file)
					.then((response) => response.json())
					.then((json) => {
						configSummernote = json;

						if (configLanguage) {
							configSummernote.lang = configLanguage;
						} else {
							configSummernote.lang = "en-US";
						}
					});
				}
				
				async function setupToolbar() {
					summerStyle[0][1] = jsonStyles["style"];
					summerStyle[1][1] = jsonStyles["font"];
					summerStyle[2][1] = jsonStyles["color"];
					summerStyle[3][1] = jsonStyles["para"];
					summerStyle[4][1] = jsonStyles["table"];
					summerStyle[5][1] = jsonStyles["insert"];
					summerStyle[6][1] = jsonStyles["view"];
					summerStyle[7][1] = jsonStyles["custom"];
					summerStyle[8][1] = jsonStyles["serverimagebutton"];
				}

				function setupSummernote() {
					$(summernoteInstance).summernote({
						toolbar: summerStyle,
						...configSummernote
					});
				}
			}
        }
    }
});

window.handleMessageFromChild = function(message) {
    var modal = document.querySelectorAll('#serverimage');

    $("#" + message.instance + "").summernote('insertImage', message.url, message.filename);
	
	if (modal) {
		$(modal).modal('hide');
		$(document.body).removeAttr('style');
		$(document.body).removeAttr('padding-right');
		$('.modal-backdrop').toggle();
	}
};