
# An interesting exception that should probably be handled by application code

## Solution - clear all the cookies

closing the browser session should fix it

I did proof of concept of my diagnosis by writing CommonExceptionHandlerMiddleware

if (ex.Source == "Microsoft.AspNet.Http")
{
	foreach (var c in context.Request.Cookies)
	{
		context.Response.Cookies.Delete(c.Key);

	}
	context.Response.Redirect("/home/about");
	//await _next(context);
	return;

}

## Steps to produce

*  run example.webapp initial run will auto create the db and an admin user
*  everything works, you login as admin
*  next you blow away the database and create a new one run initial setup again in a different web browser
*  now back in the browser where you were already logged in as admin reload the page and this exception is thrown
*  because an auth cookie was sent did not match any expected handler, probably because the securitytoken that was generated for the user no longer exists, I'm just guessing


An unhandled exception occurred while processing the request.

InvalidOperationException: No authentication handler is configured to handle the scheme: Microsoft.AspNet.Identity.Application
Microsoft.AspNet.Http.Authentication.Internal.DefaultAuthenticationManager.<SignOutAsync>d__14.MoveNext()

Stack Query Cookies Headers
InvalidOperationException: No authentication handler is configured to handle the scheme: Microsoft.AspNet.Identity.Application
Microsoft.AspNet.Http.Authentication.Internal.DefaultAuthenticationManager.<SignOutAsync>d__14.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Identity.SignInManager`1.<SignOutAsync>d__28.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Identity.SecurityStampValidator`1.<ValidateAsync>d__0.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Authentication.Cookies.CookieAuthenticationHandler.<HandleAuthenticateAsync>d__10.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
Microsoft.AspNet.Authentication.AuthenticationHandler`1.<InitializeAsync>d__48.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Authentication.AuthenticationMiddleware`1.<Invoke>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Authentication.AuthenticationMiddleware`1.<Invoke>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
Microsoft.AspNet.Authentication.AuthenticationMiddleware`1.<Invoke>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Authentication.AuthenticationMiddleware`1.<Invoke>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
Microsoft.AspNet.Authentication.AuthenticationMiddleware`1.<Invoke>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Authentication.AuthenticationMiddleware`1.<Invoke>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
Microsoft.AspNet.Authentication.AuthenticationMiddleware`1.<Invoke>d__18.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
SaasKit.Multitenancy.Internal.TenantPipelineMiddleware`1.<Invoke>d__5.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
SaasKit.Multitenancy.Internal.TenantResolutionMiddleware`1.<Invoke>d__3.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Session.SessionMiddleware.<Invoke>d__8.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
Microsoft.AspNet.Session.SessionMiddleware.<Invoke>d__8.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.IISPlatformHandler.IISPlatformHandlerMiddleware.<Invoke>d__8.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
System.Runtime.CompilerServices.TaskAwaiter.GetResult()
Microsoft.AspNet.Diagnostics.DeveloperExceptionPageMiddleware.<Invoke>d__7.MoveNext()