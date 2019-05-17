
https://www.twilio.com/blog/a-http-headers-for-the-responsible-developer

will asp.net core get built in CSP?
https://github.com/aspnet/AspNetCore/issues/6001

https://tahirnaushad.com/2017/09/12/using-csp-header-in-asp-net-core-2-0/

NWebSec hasn't been updated in over a year and issues are not responded to and the ssl cert expired 2/19
https://www.nwebsec.com/


https://github.com/NWebsec/NWebsec

https://docs.nwebsec.com/en/latest/

https://www.troyhunt.com/locking-down-your-website-scripts-with-csp-hashes-nonces-and-report-uri/

https://report-uri.com/

https://github.com/NWebsec/NWebsec/issues/100

Provide option to re-set CSP Header
https://github.com/NWebsec/NWebsec/issues/126

https://www.tpeczek.com/2016/12/content-security-policy-in-aspnet-core.html

https://www.troyhunt.com/my-blog-now-has-a-content-security-policy-heres-how-ive-done-it/

https://github.com/OWASP/CheatSheetSeries/blob/master/cheatsheets/HTTP_Strict_Transport_Security_Cheat_Sheet.md



https://github.com/ckeditor/ckeditor-dev/pull/254

https://paul.kinlan.me/using-nonce-with-service-workers/

jquery.unobtrusiveajax uses eval so cant block it with CSP:
https://github.com/aspnet/jquery-ajax-unobtrusive/issues/49
https://github.com/aspnet/jquery-ajax-unobtrusive/issues/23


Pages with ckeditor 4 cannot work fully without unsafe-inline

ckeditor add this:
<style>.cke{visibility:hidden;}</style>


sha256-tanzF8DiJ75nNsRkF5RQ4Ps7Hp6+SCOSyRkOStzgw44=
sha256-tanzF8DiJ75nNsRkF5RQ4Ps7Hp6+SCOSyRkOStzgw44=

PV/ToBAddv8tOHCMdgd5VTkp0O0R1Mny02U3sDYoNac=
PV/ToBAddv8tOHCMdgd5VTkp0O0R1Mny02U3sDYoNac=

{script-src 'self' 'unsafe-eval' https://www.google.com/recaptcha/;style-src 'self' 'unsafe-inline';img-src 'self' https://secure.gravatar.com;font-src 'self';form-action 'self';frame-ancestors 'self';block-all-mixed-content}

{script-src 'self' 'unsafe-eval' https://www.google.com/recaptcha/ 'sha256-y8DInSr2zF7PN5eoUJaOub06SWAs7LS0I9qvOBzB24w=' 'sha256-kCHLgxFYfRBgcPvUY36pivVG5Yzj/sXVNua5iRd7Cog=' 'sha256-jXsJOuxldB0vgf1I6X5N+ebOXi/v0v61nCxWZeyw1t8=';style-src 'self' 'unsafe-inline';img-src 'self' https://secure.gravatar.com;font-src 'self';form-action 'self';frame-ancestors 'self';block-all-mixed-content}
