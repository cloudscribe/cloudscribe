// Author: J Audette 2015-05-15
$(function () {
    var $elems = $('textarea[data-ckeditor-unobtrusive]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var config = {};
            config.toolbar = ($(ele).data('ckeditor-unobtrusive-config-toolbar') || "AnonymousUser");
            config.height = ($(ele).data('ckeditor-unobtrusive-config-height') || "200");
            config.title = ($(ele).data('ckeditor-unobtrusive-config-title') || "false");

            var configPath = $(ele).data('ckeditor-unobtrusive-config-url');
            if (configPath) { config.customConfig = configPath; }
            var width = $(ele).data('ckeditor-unobtrusive-config-width');
            if (width) {
                config.width = width;
            }
            var baseHref = $(ele).data('ckeditor-unobtrusive-config-basehref');
            if (baseHref) {
                config.baseHref = baseHref;
            }
            var startupFocus = $(ele).data('ckeditor-unobtrusive-config-startupfocus');
            if (startupFocus) {
                config.startupFocus = startupFocus;
            }
            var skin = $(ele).data('ckeditor-unobtrusive-config-skin');
            if (skin) {
                config.skin = skin;
            }
            var editorId = $(ele).data('ckeditor-unobtrusive-config-editorid');
            if (editorId) {
                config.editorId = editorId;
            }
            var language = $(ele).data('ckeditor-unobtrusive-config-language');
            if (language) {
                config.language = language;
            }
            var contentsLangDirection = $(ele).data('ckeditor-unobtrusive-config-contentslangdirection');
            if (contentsLangDirection) {
                config.contentsLangDirection = contentsLangDirection;
            }
            var contentsCss = $(ele).data('ckeditor-unobtrusive-config-contentscss');
            if (contentsCss) {
                config.contentsCss = contentsCss;
            }
            var bodyClass = $(ele).data('ckeditor-unobtrusive-config-bodyclass');
            if (bodyClass) {
                config.bodyClass = bodyClass;
            }
            var forcePasteAsPlainText = $(ele).data('ckeditor-unobtrusive-config-forcepasteasplaintext');
            if (forcePasteAsPlainText) {
                config.forcePasteAsPlainText = forcePasteAsPlainText;
            }
            var htmlEncodeOutput = $(ele).data('ckeditor-unobtrusive-config-htmlencodeoutput');
            if (htmlEncodeOutput) {
                config.htmlEncodeOutput = htmlEncodeOutput;
            }
            var fullPage = $(ele).data('ckeditor-unobtrusive-config-fullpage');
            if (fullPage) {
                config.fullPage = fullPage;
            }

            var filebrowserBrowseUrl = $(ele).data('ckeditor-unobtrusive-config-filebrowserbrowseurl');
            if (filebrowserBrowseUrl) {
                config.filebrowserBrowseUrl = filebrowserBrowseUrl; 
            }
            var filebrowserImageBrowseUrl = $(ele).data('ckeditor-unobtrusive-config-filebrowserimagebrowseurl');
            if (filebrowserImageBrowseUrl) {
                config.filebrowserImageBrowseUrl = filebrowserImageBrowseUrl;
            }
            var filebrowserFlashBrowseUrl = $(ele).data('ckeditor-unobtrusive-config-filebrowserflashbrowseurl');
            if (filebrowserFlashBrowseUrl) {
                config.filebrowserFlashBrowseUrl = filebrowserFlashBrowseUrl;
            }
            var filebrowserImageBrowseLinkUrl = $(ele).data('ckeditor-unobtrusive-config-filebrowserimagebrowselinkurl');
            if (filebrowserImageBrowseLinkUrl) {
                config.filebrowserImageBrowseLinkUrl = filebrowserImageBrowseLinkUrl;
            }
            if (filebrowserBrowseUrl || filebrowserImageBrowseUrl || filebrowserFlashBrowseUrl || filebrowserImageBrowseLinkUrl) {
                config.filebrowserWindowWidth = ($(ele).data('ckeditor-unobtrusive-config-filebrowserwindowwidth') || "860");
                config.filebrowserWindowHeight = ($(ele).data('ckeditor-unobtrusive-config-filebrowserwindowheight') || "700");
                config.filebrowserWindowFeatures = ($(ele).data('ckeditor-unobtrusive-config-filebrowserwindowfeatures') || "location=no,menubar=no,toolbar=no,dependent=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=yes,scrollbars=yes");
            }
            var dropFileUploadUrl = $(ele).data('ckeditor-unobtrusive-config-dropfileuploadurl');
            if (dropFileUploadUrl) {
                config.dropFileUploadUrl = dropFileUploadUrl;
            }
            
            $(ele).ckeditor(config);
            
        });
    }
});