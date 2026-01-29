/**
 * 
 * copyright 2024 ESDM, an Idox Company.
 * email: info@idoxgroup.com
 * license: Your chosen license, or link to a license file.
 * 
 */
(function (factory) {
	if (typeof define === 'function' && define.amd) {
		define(['jquery'], factory);
	} else if (typeof module === 'object' && module.exports) {
		module.exports = factory(require('jquery'));
	} else {
		factory(window.jQuery);
	}
}(function ($) {
	/**
	* @class plugin.serverimage
	*
	* allow users to select an image from the server.
	*/
	$.extend(true,$.summernote.lang, {
		'en-US': {
			serverimage: {
				dialogTitle: 'serverimage'
			}
		}
	});
  
	$.extend($.summernote.options, {
		serverimage: {
			icon: '<i class="fa fa-file-image"/>',
			title: 'Images from the Server'
		}
	});

	$.extend($.summernote.plugins, {
		'serverimage':function (context) {
			var self      = this,
			ui        = $.summernote.ui,
			$note     = context.layoutInfo.note,
			$editor   = context.layoutInfo.editor,
			$editable = context.layoutInfo.editable,
			$toolbar  = context.layoutInfo.toolbar,
			options   = context.options,
			lang      = options.langInfo;

			//translations
			switch (options.lang) {
				case "fr-FR":
					lang.serverimage.title = "Images du serveur";
					break;
				case "it-IT":
					lang.serverimage.title = "Immagini dal server";
					break;
				case "cy":
					lang.serverimage.title = "Delweddau o'r Gweinydd";
					break;
				default:
					lang.serverimage.title = "Images from the Server";
			}

			context.memo('button.serverimage',function () {
				var button = ui.button({
					contents: options.serverimage.icon,
					tooltip: lang.serverimage.title,
					click:function (e) {
						context.invoke('serverimage.show');
					}
				});
				return button.render();
			});



			this.initialize = function () {
				this.placeholderId = `serverimage-placeholder-${Math.random().toString(36).substr(2, 9)}`;
				// get the correct container for the plugin where's it's attached to the document DOM.				
				const $container = options.dialogsInBody ? $(document.body) : $editor;

				const body = `
					<div class="ratio ratio-16x9">
						<div id="${this.placeholderId}"></div>
					</div>
				`;

				// create an empty dialog container first
				//moved the creation of the iframe showserverimageDialog
				//pages with lots of comments were getting swamped with resource requests from the iframe loading

				this.$dialog = ui.dialog({
					title: lang.serverimage.title,
					body: body,
					callback: function (t) {
						t.find('.modal-dialog').css({
							width: '85vw',
							maxWidth: '1500px'
						});
					}
				}).render().appendTo($container)

			};
				
			this.destroy = function () {
				ui.hideDialog(this.$dialog);
				this.$dialog.remove();
			};
			
			this.bindEnterKey = function ($input,$btn) {
				$input.on('keypress',function (event) {
					if (event.keyCode === 13) $btn.trigger('click');
				});
			};
			
			this.bindLabels = function () {
				self.$dialog.find('.form-control:first').focus().select();
				self.$dialog.find('label').on('click', function () {
					$(this).parent().find('.form-control:first').focus();
				});
			};
			
			this.show = function () {
				var $img = $($editable.data('target'));
				var editorInfo = {};
				
				this.showserverimageDialog(editorInfo).then(function (editorInfo) {
					ui.hideDialog(self.$dialog);
					$note.val(context.invoke('code'));
					$note.change();
				});
			};
			
			this.showserverimageDialog = function (editorInfo) {
				return $.Deferred(function (deferred) {

					const placeholder = self.$dialog.find(`#${self.placeholderId}`);

					// Lazy load iframe ONLY once
					if (placeholder.is(':empty')) {
						const iframeHtml = `
											<iframe 
												src="/filemanager/filedialog?Type=image&ShowModalHeader=false" 
												data-wysiwyg-instance="${$($editor).prev("textarea")[0].id}"
												class="w-100 h-100"
											></iframe>
										`;

						placeholder.html(iframeHtml);
					}

					ui.onDialogShown(self.$dialog, function () {
						context.triggerEvent('dialog.shown');
					});

					ui.onDialogHidden(self.$dialog, function () {
						if (deferred.state() === 'pending') deferred.reject();
					});

					ui.showDialog(self.$dialog);
				});
			};
		}
	});
}));