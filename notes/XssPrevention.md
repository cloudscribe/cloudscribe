

https://www.mike-gualtieri.com/posts/stealing-data-with-css-attack-and-defense

https://davidmurdoch.com/2017/09/02/the-grave-accent-and-xss/

https://www.contextis.com/resources/blog/comma-separated-vulnerabilities/


https://scotthelme.co.uk/csrf-is-dead/

http://blog.novanet.no/a-pile-of-anti-forgery-cookies/

Input Filtering:

Our approach in mojoPortal has always been to assume that in fact a user can insert malicious content in a wysiwyg editor. We distinguish between untrusted users and trusted users, and untrusted content vs trusted content created by the corresponding user type.

We filter untrusted content on output rather than input and don't expect the wysiwyg to filter the content for us at input time. In fact someone could easily disable javascript bypassing the editor and inserting content directly in the text area anyway.

With input filtering you only get one try to filter the content and if you miss something it would be a problem. With output filtering we can always change the filter if it proves to let something through.

So the fact that wysiwyg editors don't protect us from dirty input doesn't concern me, in fact we usually disable the built in filtering because it blocks trusted users from adding javascript.

The only kind of editor xss I would be concerned about is if the user could be sent a malicious link to an edit page within their site and some unexpected script could be run to attack that user. Of course such a link would probably have to have a lot of unexpected stuff in the url itself which is one of the reasons I favor plain text email over html which can hide the url behind a link.

If I were trying to really filter input, then I would probably use BB Code or markdown at input time instead of a normal html wysiwyg (like stackoverflow uses). Then the markdown or bbcode content would be used to generate safe html at output time.

In cloudscribe when I build the new content system I've been thinking in terms of a content type field which would indicate the type as html, markdown, bbcode, plaintext, etc so we would have options to use things like that. But I still think for non technical users html wysiwyg is the most user friendly and most normal people can't be expected to know markdown or bbcode syntax.

Output Filtering:

We can support markdown and/or bbcode for input and the corresponding processing should produce safe html output.
However markdown or bbcode is not a friendly option for most people who are not technical, so we still need to support html input and we need a way to filter untrusted raw html on output.

Untrusted html content should be filtered on output. In mojoPortal we used NeatHtml for this. For cloudscribe we need to investigate what is available.

http://weblog.west-wind.com/posts/2012/Jul/19/NET-HTML-Sanitation-for-rich-HTML-Input
https://github.com/RickStrahl/HtmlSanitizer

http://htmlagilitypack.codeplex.com/
http://www.nuget.org/packages/HtmlAgilityPack

https://eksith.wordpress.com/2011/06/14/whitelist-santize-htmlagilitypack/

https://gist.github.com/ntulip/814428

CsQuery is like jquery implemented in C# with server side dom to manipulate
https://github.com/jamietre/CsQuery

https://www.owasp.org/index.php/Category:OWASP_AntiSamy_Project_.NET

https://www.owasp.org/index.php/XSS_%28Cross_Site_Scripting%29_Prevention_Cheat_Sheet

http://security.stackexchange.com/questions/49315/escaping-rich-text-editor-output/49342#49342


Content Security Policy is another way to prevent unwanted external scripts/resources
http://content-security-policy.com/
browser support info for the Content-Security-Policy response header
http://caniuse.com/contentsecuritypolicy

need to google Content Security Policy for more info, I think I recal a .NET library
probably can find or implement something that plugs into the owin pipeline

https://stackoverflow.com/questions/20504846/why-is-it-common-to-put-csrf-prevention-tokens-in-cookies

