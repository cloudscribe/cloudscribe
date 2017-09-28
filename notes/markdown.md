
http://xoofx.com/blog/2016/06/13/implementing-a-markdown-processor-for-dotnet/
https://github.com/lunet-io/markdig

https://dotnetthoughts.net/markdown-tag-helper-for-aspnet-core/
https://github.com/lunet-io/markdig
https://www.nuget.org/packages/Markdig/

https://www.nuget.org/packages/Markdown/

http://commonmark.org/
https://github.com/commonmark/CommonMark



markdown is used in github frequently for documentation
it is also used on stackoverflow and other sites for users to enter text that can be safely converted to html and preventing xss

we should probably support markdoqn as a data format when we build the cms
ie content would be captured and stored as markdown and then converted to html at render time

we would need a javascript editor to suport markdown with preview in the browser
and we would need server side code to convert markdown to html 

the editor used on stackoverflow
https://code.google.com/p/pagedown/wiki/PageDown

https://code.google.com/p/markdownsharp/  (not updated since 2010 which seems strange)

this server side one says it even supports .net vnext, usage looks good
https://github.com/Knagis/CommonMark.NET/

this one claims better server side perf and has both client side and server side implementations
hasn't made a new release since 2012
http://www.toptensoftware.com/markdowndeep/ apache license
https://github.com/toptensoftware/markdowndeep

another one
https://github.com/OscarGodson/EpicEditor

markdown is kind of geeky, can it be made friendly for non techies?
http://stackoverflow.com/questions/12840132/integrated-markdown-wysiwyg-text-editor
maybe using ckeditor with a markdown plugin

a brand new server side one
https://github.com/media-tools/core/

https://github.com/toopay/bootstrap-markdown
https://www.nuget.org/packages/bootstrap-markdown.less
http://www.codingdrama.com/bootstrap-markdown/

https://github.com/evilstreak/markdown-js

## Markdown with metadata

If we were to support makrdown in simplecontent we would need to be able to store additional metadata that is not neccessarily rendered.
ie postid, pubdate, lastmodified, settings

https://hiltmon.com/blog/2012/06/18/markdown-metadata/
https://github.com/fletcher/MultiMarkdown - no longer maintained

## Markdown static site generators

https://easystatic.com/  - node

https://wyam.io/ - .NET

https://github.com/mixu/markdown-styles

http://www.mkdocs.org/  python
