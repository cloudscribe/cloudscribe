/*
 * cloudscribe-filedrop plugin for CKEditor
 * Copyright (C) 2013 Joe Audette, Source Tree Solutions LLC
 * Created 2013-11-28
 * Last Modified 2017-02-20
 *
 */

CKEDITOR.plugins.add( 'cloudscribe-filedrop',
{
	init : function( editor )
	{
		if(!(editor.config.dropFileUploadUrl)) {  return; }
	
		var theEditor = editor;
		var uploadUrl = editor.config.dropFileUploadUrl;
		var xsrfToken = editor.config.dropFileXsrfToken;
		var isLocked = false;
		var linkToOrig = editor.config.linkWebSizeToOriginal;
		
		function onDragStart(event) {                 
                //console.log("onDragStart");
        };
			
		function onDragOver(event) { 
				event.preventDefault();
				return false;
                //console.log("onDragOver");	
         };
			
		
		function onDropped(event) { 
				event = event || window.event;
				
				var files = event.dataTransfer.files || event.target.files; 
				if(files) {
					event.preventDefault();
					event.stopPropagation ();
					if(!isLocked) { isLocked = theEditor.lockSelection(); }
					uploadFile(files[0]); // one file at a time
				}
            };
			
		function uploadFile(file) {
		
			switch(file.type){
				case "image/jpeg":
				case "image/jpg":
				case "image/gif":
				case "image/png":

				var formData = new FormData();
				formData.append("__RequestVerificationToken", xsrfToken);
				formData.append(file.name, file);
					
				$.ajax({
					type:"POST",
					processData: false,
					contentType: false,
					dataType: "json",
					url: uploadUrl,
					data: formData,
					success: uploadSuccess,
					complete: ajaxComplete
					});
					
					break;
			}
		}
		
		function ajaxComplete() {
			if(isLocked) {
			  theEditor.unlockSelection();
			  isLocked = false;
			}
		}
		
		function uploadSuccess( data, textStatus, jqXHR ) {
			
			try {
			    if(data[0].errorMessage) { alert(data[0].errorMessage); return; }
				
				if(data[0].resizedUrl) {
				    if(linkToOrig)
					{
					    theEditor.insertHtml( "<a href='" + data[0].originalUrl +"'><img src='" + data[0].resizedUrl + "' alt=' ' /></a>" );
					}
					else {
					theEditor.insertHtml( "<img src='" + data[0].resizedUrl + "' alt=' ' />" );
					}
				}
				else {
					theEditor.insertHtml( "<img src='" + data[0].originalUrl + "' alt=' ' />" );
				}
			} catch(err) {
				//console.log(err);
				try {
					theEditor.focus();
					theEditor.unlockSelection(true);
					isLocked = false;
					if(data.files[0].resizedUrl) {
						theEditor.insertHtml( "<a href='" + data[0].originalUrl +"'><img src='" + data[0].resizedUrl + "' alt=' ' /></a>" );
					}
					else {
						theEditor.insertHtml( "<img src='" + data[0].originalUrl + "' alt=' ' />" );
					}
				} catch(err2) {
					//console.log(err2);
				}
			}
			
		}
			 
		editor.on('instanceReady', function (e) {
			// make sure the browser supports the file api
			if (window.File && window.FileList && window.FileReader) {
				//editor.document.on('dragstart', onDragStart);
				//editor.document.on('drop', onDrop);
				//editor.document.on('dragover', onDragOver);
				editor.document.$.addEventListener("drop", onDropped, true);
				editor.document.$.addEventListener("dragover", onDragOver, true);
				
				}
		});
		
		editor.on('blur', function (e) {
			// make sure the browser supports the file api
			if (window.File && window.FileList && window.FileReader) {
				isLocked = theEditor.lockSelection(); //this is needed for ie but should not really be needed here seems like a bug in the editor 
				
				}
		});
		
			
	} //Init
} );
