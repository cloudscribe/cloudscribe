CKEDITOR.editorConfig = function( config )
{
	// config.language = 'en';
	config.entities = false;
	config.smiley_path = '/cr/images/emojis/';
	config.scayt_autoStartup = false;
	config.disableNativeSpellChecker = false;
	//config.justifyClasses = [ 'AlignLeft', 'AlignCenter', 'AlignRight', 'AlignJustify' ];
	//config.indentClasses = ['Indent1', 'Indent2', 'Indent3'];
    // config.extraPlugins = 'cloudscribe-filedrop,sourcedialog,codesnippet,autosave';
    config.extraPlugins = 'cloudscribe-filedrop,autosave';
	config.autosave = { saveDetectionSelectors: "a[href^='javascript:__doPostBack'][id*='Save'],a[id*='Cancel'],button[id*='Save']", messageType: "no"};
	config.removePlugins = 'uploadimage'; //cloudscribe now does this via it's own mechanism to support drag/drop/paste image upload

	//this is needed if the language plugin is not removed above. Creates a list for the WCAG 3.1.2 Language of Parts specification
	config.language_list = [ 'en:English', 'cy:Welsh', 'fr:French', 'es:Spanish' ];

    // config.removePlugins = 'scayt,wsc';
	// //config.oembed_maxWidth = '560';
	// //config.oembed_maxHeight = '315';

    config.fillEmptyBlocks = false;
    config.forcePasteAsPlainText = 'allow-word';
    config.filebrowserWindowHeight = '70%';
    config.filebrowserWindowWidth = '80%';

    // config.linkWebSizeToOriginal = true;

	// //config.protectedSource.push(/<i[^>]></i>/g);
    // //config.protectedSource.push(/<span[^>]></span>/g);
	// //config.protectedSource.push(/<div[^>]></div>/g);

	config.allowedContent = true;
    config.extraAllowedContent = 'p(*)[*]{*};div(*)[*]{*};li(*)[*]{*};ul(*)[*]{*};span(*)[*]{*}';
    // CKEDITOR.dtd.$removeEmpty.i = 0;

	config.fontSize_defaultLabel = 'Normal';
    config.fontSize_sizes = 'X-Small/font-xsmall;Small/font-small;Normal/font-normal;Large/font-large;X-Large/font-xlarge';
    config.fontSize_style =
    {
		element:    'span',
		attributes: { 'class': '#(size)' },
		overrides:  [{ element: 'font', attributes: { 'size': null}}]
    };

	config.format_tags = 'p;h1;h2;h3;h4;pre;address;div';

    config.toolbar_cloudscribedefault =
    [
        ['Sourcedialog', 'Maximize'],
        ['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
        ['Undo', 'Redo', '-', 'Find', 'Replace', 'Bold', 'Italic', 'Underline', '-', 'Strike', 'Superscript'],
        '/',
        ['Blockquote', 'Format'], ['NumberedList', 'BulletedList'],
        ['Link', 'Unlink', 'Anchor'],
        ['Image', 'oembed', 'Table', 'HorizontalRule', 'EmojiPanel', 'SpecialChar', 'CodeSnippet']
    ];

	// config.toolbar_CKFull =
	// [
	// 	['Source','-','Save','NewPage','Preview','-','Templates'],
	// 	['Cut','Copy','Paste','PasteText','PasteFromWord','-','Print'],
	// 	['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
	// 	['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
	// 	'/',
	// 	['Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
	// 	['NumberedList','BulletedList','-','Outdent','Indent','Blockquote'],
	// 	['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
	// 	['Link','Unlink','Anchor'],
	// 	['Image','oembed','Table','HorizontalRule','EmojiPanel','SpecialChar','PageBreak','CodeSnippet'],
	// 	'/',
	// 	['Styles','Format','Font','FontSize'],
	// 	['TextColor','BGColor'],
	// 	['Maximize', 'ShowBlocks','-','About']
    //     ];




	// config.toolbar_Full =
    // [
	// 	['Source','Maximize'],
	// 	['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
	// 	['Undo','Redo','-','Find','Replace','Bold','Italic','Underline','-','Strike','Superscript'],
	// 	'/',
	// 	['Blockquote','Format','Styles','FontSize'],['NumberedList','BulletedList'],
	// 	['Link','Unlink','Anchor'],
	// 	['Image','oembed','Table','HorizontalRule','EmojiPanel','SpecialChar', 'CodeSnippet']

    // ];

	// config.toolbar_Newsletter =
    // [
	// 	['Source'],
	// 	['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
	// 	['Undo', 'Redo', '-', 'Find', 'Replace'],
	// 	'/',
	// 	['Blockquote', 'Bold', 'Italic', 'Underline', 'Strike', 'Superscript'],
	// 	['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
	// 	['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
	// 	['Link', 'Unlink', 'Anchor'],
	// 	['Image', 'Table', 'HorizontalRule', 'SpecialChar'],
	// 	'/',
	// 	['Format', 'Font', 'FontSize'],
	// 	['TextColor', 'BGColor'],
	// 	['Maximize','Preview']

    // ];

	// config.toolbar_FullWithTemplates =
    // [
	// 	['Source','Maximize','ShowBlocks'],
	// 	['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
	// 	['Undo','Redo'],['Find','Replace'],['Bold','Italic','Underline','Strike','Superscript'],
	// 	'/',
	// 	['Blockquote','Format','Styles','FontSize'],['NumberedList','BulletedList','Outdent', 'Indent'],['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
	// 	['Link','Unlink','Anchor'],
	// 	['Templates','Image','Flash','oembed','Table','HorizontalRule','EmojiPanel','SpecialChar']

    // ];

	// config.toolbar_Forum =
    // [
	// 	['Cut','Copy','PasteText','-'],
	// 	['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
	// 	['Blockquote','Bold','Italic','Underline'],
	// 	['NumberedList', 'BulletedList'],
	// 	['Link','Unlink'],
	// 	['SpecialChar','EmojiPanel']
	// ];

	// config.toolbar_ForumWithImages =
    // [
	// 	['Cut','Copy','PasteText','-'],
	// 	['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
	// 	['Blockquote','Bold','Italic','Underline','Image'],
	// 	['NumberedList', 'BulletedList'],
	// 	['Link','Unlink'],
	// 	['SpecialChar','EmojiPanel']
	// ];



	// config.toolbar_AnonymousUser =
    // [
	// 	['Cut','Copy','PasteText'],
	// 	['Blockquote', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'EmojiPanel']
	// ];

	// config.toolbar_None = [];



};
