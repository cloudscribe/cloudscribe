$(async function () {
	var $elems = $('textarea[data-tinymce]').toArray();

	if ($elems) {
		for(elem of $elems) {
			var config = {};
			var tinyInstance = $(elem).data('tinymce');
			var configPath = $(elem).data('tinymce-config-url');
			var dropFileUploadUrl = $(elem).data('tinymce-config-dropfileuploadurl');
			var dropFileXsrfToken = $('[name="__RequestVerificationToken"]:first').val();
			var configLanguage = $(elem).data('tinymce-config-language');
			
			if (tinyInstance) {
				tinyInstance = 'textarea[data-tinymce=' + tinyInstance + ']';
			} else {
				tinyInstance = 'textarea[data-tinymce=""]';
			}
			if (configPath) { config.customConfig = configPath; }

			if (configPath) {
				var configTiny;
				
				await fetch(configPath)
				.then((res) => res.json())
				.then((text) => {
					configTiny = text;

					if (configLanguage) {
						configTiny.language = configLanguage;
					}
					
					tinymce.init({
						selector: tinyInstance,
						setup: (editor) => {
							editor.on('drop', (e) => {
								e.preventDefault();
								e.stopPropagation();
								return true;
							});
						},
						init_instance_callback: (editor) => {
							editor.options.register('dropFileUploadUrl', {
								processor: 'string'
							});
							editor.options.register('dropFileXsrfToken', {
								processor: 'string'
							});
							editor.options.set('dropFileUploadUrl', dropFileUploadUrl);
							editor.options.set('dropFileXsrfToken', dropFileXsrfToken);
						},
						...configTiny
					});
				})
				.catch((e) => console.error(e));
			}
		}
	}
});