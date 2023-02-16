# ckeditorfa5
ckeditor fontAwesome 5 plugin with Font Awesome v.5.x (current branch is compatible with Font Awesome **v.5.15.x**)

# settings
1. add in your html page all.min.css
2. add in your config.js from ckeditor folder:
   - config.extraPlugins = 'ckeditorfa';
   - config.allowedContent = true;
   - config.contentsCss = '/{your_path}/all.min.css';

CKEDITOR.dtd.$removeEmpty['span'] = false;

# info
for Font Awesome v.4.7 use branch **fa4**
