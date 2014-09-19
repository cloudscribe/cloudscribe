using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using log4net;

namespace cloudscribe.Core.Web.Filters
{
    public class SiteAuthFilter : AuthorizeAttribute
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteAuthFilter));

        public SiteAuthFilter()
        {

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //IOwinContext owinContext = httpContext.GetOwinContext();

            //bool didrewrite = owinContext.Get<bool>("DidRewriteUrl");
            //if (didrewrite)
            //{
            //    log.Info("did rewrite url");
            //}
            //else
            //{
            //    log.Info("did not rewrite url");
            //}
            return true;
            //return base.AuthorizeCore(httpContext);
        }
    }

    public class SiteContextFilter : FilterAttribute, IActionFilter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteContextFilter));

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //IOwinContext owinContext = filterContext.HttpContext.GetOwinContext();

            //bool didrewrite = owinContext.Get<bool>("DidRewriteUrl");
            //if (didrewrite)
            //{
            //    log.Info("did rewrite url");
            //}
            //else
            //{
            //    log.Info("did not rewrite url");
            //}

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
