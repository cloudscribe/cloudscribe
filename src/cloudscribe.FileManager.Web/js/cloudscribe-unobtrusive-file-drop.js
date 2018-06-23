(function () {
	document.addEventListener("DOMContentLoaded", function () {
		var dropElements = document.querySelectorAll('[data-dropzone]');
		var previewTemplate = "<div class=\"dz-preview dz-file-preview\"><div class=\"dz-image\"><img data-dz-thumbnail /></div><div class=\"dz-details collapse\"><div class=\"dz-size collapse\"><span data-dz-size></span></div><div class=\"dz-filename collapse\"><span data-dz-name></span></div></div><div class=\"dz-progress collapse\"><span class=\"dz-upload\" data-dz-uploadprogress></span></div><div class=\"dz-error-message collapse\"><span data-dz-errormessage></span></div><div class=\"dz-success-mark collapse\"></div><div class=\"dz-error-mark collapse\"></div></div>";

		var dropZoneBuilder = {
			buildDropZone: function (div) {
				var image = item.getElementsByTagName('img')[0];
				var myDropzone = new Dropzone('#' + div.id, {
					url: div.dataset.uploadUrl,
					method: "post",
					headers: {
						'X-CSRFToken': div.dataset.antiForgeryToken
					},
					maxFiles: 1,
					acceptedFiles: div.dataset.acceptedFiles,
					previewTemplate: previewTemplate,
					createImageThumbnails: false

				});
				myDropzone.on("sending", function (file, xhr, formData) {
					formData.append("maxWidth", div.dataset.resizeWidth);
					formData.append("maxHeight", div.dataset.resizeHeight);
					if (div.dataset.targetPath) {
						formData.append("targetPath", div.dataset.targetPath);
					}
				});
				myDropzone.on("success", function (file, serverResponse) {
					myDropzone.removeFile(file);
					var imgUrl = serverResponse[0].resizedUrl;
					if (image) {
						image.src = imgUrl;
					}
					if (div.dataset.targetInputId) {
						var input = document.getElementById(div.dataset.targetInputId);
						if (input) {
							input.value = imgUrl;
						}
					}
				});
			}
		};

		for (var i = 0; i < dropElements.length; i++) {
			var item = dropElements[i];
			var firstImg = item.getElementsByTagName('img')[0];
			dropZoneBuilder.buildDropZone(item, firstImg);
		}
	});
})();
		