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

			this.initialize = function() {
				// get the correct container for the plugin where's it's attached to the document DOM.
				var $container = options.dialogsInBody ? $(document.body) : $editor;

				// Build the Body HTML of the Dialog.
				var body = '<div class="ratio ratio-16x9"><iframe src="/filemanager/filedialog" data-wysiwyg-instance="' + $($container).prev("textarea")[0]["id"] + '" id="instanceId"/></div>';

				this.$dialog = ui.dialog({
					// Set the title for the Dialog.
					title: lang.serverimage.dialogTitle,
					// Set the Body of the Dialog.
					body: body,
					callback: function (t) {
						t.find(".modal-dialog").addClass("modal-xl");
						$(document.body).find('[aria-label="serverimage"]').attr('id', 'serverimage');
					},
				})
				.render()
				.appendTo($container);
			}
	
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
			
			this.showserverimageDialog = function(editorInfo) {
				return $.Deferred(function (deferred) {
					ui.onDialogShown(self.$dialog,function () {
						context.triggerEvent('dialog.shown');
					});
					ui.onDialogHidden(self.$dialog,function () {
						if (deferred.state() === 'pending') deferred.reject();
					});
					ui.showDialog(self.$dialog);
				});
			};
		}
	});
}));