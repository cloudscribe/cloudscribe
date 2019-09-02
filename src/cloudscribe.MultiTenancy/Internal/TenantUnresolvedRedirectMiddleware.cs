//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;

//namespace cloudscribe.Multitenancy.Internal
//{
//    public class TenantUnresolvedRedirectMiddleware<TTenant>
//    {
//        private readonly string redirectLocation;
//        private readonly bool permanentRedirect;
//        private readonly RequestDelegate next;

//        public TenantUnresolvedRedirectMiddleware(
//            RequestDelegate next,
//            string redirectLocation,
//            bool permanentRedirect)
//        {
//            Ensure.Argument.NotNull(next, nameof(next));
//            Ensure.Argument.NotNull(redirectLocation, nameof(redirectLocation));

//            this.next = next;
//            this.redirectLocation = redirectLocation;
//            this.permanentRedirect = permanentRedirect;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            Ensure.Argument.NotNull(context, nameof(context));

//            var tenantContext = context.GetTenantContext<TTenant>();

//            if (tenantContext == null)
//            {
//                Redirect(context, redirectLocation);
//                return;
//            }

//            // otherwise continue processing
//            await next(context);
//        }
//        private void Redirect(HttpContext context, string redirectLocation)
//        {
//            context.Response.Redirect(redirectLocation);
//            context.Response.StatusCode = permanentRedirect ? StatusCodes.Status301MovedPermanently : StatusCodes.Status302Found;
//        }
//    }
//}
