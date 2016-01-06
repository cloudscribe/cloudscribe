// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2016-01-06
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Setup;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;


namespace cloudscribe.Setup.Web
{
    public class SetupController : Controller
    {    
        public SetupController(
            IApplicationEnvironment appEnv,
            ILogger<SetupController> logger,
            IOptions<SetupOptions> setupOptionsAccessor,
            SetupManager setupManager,
            IAuthorizationService authorizationService,
            IEnumerable<ISetupTask> setupSteps = null
        )
        {
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (setupOptionsAccessor == null) { throw new ArgumentNullException(nameof(setupOptionsAccessor)); }
            if (setupManager == null) { throw new ArgumentNullException(nameof(setupManager)); }
            if (authorizationService == null) { throw new ArgumentNullException(nameof(authorizationService)); }

            log = logger;
            appBasePath = appEnv.ApplicationBasePath;
            this.setupManager = setupManager;
            setupOptions = setupOptionsAccessor.Value;
            this.authorizationService = authorizationService;
            if(setupSteps != null)
            {
                this.setupSteps = setupSteps;
            }

        }

        IEnumerable<ISetupTask> setupSteps = null;
        private IAuthorizationService authorizationService;
        private SetupOptions setupOptions;
        private SetupManager setupManager;
        private string appBasePath;
        private ILogger log;
        // private int scriptTimeout;
        private DateTime startTime = DateTime.UtcNow;
        private static object Lock = new object();



        public async Task<IActionResult> Index()
        { 
            bool isAllowed = await authorizationService.AuthorizeAsync(User, "SetupSystemPolicy");
            
            if (!setupOptions.AllowAnonymous && !isAllowed)
            {
                log.LogInformation("returning 404 because allowAnonymous is false and user is either not authenticated or not allowed by policy");
                Response.StatusCode = 404;
                return new EmptyResult();
            }


            //scriptTimeout = Server.ScriptTimeout;
            //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            //Response.BufferOutput = true;
            // Server.ScriptTimeout = int.MaxValue;

            startTime = DateTime.UtcNow;
            
            await WritePageHeader(HttpContext.Response);

            if (!setupOptions.AllowAnonymous && isAllowed)
            {
                await WritePageContent(Response,
                    "RunningSetupForAllowedUser" //SetupResources.RunningSetupForAdminUser
                    );

            }

            // SetupManager and ISetupTasks will use this function to write to the response
            Func<string, bool, Task> outputWriter =  async (string message, bool showTime) =>
            {
                await WritePageContent(Response, message, showTime);
                
            };

            // this locking strategy did not work as expected perhaps because we are doing things async
            //int lockTimeoutMilliseconds = config.GetOrDefault("AppSetings:SetupLockTimeoutMilliseconds", 60000); // 1 minute

            //if(!Monitor.TryEnter(Lock, lockTimeoutMilliseconds))
            //{
            //    //throw new Exception("setup is already locked and runnning")

            //    await WritePageContent(Response,
            //            "SetupAlreadyInProgress" //SetupResources.SetupAlreadyInProgress
            //            );
            //}
            //else
            //{
            bool keepGoing;
            try
            {
                await setupManager.ProbeSystem(outputWriter);
                // the setup system must be bootstrapped first 
                // to make sure mp_SchemaVersion table exists
                // which is used to keep track of script versions that have already been run
                bool needSetupUpdate = setupManager.NeedsUpgrade("cloudscribe-setup");
                if(needSetupUpdate)
                {
                    keepGoing = await setupManager.SetupCloudscribeSetup(outputWriter);
                }
                else
                {
                    keepGoing = true;
                }
                
               
                if(keepGoing)
                {
                    // this runs the scripts for other apps including cloudscribe-core, 
                    // cloudscribe-logging, and any custom apps that use the setup system
                    keepGoing = await setupManager.SetupOtherApplications(outputWriter);
                }
                
                if(setupSteps != null)
                { 
                    foreach(ISetupTask step in setupSteps)
                    {
                        try
                        {
                            await step.DoSetupStep(
                                outputWriter, 
                                setupManager.NeedsUpgrade,
                                setupManager.GetSchemaVersion,
                                setupManager.GetCodeVersion
                                );
                        }
                        catch(Exception ex)
                        {
                            await WriteException(Response, ex);
                        }

                        
                    }
                }


            }
            catch(Exception ex)
            {
                
                await WriteException(Response, ex);
                
            }
            finally
            {
                //ClearSetupLock();
                //Monitor.Exit(Lock);
            }

            if(setupOptions.ShowSchemaListOnSetupPage)
            {
                await WriteInstalledSchemaSummary(Response);
            }
            


            await WritePageFooter(Response);
            
            return new EmptyResult();
        }

        public async Task<IActionResult> Status()
        {
            bool isAllowed = await authorizationService.AuthorizeAsync(User, "SetupSystemPolicy");

            if (!isAllowed)
            {
                log.LogInformation("returning 404 because user is either not authenticated or not allowed by policy");
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            await WritePageHeader(HttpContext.Response);

            if(setupOptions.ProbeSystemOnStatusPage)
            {
                // SetupManager and ISetupTasks will use this function to write to the response
                Func<string, bool, Task> outputWriter = async (string message, bool showTime) =>
                {
                    await WritePageContent(Response, message, showTime);

                };
                await setupManager.ProbeSystem(outputWriter);

            }

            await WriteInstalledSchemaSummary(Response);

            await WritePageFooter(Response);

            return new EmptyResult();
        }

        private async Task WriteInstalledSchemaSummary(HttpResponse response)
        {
            //await WritePageContent(response, message, false);
            List<VersionItem> currentSchemas = await setupManager.GetInstalledSchemaList();
            await response.WriteAsync("<h2>Current Schema Versions</h2>");
            await response.WriteAsync("<ul>");

            string formatString = "<li>{0} - {1}</li>";
            foreach(VersionItem item in currentSchemas)
            {
                string itemMarkup = string.Format(formatString, item.Name, item.CurrentVersion.ToString());
                await response.WriteAsync(itemMarkup);
            }

            await response.WriteAsync("</ul>");

        }

        private async Task WriteException(HttpResponse response, Exception ex)
        {
            if(!setupOptions.ShowErrors)
            {
                await WritePageContent(Response, "an exception occurred but configuration settings don't allow showing the details here.");
                return;
            }
            await WritePageContent(response, "<hr />" + ex.Message + " -- " + ex.StackTrace, false);
        }

        private async Task WritePageContent(HttpResponse response, string message)
        {
            await WritePageContent(response, message, false);
        }

        private async Task WritePageContent(HttpResponse response, string message, bool showTime)
        {
            if (showTime)
            {
                await response.WriteAsync(
                    string.Format("{0} - {1}",
                    message,
                    DateTime.UtcNow.Subtract(startTime)));
            }
            else
            {
                await response.WriteAsync(message);
            }
            await response.WriteAsync("<br/>");
       
        }

        private async Task WritePageHeader(HttpResponse response)
        {
            string setupTemplatePath = setupOptions.SetupHeaderConfigPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                setupTemplatePath = setupOptions.SetupHeaderConfigPathRtl.Replace("/", Path.DirectorySeparatorChar.ToString());
            }

            string fsPath = appBasePath + setupTemplatePath;

            if (System.IO.File.Exists(fsPath))
            {
                string sHtml = string.Empty;
                using (StreamReader oStreamReader = System.IO.File.OpenText(fsPath))
                {
                    sHtml = oStreamReader.ReadToEnd();
                }
                await response.WriteAsync(sHtml);
            }

        }

        private async Task WritePageFooter(HttpResponse response)
        {
            await response.WriteAsync("</body>");
            await response.WriteAsync("</html>");
        }

    }
}
