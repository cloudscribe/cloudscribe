

http://ogp.me/

https://developers.facebook.com/docs/sharing/webmasters

http://schema.org/Blog

https://developer.mozilla.org/en-US/docs/Learn/HTML/Introduction_to_HTML/The_head_metadata_in_HTML

<meta property="og:image" content="https://developer.cdn.mozilla.net/static/img/opengraph-logo.dc4e08e2f6af.png">
<meta property="og:description" content="The Mozilla Developer Network (MDN) provides
information about Open Web technologies including HTML, CSS, and APIs for both Web sites
and HTML5 Apps. It also documents Mozilla products, like Firefox OS.">
<meta property="og:title" content="Mozilla Developer Network">

https://github.com/ghorsey/OpenGraph-Net

taghelpers
https://rehansaeed.com/social-taghelpers-for-asp-net-core/
https://github.com/ASP-NET-Core-Boilerplate/Framework/tree/master/Source/MVC6/Boilerplate.AspNetCore.TagHelpers

## boilerplate

 @* See https://developers.google.com/web/updates/2014/11/Support-for-installable-web-apps-with-webapp-manifest-in-chrome-38-for-Android  *@
    <link rel="manifest" href="~/manifest/manifest.json">
    @* See https://developers.google.com/web/updates/2014/11/Support-for-theme-color-in-Chrome-39-for-Android *@
    <meta name="theme-color" content="#3f51b5">
    @*  Add to homescreen for Chrome on Android. Fallback for manifest.json *@
    <meta name="mobile-web-app-capable" content="yes">
    <meta name="application-name" content="@(Tenant?.SiteName ?? "MyApp")">
    @*  Add to homescreen for Safari on iOS *@
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent">
    <meta name="apple-mobile-web-app-title" content="@(Tenant?.SiteName ?? "MyApp")">
    @*  Homescreen icons *@
    <link rel="apple-touch-icon" href="~/hbsmrColchester/images/manifest/icon-48x48.png">
    <link rel="apple-touch-icon" sizes="72x72" href="~/images/manifest/icon-72x72.png">
    <link rel="apple-touch-icon" sizes="96x96" href="~/images/manifest/icon-96x96.png">
    <link rel="apple-touch-icon" sizes="144x144" href="~/images/manifest/icon-144x144.png">
    <link rel="apple-touch-icon" sizes="192x192" href="~/images/manifest/icon-192x192.png">
    @*  Tile icon for Windows 8 (144x144 + tile color) *@
    <meta name="msapplication-TileImage" content="/images/manifest/icon-144x144.png">
    <meta name="msapplication-TileColor" content="#FFFFFF">
    <meta name="msapplication-tap-highlight" content="no">
