
https://arstechnica.com/information-technology/2018/03/lets-encrypt-takes-free-wildcard-certificates-live/

https://letsencrypt.org/docs/client-options/


https://blogs.msdn.microsoft.com/webdev/2017/11/29/configuring-https-in-asp-net-core-across-different-platforms/

https://blog.benroux.me/running-multiple-https-domains-from-the-same-server/

https://weblog.west-wind.com/posts/2016/Feb/22/Using-Lets-Encrypt-with-IIS-on-Windows

https://weblog.west-wind.com/posts/2016/Jul/09/Moving-to-Lets-Encrypt-SSL-Certificates

http://certify.webprofusion.com/

https://letsencrypt.org/2016/07/26/full-ipv6-support.html

https://letsencrypt.org/docs/rate-limits/

https://tech.slashdot.org/story/16/09/26/2028215/mozillas-proposed-conclusion-game-over-for-wosign-and-startcom

https://medium.com/@MaartenSikkema/automatically-request-and-use-lets-encrypt-certificates-in-dotnet-core-9d0d152a59b5#.5753cuqpd

LetsEncrypt's validation process includes creating some special files in a known location within the website and checking they can be accessed in that location from the internet, but this will fail because by default cloudscribe will not serve up extensionless static files. The solution is to make the following changes to web.config in the root of the site (not the wwwroot folder).
Before
<handlers>
     <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified"/>
</handlers>
After
<handlers accessPolicy="Script,Read">
    <add name="LetsEncrypt" path=".well-known/acme-challenge/*" verb="*" modules="StaticFileModule" preCondition="integratedMode" resourceType="File" requireAccess="Read" />
    <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
</handlers>
