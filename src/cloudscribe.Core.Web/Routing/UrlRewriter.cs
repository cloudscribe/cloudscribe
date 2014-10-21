using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using log4net;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web;



namespace cloudscribe.Core.Web
{
    public class UrlRewriter : IHttpModule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UrlRewriter));
        private static bool debugLog = log.IsDebugEnabled;


        public void Init(HttpApplication app)
        {
            app.BeginRequest += app_BeginRequest;

        }

        void app_BeginRequest(object sender, EventArgs e)
        {
            if (!AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                // the purpose of this module is to rewrite urls for folder based multi sites
                // so they can use the same routes and keep routing more simple
                // if not using folders just invoke the next middleware
                return;
            }

            if(!AppSettings.UseUrlRewriteForFolderSites)
            {
                return;
            }

            HttpApplication app = (HttpApplication)sender;
            IOwinContext owinContext = app.Context.GetOwinContext();
            StandardKernel ninjectKernel = owinContext.Get<StandardKernel>();
            ISiteRepository siteRepo = ninjectKernel.Get<ISiteRepository>();

            string virtualFolderName = SiteContext.GetFirstFolderSegment(app.Request.Url.ToString());
            if (virtualFolderName.Length > 1)
            {
                if (siteRepo.FolderExists(virtualFolderName))
                {
                    string requestPath = app.Request.Path;
                    if (requestPath.Contains(virtualFolderName + "/"))
                    {
                        string pathToUse = requestPath.Remove(0, virtualFolderName.Length + 1);
                        bool setClientFilePath = false;
                        string pathInfo = string.Empty;
                        app.Context.Items["DidRewriteUrl"] = true;
                        
                        // can't do this because this module runs before owin pipeline so owincontext does not exist yet
                        //app.Context.GetOwinContext().Set<bool>("DidRewriteUrl", true);
                        //app.Context.GetOwinContext().Set<string>("AppRequestPath", app.Request.Path);


                        //this works, but the owin Request then does not know about the rawurl in the browser
                        // http://cloudscribe/mojo4/home for sitefolder named mojo4 routes correctly to http://cloudscribe/home
                        // after the rewrite and that is all that the owin request sees
                        // HttpContext.Current.Request.RawUrl does have the original url that corresponds to the web browser url

                        app.Context.RewritePath(
                            pathToUse,
                            pathInfo,
                            app.Request.QueryString.ToString(),
                            setClientFilePath);

                    }

                }
            }

        }

        public void Dispose() { }
    }

    public class UrlRewriterMiddleware : OwinMiddleware
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UrlRewriterMiddleware));
        private static bool debugLog = log.IsDebugEnabled;
        private ISiteRepository siteRepo;

        public UrlRewriterMiddleware(OwinMiddleware next, ISiteRepository siteRepository)
            : base(next)
        {
            siteRepo = siteRepository;
        }

        public async override Task Invoke(IOwinContext context)
        {
            if (!AppSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                // the purpose of this module is to rewrite urls for folder based multi sites
                // so they can use the same routes and keep routing more simple
                // if not using folders just invoke the next middleware
                await Next.Invoke(context);
            }

            

            string virtualFolderName = SiteContext.GetFirstFolderSegment(context.Request.Uri.ToString());
            if (virtualFolderName.Length > 1)
            {
                if(siteRepo.FolderExists(virtualFolderName))
                {
                    string requestPath = context.Request.Path.Value;
                    if(requestPath.Contains(virtualFolderName + "/"))
                    {
                        string pathToUse = requestPath.Remove(0, virtualFolderName.Length + 1);
                        context.Request.Path = new PathString(pathToUse);
                    
                      
                    }

                }
            }

            //Console.WriteLine("Begin Request");
            await Next.Invoke(context);
            //Console.WriteLine("End Request");
        }

        
       
    }
}
