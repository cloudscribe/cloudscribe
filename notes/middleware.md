
http://www.talkingdotnet.com/how-to-enable-gzip-compression-in-asp-net-core/

https://carlos.mendible.com/2017/02/12/net-core-health-endpoint-monitoring-middleware/

http://docs.asp.net/en/latest/fundamentals/middleware.html

https://www.jeffogata.com/asp-net-core-middleware/

https://www.billboga.com/posts/using-aspnet-cores-middleware-to-modify-response-body

https://github.com/ClintBailiff/MiddlewareDemo
http://odetocode.com/blogs/scott/archive/2015/10/06/authorization-policies-and-middleware-in-asp-net-5.aspx

app.Use((context, next) =>
            {
                var request = context.Request;
                var host = request.Host;
                if (host.Host.Equals("mysite.com", StringComparison.OrdinalIgnoreCase))
                {
                    HostString newHost;
                    if (host.Port.HasValue)
                    {
                        newHost = new HostString("www.mysite.com", host.Port.Value);
                    }
                    else
                    {
                        newHost = new HostString("www.mysite.com");
                    }
                    context.Response.Redirect(UriHelper.Encode(request.Scheme, newHost,
                        request.PathBase, request.Path, request.QueryString));
                    return Task.FromResult(0);
                }
                return next();
            });

https://github.com/OrchardCMS/Orchard2/blob/5342a792dbac9fb70a7d76c2e17bfee7b9c0ba2c/src/Orchard.Hosting.Web/OrchardShell.cs#L36-L63



owin middleware

http://coding.abel.nu/2014/06/asp-net-identity-and-owin-overview/

http://coding.abel.nu/2014/06/understanding-the-owin-external-authentication-pipeline/

http://coding.abel.nu/2014/06/writing-an-owin-authentication-middleware/

http://coding.abel.nu/2014/11/using-owin-external-login-without-asp-net-identity/

http://benfoster.io/blog/how-to-write-owin-middleware-in-5-different-steps

http://www.asp.net/aspnet/overview/owin-and-katana/owin-middleware-in-the-iis-integrated-pipeline

http://chris.59north.com/post/2014/05/06/Understanding-OWIN-%E2%80%93-hosting-and-middleware.aspx

http://www.codeproject.com/Articles/864725/ASP-NET-Understanding-OWIN-Katana-and-the-Middlewa

http://stackoverflow.com/questions/17509768/changing-the-response-object-from-owin-middleware

http://stackoverflow.com/questions/26214113/how-can-i-safely-intercept-the-response-stream-in-a-custom-owin-middleware

http://dotnetcodr.com/2014/04/21/owin-and-katana-part-3-more-complex-middleware/

maybe should make my own APM solution based on applicaiton insights api
https://github.com/Microsoft/ApplicationInsights-aspnet5/blob/master/src/Microsoft.ApplicationInsights.AspNet/RequestTrackingMiddleware.cs

https://github.com/Microsoft/ApplicationInsights-aspnet5/blob/master/src/Microsoft.ApplicationInsights.AspNet/ExceptionTrackingMiddleware.cs

http://azure.microsoft.com/blog/2015/06/25/announcing-the-1-0-0-release-of-application-insights-windows-sdks/

https://azure.microsoft.com/en-us/pricing/details/application-insights/

https://azure.microsoft.com/en-us/documentation/articles/app-insights-start-monitoring-app-health-usage/



http://stackoverflow.com/questions/19096723/login-request-validation-token-issue


InvalidOperationException: The antiforgery cookie token and form field token do not match.

An unhandled exception occurred while processing the request.

InvalidOperationException: The provided antiforgery token was meant for a different claims-based user than the current user.
Microsoft.AspNet.Antiforgery.DefaultAntiforgeryTokenGenerator.ValidateTokens(HttpContext httpContext, AntiforgeryToken sessionToken, AntiforgeryToken fieldToken)

I use gulp-gzip package for make .gz files.
And small middleware for handle requests for static files:
https://github.com/aspnet/Home/issues/1584#issuecomment-230340750

`public class CompressedStaticFileMiddleware
{
private IHostingEnvironment _hostingEnv;
private StaticFileMiddleware _base;
public CompressedStaticFileMiddleware(RequestDelegate next, IHostingEnvironment hostingEnv, IOptions options, ILoggerFactory loggerFactory) 
{
_hostingEnv = hostingEnv;
var contentTypeProvider = options.Value.ContentTypeProvider ?? new FileExtensionContentTypeProvider();

        _base = new StaticFileMiddleware(next, hostingEnv, new StaticFileOptions()
        {
            ContentTypeProvider = contentTypeProvider,
            FileProvider = options.Value.FileProvider ?? hostingEnv.WebRootFileProvider,
            OnPrepareResponse = ctx =>
            {
                const string ext = ".gz";
                if (ctx.File.Name.EndsWith(ext))
                {
                    string contentType = null;
                    if(contentTypeProvider.TryGetContentType(ctx.File.PhysicalPath.Remove(ctx.File.PhysicalPath.Length - ext.Length, ext.Length), out contentType))
                        ctx.Context.Response.ContentType = contentType;
                    ctx.Context.Response.Headers.Add("Content-Encoding", new[] { "gzip" });
                }
            }
        }, loggerFactory);
    }

    public Task Invoke(HttpContext context)
    {
        if(context.Request.Path.HasValue)
        {
            string acceptEncoding = context.Request.Headers["Accept-Encoding"];
            if (
                !string.IsNullOrEmpty(acceptEncoding) && 
                (acceptEncoding.Contains("gzip") && 
                System.IO.File.Exists(_hostingEnv.MapPath(context.Request.Path.Value.StartsWith("/") ? context.Request.Path.Value.Remove(0, 1) : context.Request.Path.Value) + ".gz"))
            )
            {
                context.Request.Path = new PathString(context.Request.Path.Value + ".gz");
                return _base.Invoke(context);
            }
        }
        return _base.Invoke(context);
    }
}`

public class CompressedStaticFileMiddleware
    {
        private IHostingEnvironment _hostingEnv;
        private StaticFileMiddleware _base;
        public CompressedStaticFileMiddleware(RequestDelegate next, IHostingEnvironment hostingEnv, IOptions<StaticFileOptions> options, ILoggerFactory loggerFactory)
        {
            _hostingEnv = hostingEnv;
            var contentTypeProvider = options.Value.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            options.Value.ContentTypeProvider = contentTypeProvider;
            options.Value.FileProvider = options.Value.FileProvider ?? hostingEnv.WebRootFileProvider;
            options.Value.OnPrepareResponse = ctx =>
            {
                const string ext = ".gz";
                if (ctx.File.Name.EndsWith(ext))
                {
                    string contentType = null;
                    if (contentTypeProvider.TryGetContentType(ctx.File.PhysicalPath.Remove(ctx.File.PhysicalPath.Length - ext.Length, ext.Length), out contentType))
                        ctx.Context.Response.ContentType = contentType;
                    ctx.Context.Response.Headers.Add("Content-Encoding", new[] { "gzip" });
                }
            };

            _base = new StaticFileMiddleware(next, hostingEnv, options, loggerFactory);
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue)
            {
                string acceptEncoding = context.Request.Headers["Accept-Encoding"];
                if (
                    !string.IsNullOrEmpty(acceptEncoding) &&
                    (
                        acceptEncoding.Contains("gzip") &&
                        System.IO.File.Exists(
                            System.IO.Path.Combine(
                                _hostingEnv.WebRootPath, context.Request.Path.Value.StartsWith("/") 
                                ? context.Request.Path.Value.Remove(0, 1) 
                                : context.Request.Path.Value, ".gz"
                            )
                        )
                    )
                )
                {
                    context.Request.Path = new PathString(context.Request.Path.Value + ".gz");
                    return _base.Invoke(context);
                }
            }
            return _base.Invoke(context);
        }
    }