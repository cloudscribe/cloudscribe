

//using System;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.Net.Http.Headers;

////https://github.com/aspnet/Diagnostics/blob/dev/src/Microsoft.AspNetCore.Diagnostics/ExceptionHandler/ExceptionHandlerMiddleware.cs

//namespace cloudscribe.Core.Web.Middleware
//{

    

//    public class ExceptionHandlerOptions
//    {
//        public PathString ExceptionHandlingPath { get; set; }

//        public RequestDelegate ExceptionHandler { get; set; }
//    }

//    public class CommonExceptionHandlerMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ExceptionHandlerOptions _options;
//        private readonly ILogger _logger;
//        private readonly Func<object, Task> _clearCacheHeadersDelegate;
//        private readonly DiagnosticSource _diagnosticSource;

//        public CommonExceptionHandlerMiddleware(
//            RequestDelegate next,
//            ILoggerFactory loggerFactory,
//            DiagnosticSource diagnosticSource,
//            IOptions<ExceptionHandlerOptions> options = null
//            )
//        {
//            _next = next;
//            if(options == null)
//            {
//                _options = new ExceptionHandlerOptions();
//            }
//            else
//            {
//                _options = options.Value;
//            }
            
//            _logger = loggerFactory.CreateLogger<CommonExceptionHandlerMiddleware>();
//            if (_options.ExceptionHandler == null)
//            {
//                _options.ExceptionHandler = _next;
//            }
//            _clearCacheHeadersDelegate = ClearCacheHeaders;
//            _diagnosticSource = diagnosticSource;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Processing Exception: ", ex);
//                // We can't do anything if the response has already started, just abort.
//                if (context.Response.HasStarted)
//                {
//                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
//                    throw;
//                }

//                PathString originalPath = context.Request.Path;
//                if (_options.ExceptionHandlingPath.HasValue)
//                {
//                    context.Request.Path = _options.ExceptionHandlingPath;
//                }
//                try
//                {
//                    context.Response.Clear();

//                    //if (ex.Source == "Microsoft.AspNet.Http")
//                    //{
//                    //    foreach (var c in context.Request.Cookies)
//                    //    {
//                    //        context.Response.Cookies.Delete(c.Key);

//                    //    }
//                    //    context.Response.Redirect("/home/about");
//                    //    //await _next(context);
//                    //    return;

//                    //}

//                    var exceptionHandlerFeature = new ExceptionHandlerFeature()
//                    {
//                        Error = ex,
//                    };
//                    context.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);
//                    context.Response.StatusCode = 500;
//                    context.Response.OnStarting(_clearCacheHeadersDelegate, context.Response);

                    

//                    await _options.ExceptionHandler(context);

//                    if (_diagnosticSource.IsEnabled("cloudscribe.Core.Web.Middleware.CommonExceptionHandlerMiddleware.HandledException"))
//                    {
//                        _diagnosticSource.Write("cloudscribe.Core.Web.Middleware.CommonExceptionHandlerMiddleware.HandledException", 
//                            new { httpContext = context, exception = ex });
//                    }

//                    // TODO: Optional re-throw? We'll re-throw the original exception by default if the error handler throws.
//                    return;
//                }
//                catch (Exception ex2)
//                {
//                    // Suppress secondary exceptions, re-throw the original.
//                    _logger.LogError( "An exception was thrown attempting to execute the error handler.",ex2);
//                }
//                finally
//                {
//                    context.Request.Path = originalPath;
//                }
//                throw; // Re-throw the original if we couldn't handle it
//            }
//        }

//        private Task ClearCacheHeaders(object state)
//        {
//            var response = (HttpResponse)state;
//            response.Headers[HeaderNames.CacheControl] = "no-cache";
//            response.Headers[HeaderNames.Pragma] = "no-cache";
//            response.Headers[HeaderNames.Expires] = "-1";
//            response.Headers.Remove(HeaderNames.ETag);
//            return Task.FromResult(0);
//        }
//    }
//}
