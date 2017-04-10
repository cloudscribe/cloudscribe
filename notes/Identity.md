
https://leastprivilege.com/2016/12/16/identity-vs-permissions/

https://identityserver4.readthedocs.io/en/release/topics/federation_gateway.html


http://stackoverflow.com/questions/41682434/how-to-put-asp-net-identity-roles-into-the-identityserver4-identity-token

https://stackoverflow.com/questions/41687659/how-to-add-additional-claims-to-be-included-in-the-access-token-using-asp-net-id

https://damienbod.com/2016/02/14/authorization-policies-and-data-protection-with-identityserver4-in-asp-net-core/


https://blogs.msdn.microsoft.com/webdev/2017/01/23/asp-net-core-authentication-with-identityserver4/

## My Thoughts around IdentityServer4

All the samples separate the OP (OpenID Connect provider) aka Identity Server, into a separate web app which is great for scalability. The samples often have multiple clients and maybe an api so you spin up 3 or 4 web apps which is a good way to show how the distributed system can work.

Being able to separate those services to different end points is crucial for the scalability.

From my point of view, it is good to build all apps with scalability in mind, but that doesn't mean all apps really need it or will see the work load to justify it. So the kind of starter template I want for Identity server is a way to run all the parts from a single endpoint in such a way that you can still separate them to different endpoints later if the need for scale merits it. It is too much ceremony to have a bunch of separate web apps for every project, and of course costs to deploy multiple end points is higher. I want a pattern to build apps at a single endpoint where the pieces are composed together from nugets. But, this must be done in such a way that by packaging and configuration changes the pieces can be split apart later and one by one separate pieces can be moved to different endpoints if the workload gets to the point where that is needed.

Within a single web deployement there can be the identity server, mvc web app, various apis, as a single endpoint, with different urls of course but deployed as a single web application in a single IIS web site or nginx/docker web setup. There could also be within the same deployment SPA apps with a single html page each that talks to apis. At any point where the load merits it the spa apps could be separated to their own end points but until then they can be packaged together under one website.


A signing certificate is a dedicated certificate used to sign tokens, allowing for client applications to verify that the contents of the token have not been altered in transit. This involves a private key used to sign the token and a public key to verify the signature. This public key is accessible to client applications via the jwks_uri in the OpenID Connect discovery document.
When you go to create and use your own signing certificate, feel free to use a self-signed certificate. This certificate does not need to be issued by a trusted certificate authority.
https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4


https://identityserver4.readthedocs.io/en/dev/intro/big_picture.html

https://identityserver4.readthedocs.io/en/dev/quickstarts/6_aspnet_identity.html

http://andrewlock.net/an-introduction-to-openid-connect-in-asp-net-core/

https://blogs.msdn.microsoft.com/webdev/2016/09/19/introducing-identityserver4-for-authentication-and-access-control-in-asp-net-core/

https://identityserver4.readthedocs.io/en/dev/quickstarts/0_overview.html

http://stackoverflow.com/questions/27677345/what-are-a-security-token-and-security-stamp-in-asp-net-identity

http://www.dotnetcurry.com/aspnet/1223/secure-aspnet-web-api-using-tokens-owin-angularjs

http://stackoverflow.com/questions/29048122/token-based-authentication-in-asp-net-5-vnext
http://stackoverflow.com/questions/34612631/prevent-token-based-authorization-of-preflight-options-requests-in-asp-net-5-vn

https://github.com/mrsheepuk/ASPNETSelfCreatedTokenAuthExample

https://github.com/IdentityModel/oidc-client-js
https://github.com/IdentityModel/oidc-client-js/wiki

OpenID Connect Basic Client Implementer's Guide
https://openid.net/specs/openid-connect-basic-1_0.html

https://nat.sakimura.org/2014/12/10/making-a-javascript-openid-connect-client/
https://github.com/GluuFederation/openid-implicit-client


http://www.simplecloud.info/
System for Cross-domain Identity Management
SCIM 2, the open API for managing identities is now complete and published under the IETF.

http://damienbod.com/2015/11/08/oauth2-implicit-flow-with-angular-and-asp-net-5-identity-server/

https://leastprivilege.com/2016/09/13/new-in-identityserver4-clients-without-secrets/


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

## OIDC js Client
https://github.com/IdentityModel/oidc-client-js/wiki
https://mderriey.github.io/2016/08/21/openid-connect-and-js-applications-with-oidc-client-js/
https://identityserver.github.io/Documentation/docsv2/overview/jsGettingStarted.html

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

{"issuer":"https://localhost:44399",
"authorization_endpoint":"https://localhost:44399/connect/authorize",
"token_endpoint":"https://localhost:44399/connect/token",
"userinfo_endpoint":"https://localhost:44399/connect/userinfo",
"end_session_endpoint":"https://localhost:44399/connect/endsession",
"check_session_iframe":"https://localhost:44399/connect/checksession",
"revocation_endpoint":"https://localhost:44399/connect/revocation",
"introspection_endpoint":"https://localhost:44399/connect/introspect",
"frontchannel_logout_supported":true,"frontchannel_logout_session_supported":true,
"scopes_supported":["openid","profile","offline_access","api1"],
"claims_supported":["sub","name","family_name","given_name","middle_name","nickname","preferred_username","profile","picture","website","gender","birthdate","zoneinfo","locale","updated_at"],
"response_types_supported":["code","token","id_token","id_token token","code id_token","code token","code id_token token"],
"response_modes_supported":["form_post","query","fragment"],
"grant_types_supported":["authorization_code","client_credentials","refresh_token","implicit","password"],
"subject_types_supported":["public"],
"id_token_signing_alg_values_supported":["RS256"],
"token_endpoint_auth_methods_supported":["client_secret_basic","client_secret_post"],
"code_challenge_methods_supported":["plain","S256"]
}

{"issuer":"https://localhost:44399/two",
"authorization_endpoint":"https://localhost:44399/two/connect/authorize",
"token_endpoint":"https://localhost:44399/two/connect/token",
"userinfo_endpoint":"https://localhost:44399/two/connect/userinfo",
"end_session_endpoint":"https://localhost:44399/two/connect/endsession",
"check_session_iframe":"https://localhost:44399/two/connect/checksession",
"revocation_endpoint":"https://localhost:44399/two/connect/revocation",
"introspection_endpoint":"https://localhost:44399/two/connect/introspect",
"frontchannel_logout_supported":true,
"frontchannel_logout_session_supported":true,
"scopes_supported":["openid","profile","offline_access","api1"],
"claims_supported":["sub","name","family_name","given_name","middle_name","nickname","preferred_username","profile","picture","website","gender","birthdate","zoneinfo","locale","updated_at"],
"response_types_supported":["code","token","id_token","id_token token","code id_token","code token","code id_token token"],
"response_modes_supported":["form_post","query","fragment"],
"grant_types_supported":["authorization_code","client_credentials","refresh_token","implicit","password"],
"subject_types_supported":["public"],
"id_token_signing_alg_values_supported":["RS256"],
"token_endpoint_auth_methods_supported":["client_secret_basic","client_secret_post"],
"code_challenge_methods_supported":["plain","S256"]}


https://localhost:44399/consent?returnUrl=%2Fconnect%2Fauthorize%2Fconsent%3Fclient_id%3Djs%26redirect_uri%3Dhttps%253A%252F%252Flocalhost%253A44399%252Fcallback.html%26response_type%3Did_token%2520token%26scope%3Dopenid%2520profile%2520api1%26state%3D8902207321e94c978e2a71b356b576e4%26nonce%3D2c8de8c871e54a73884ff7c6217620ea

    public async Task<IEndpointResult> ProcessAsync(HttpContext context)
	{
		if (context.Request.Method != "GET")
		{
			_logger.LogWarning("Invalid HTTP method for authorize endpoint.");
			return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
		}

		if (_matcher.IsAuthorizePath(context.Request.Path))
		{
			return await ProcessAuthorizeAsync(context);
		}

		if (_matcher.IsAuthorizeAfterLoginPath(context.Request.Path))
		{
			return await ProcessAuthorizeAfterLoginAsync(context);
		}

		if (_matcher.IsAuthorizeAfterConsentPath(context.Request.Path))
		{
			return await ProcessAuthorizeAfterConsentAsync(context);
		}

		return new StatusCodeResult(HttpStatusCode.NotFound);
	}


 <VersionSuffix>rc20170328</VersionSuffix> 
 <AssemblyName>cloudscribe.IDS4Fork</AssemblyName>
 <PackageId>cloudscribe.IDS4Fork</PackageId>