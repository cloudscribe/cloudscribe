// Author: J Audette 2015-05-15, 2017-06-08
$(function () {
    var $elems = $('textarea[data-ckeditor-unobtrusive]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var config = {};

           
            config.toolbar = ($(ele).data('ckeditor-config-toolbar') || "cloudscribedefault");
            config.height = ($(ele).data('ckeditor-config-height') || "200");
            config.title = ($(ele).data('ckeditor-config-title') || "false");

            var configPath = $(ele).data('ckeditor-config-url');
            if (configPath) { config.customConfig = configPath; }
            var width = $(ele).data('ckeditor-config-width');
            if (width) {
                config.width = width;
            }
            var baseHref = $(ele).data('ckeditor-config-basehref');
            if (baseHref) {
                config.baseHref = baseHref;
            }
            var startupFocus = $(ele).data('ckeditor-config-startupfocus');
            if (startupFocus) {
                config.startupFocus = startupFocus;
            }
            var skin = $(ele).data('ckeditor-config-skin');
            if (skin) {
                config.skin = skin;
            }
            var editorId = $(ele).data('ckeditor-config-editorid');
            if (editorId) {
                config.editorId = editorId;
            }
            var language = $(ele).data('ckeditor-config-language');
            if (language) {
                config.language = language;
            }
            var contentsLangDirection = $(ele).data('ckeditor-config-contentslangdirection');
            if (contentsLangDirection) {
                config.contentsLangDirection = contentsLangDirection;
            }
            var contentsCss = $(ele).data('ckeditor-config-contentscss');
            if (contentsCss) {
                config.contentsCss = contentsCss;
            }
            var bodyClass = $(ele).data('ckeditor-config-bodyclass');
            if (bodyClass) {
                config.bodyClass = bodyClass;
            }
            var forcePasteAsPlainText = $(ele).data('ckeditor-config-forcepasteasplaintext');
            if (forcePasteAsPlainText) {
                config.forcePasteAsPlainText = forcePasteAsPlainText;
            }
            var htmlEncodeOutput = $(ele).data('ckeditor-config-htmlencodeoutput');
            if (htmlEncodeOutput) {
                config.htmlEncodeOutput = htmlEncodeOutput;
            }
            var fullPage = $(ele).data('ckeditor-config-fullpage');
            if (fullPage) {
                config.fullPage = fullPage;
            }

            var filebrowserBrowseUrl = $(ele).data('ckeditor-config-filebrowseurl');
            if (filebrowserBrowseUrl) {
                config.filebrowserBrowseUrl = filebrowserBrowseUrl; 
            }
            var filebrowserImageBrowseUrl = $(ele).data('ckeditor-config-imagebrowseurl');
            if (filebrowserImageBrowseUrl) {
                config.filebrowserImageBrowseUrl = filebrowserImageBrowseUrl;
            }
            var filebrowserFlashBrowseUrl = $(ele).data('ckeditor-config-flashbrowseurl');
            if (filebrowserFlashBrowseUrl) {
                config.filebrowserFlashBrowseUrl = filebrowserFlashBrowseUrl;
            }
            var filebrowserImageBrowseLinkUrl = $(ele).data('ckeditor-config-imagebrowselinkurl');
            if (filebrowserImageBrowseLinkUrl) {
                config.filebrowserImageBrowseLinkUrl = filebrowserImageBrowseLinkUrl;
            }
            
            var dropFileUploadUrl = $(ele).data('ckeditor-config-dropfileuploadurl');
            if (dropFileUploadUrl) {
                config.dropFileUploadUrl = dropFileUploadUrl;
                config.dropFileXsrfToken = $('[name="__RequestVerificationToken"]:first').val();
            }

            var editor = CKEDITOR.replace(ele, config);
            
            editor.on('change', function() {
                //console.log('ckeditor onchange');
                window.thisPage = window.thisPage || {};
                window.thisPage.hasUnsavedChanges = true;
            });
            
        });
    }
});