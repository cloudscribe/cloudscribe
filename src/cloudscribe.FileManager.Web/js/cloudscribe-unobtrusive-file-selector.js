(function () {
	document.addEventListener("DOMContentLoaded", function () {
		var fileSelectorElements = document.querySelectorAll('[data-file-selector]');
		window.cloudscribeFileSelector = {
			openServerBrowser: function (fileBrowseUrl) {
				$('#fileBrowseDialog').find('iframe').attr('src', fileBrowseUrl);
				$('#fileBrowseDialog').modal('show');
			},
			closeServerBrowser: function () {
				$('#fileBrowseDialog').modal('hide');
			},

			buildFileSelector: function (btn) {

				var fileSelector = {
					selectorButton: btn,
					serverFileSelected: function (url) {
						//console.log(url);
						if (btn.dataset.targetInputId) {
							document.getElementById(btn.dataset.targetInputId).value = url;
						}
						if (btn.dataset.altTargetInputId) {
							document.getElementById(btn.dataset.altTargetInputId).value = url;
						}
						cloudscribeFileSelector.closeServerBrowser();
					}

				};

				fileSelector.selectorButton.onclick = function () {
					window.FileSelectCallback = function (url) {
						fileSelector.serverFileSelected(url);
					};
					cloudscribeFileSelector.openServerBrowser(btn.dataset.fileBrowseUrl);
				}; 
			}   
		};

		for (var i = 0; i < fileSelectorElements.length; i++) {
			var item = fileSelectorElements[i];
			cloudscribeFileSelector.buildFileSelector(item);
		}
		
	});
	
})();