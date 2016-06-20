
http://stackoverflow.com/questions/27677345/what-are-a-security-token-and-security-stamp-in-asp-net-identity

http://www.dotnetcurry.com/aspnet/1223/secure-aspnet-web-api-using-tokens-owin-angularjs

http://stackoverflow.com/questions/29048122/token-based-authentication-in-asp-net-5-vnext
http://stackoverflow.com/questions/34612631/prevent-token-based-authorization-of-preflight-options-requests-in-asp-net-5-vn

https://github.com/mrsheepuk/ASPNETSelfCreatedTokenAuthExample


http://www.simplecloud.info/
System for Cross-domain Identity Management
SCIM 2, the open API for managing identities is now complete and published under the IETF.

http://damienbod.com/2015/11/08/oauth2-implicit-flow-with-angular-and-asp-net-5-identity-server/


http://bitoftech.net/2015/03/31/asp-net-web-api-claims-authorization-with-asp-net-identity-2-1/

http://www.asp.net/identity/overview/migrations/migrating-an-existing-website-from-sql-membership-to-aspnet-identity

https://aspnetidentity.codeplex.com/workitem/2333


http://odetocode.com/blogs/scott/archive/2014/01/20/implementing-asp-net-identity.aspx

http://msdn.microsoft.com/en-us/library/microsoft.aspnet.identity%28v=vs.108%29.aspx

http://stackoverflow.com/questions/19487322/what-is-asp-net-identitys-iusersecuritystampstoretuser-interface

http://blogs.msdn.com/b/webdev/archive/2014/02/12/per-request-lifetime-management-for-usermanager-class-in-asp-net-identity.aspx

http://odetocode.com/blogs/scott/archive/2014/01/20/implementing-asp-net-identity.aspx

http://msdn.microsoft.com/en-us/library/microsoft.aspnet.identity%28v=vs.108%29.aspx

http://stackoverflow.com/questions/19487322/what-is-asp-net-identitys-iusersecuritystampstoretuser-interface

http://brockallen.com/2013/10/20/the-good-the-bad-and-the-ugly-of-asp-net-identity/

http://brockallen.com/2014/02/11/introducing-identityreboot/
https://github.com/brockallen/BrockAllen.IdentityReboot

http://brockallen.com/2014/02/09/how-membershipreboot-stores-passwords-properly/

http://brockallen.com/2014/02/10/how-membershipreboot-mitigates-login-and-two-factor-authentication-brute-force-attacks/

http://brockallen.com/2014/02/11/concerns-with-two-factor-authentication-in-asp-net-identity-v2/

https://github.com/IdentityServer/Thinktecture.IdentityServer3

http://identityserver.github.io/Documentation/docs/

### Old Notes

MS does not show much interest in helping us with multi tenancy
https://github.com/aspnet/Security/issues/35

seems like this is what we want to override
IAuthenticationHandler
https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http.Features/Authentication/IAuthenticationHandler.cs
public interface IAuthenticationHandler
{
	void GetDescriptions(DescribeSchemesContext context);

	Task AuthenticateAsync(AuthenticateContext context);

	Task ChallengeAsync(ChallengeContext context);

	Task SignInAsync(SignInContext context);

	Task SignOutAsync(SignOutContext context);
}

public abstract class AuthenticationHandler : IAuthenticationHandler
https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationHandler.cs

https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationHandler%601.cs

HttpAuthenticationFeature implements IHttpAuthenticationFeature contains ref to IAuthenticationHandler and ClaimsPrincipal User



http://blog.dudak.me/2015/non-linear-middleware-chains-in-asp-net-5/

https://github.com/OrchardCMS/Orchard2/blob/5342a792dbac9fb70a7d76c2e17bfee7b9c0ba2c/src/Orchard.Hosting.Web/Routing/Routes/TenantRoute.cs




https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http/Features/Authentication/HttpAuthenticationFeature.cs

abstract AuthenticationManager
https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http.Abstractions/Authentication/AuthenticationManager.cs

DefaultAuthenticationManager inherits AuthenticationManager
https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNet.Http/Authentication/DefaultAuthenticationManager.cs
private FeatureReference<IHttpAuthenticationFeature> _authentication = FeatureReference<IHttpAuthenticationFeature>.Default;
takes IFeatureCollection features in its constructor
private IHttpAuthenticationFeature HttpAuthenticationFeature
{
	get { return _authentication.Fetch(_features) ?? _authentication.Update(_features, new HttpAuthenticationFeature()); }
}

this is internal so we can't inherit from it and it depends on wired up cookie options
CookieAuthenticationHandler 
https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Cookies/CookieAuthenticationHandler.cs

CookieAuthenticationMiddleware is public so we can perhaps inherit from it and override things
public class CookieAuthenticationMiddleware : AuthenticationMiddleware<CookieAuthenticationOptions>
https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Cookies/CookieAuthenticationMiddleware.cs

it creates the CookieAuthenticationHandler
protected override AuthenticationHandler<CookieAuthenticationOptions> CreateHandler()
{
	return new CookieAuthenticationHandler();
}

which in turn uses the wired up cookie options
by implementing ourt own cookie middleware we could wire up our own handler that inherits from AuthenticationHandler<CookieAuthenticationOptions>
internally we could ignore the options or use them depending on context

public abstract class AuthenticationMiddleware<TOptions> where TOptions : AuthenticationOptions, new()
https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationMiddleware.cs
