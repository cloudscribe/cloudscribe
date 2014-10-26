// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2014-10-26
// 

using cloudscribe.AspNet.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.SiteUser;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    public class CloudscribeBaseController : Controller
    {
        public CloudscribeBaseController()
        {
            
        }

        protected ISiteContext Site
        {
            get { return HttpContext.GetOwinContext().Get<ISiteContext>(); }

        }
    }
}
