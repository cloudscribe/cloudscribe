tinymce.init({
    selector: 'textarea[data-tinymce]',
    menu: {
        file: { title: 'File', items: 'preview | print' },
        edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall | searchreplace' },
        view: { title: 'View', items: 'code | spellchecker | preview fullscreen' },
        insert: { title: 'Insert', items: 'image link media codesample inserttable | charmap emoticons hr | anchor' },
        format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | styles blocks fontfamily fontsize align lineheight | forecolor backcolor | removeformat' },
        tools: { title: 'Tools', items: 'code' },
        table: { title: 'Table', items: 'inserttable | cell row column | advtablesort | tableprops deletetable' },
        help: { title: 'Help', items: 'help' }
    },
    toolbar: [
        'fullscreen code | preview print | cut copy paste pastetext | undo redo | searchreplace | selectall | image imageuploader | codesample | table | hr | charmap | pagebreak | accordion | emoticons | help',
        'blocks | bold italic underline strikethrough subscript superscript | forecolor backcolor | removeformat | numlist bullist | indent outdent | blockquote | alignleft aligncenter alignright alignjustify alignnone | link unlink | anchor'
    ],
    plugins: 'fullscreen code preview searchreplace image imageuploader codesample table charmap pagebreak accordion emoticons lists advlist link anchor',
    promotion: false,
    branding: false,
    license_key: 'gpl'
});