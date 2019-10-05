//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Extensions;
//using System;
//using System.Threading.Tasks;

//namespace cloudscribe.Multitenancy.Internal
//{
//    public class PrimaryHostnameRedirectMiddleware<TTenant>
//    {
//        private readonly Func<TTenant, string> primaryHostnameAccessor;
//        private readonly bool permanentRedirect;
//        private readonly RequestDelegate next;

//        public PrimaryHostnameRedirectMiddleware(
//            RequestDelegate next,
//            Func<TTenant, string> primaryHostnameAccessor,
//            bool permanentRedirect)
//        {
//            Ensure.Argument.NotNull(next, nameof(next));
//            Ensure.Argument.NotNull(primaryHostnameAccessor, nameof(primaryHostnameAccessor));

//            this.next = next;
//            this.primaryHostnameAccessor = primaryHostnameAccessor;
//            this.permanentRedirect = permanentRedirect;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            Ensure.Argument.NotNull(context, nameof(context));

//            var tenantContext = context.GetTenantContext<TTenant>();

//            if (tenantContext != null)
//            {
//                var primaryHostname = primaryHostnameAccessor(tenantContext.Tenant);

//                if (!string.IsNullOrWhiteSpace(primaryHostname))
//                {
//                    if (!context.Request.Host.Value.Equals(primaryHostname, StringComparison.OrdinalIgnoreCase))
//                    {
//                        Redirect(context, primaryHostname);
//                        return;
//                    }
//                }
//            }

//            // otherwise continue processing
//            await next(context);
//        }
//        private void Redirect(HttpContext context, string primaryHostname)
//        {
//            var builder = new UriBuilder(context.Request.GetEncodedUrl());
//            builder.Host = primaryHostname;

//            context.Response.Redirect(builder.Uri.AbsoluteUri);
//            context.Response.StatusCode = permanentRedirect ? StatusCodes.Status301MovedPermanently : StatusCodes.Status302Found;
//        }
//    }
//}
