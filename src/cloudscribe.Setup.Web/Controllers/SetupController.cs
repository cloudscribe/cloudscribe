// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2016-01-05
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Setup;
//using cloudscribe.Resources;
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

// this needs redesign/refactoring
// but it is working and doind what it needs to do 

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
            //if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (setupManager == null) { throw new ArgumentNullException(nameof(setupManager)); }

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
        
        private bool canAccessDatabase = false;
        private bool setupSchemaHasBeenCreated = false;
        private bool canAlterSchema = false;
        private bool showConnectionError = false;
        private bool needSetupSchemaUpgrade = false;
        private Version cloudscribeCoreCodeVersion = new Version(0,0);
        private Version cloudscribeCoreDbSchemaVersion = new Version(0,0);
        // private int scriptTimeout;
        private DateTime startTime;
        private bool exceptionsRendered = false;


        private static object Lock = new object();



        public async Task<IActionResult> Index()
        {
            //scriptTimeout = Server.ScriptTimeout;
            //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            
            bool isAllowed = await authorizationService.AuthorizeAsync(User, "SetupSystemPolicy");
            
            if (!setupOptions.AllowAnonymous && !isAllowed)
            {
                log.LogInformation("returning 404 because allowAnonymous is false and user is either not authenticated or not allowed by policy");
                Response.StatusCode = 404;
                return new EmptyResult();
            }



            //Response.BufferOutput = true;
            // Server.ScriptTimeout = int.MaxValue;
            bool result;
            startTime = DateTime.UtcNow;
            
            await WritePageHeader(HttpContext.Response);

            if (!setupOptions.AllowAnonymous && isAllowed)
            {
                await WritePageContent(Response,
                    "RunningSetupForAllowedUser" //SetupResources.RunningSetupForAdminUser
                    );

            }

            // ISetupTasks will use this function to write to the response
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
            try
            {
                await ProbeSystem(Response);
                // the setup system must be bootstrapped first 
                // to make sure mp_SchemaVersion table exists
                // which is used to keep track of script versions that have already been run
                result = await SetupCloudscribeSetup(Response);
               
                if(result)
                {
                    // this runs the scripts for other apps including cloudscribe-core, 
                    // cloudscribe -logging, and any custom apps that use the setup system
                    result = await SetupOtherApplications(Response);
                }

                // this part could be potentially refactored
                // and abstracted to run steps that could be plugged in
                // the goal would be to decouple setup from cloudscribe-core
                // while still providing a way for cloudscribe core data to be ensured
                //result = await EnsureSiteExists(Response);


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

            try
            {
                await ShowSetupStatus(Response);
            }
            catch(Exception ex)
            {
                await WriteException(Response, ex);
            }
            
            await WritePageFooter(Response);
            
            return new EmptyResult();
        }
        
        

        private async Task<bool> SetupCloudscribeSetup(HttpResponse response)
        {

            bool result = true;

            if (!setupSchemaHasBeenCreated)
            {
                if (canAlterSchema)
                {

                    setupSchemaHasBeenCreated = await CreateInitialSchema(response, "cloudscribe-setup");

                    if (setupSchemaHasBeenCreated)
                    {
                        //recheck
                        needSetupSchemaUpgrade = setupManager.NeedsUpgrade("cloudscribe-setup");
                    }

                }
            }

            if (
                (setupSchemaHasBeenCreated)
                && (needSetupSchemaUpgrade)
                && (canAlterSchema)
                )
            {
                needSetupSchemaUpgrade = await UpgradeSchema(response, "cloudscribe-setup");
            }

           
            return result;
        }

        private async Task<bool> SetupOtherApplications(HttpResponse response)
        {
            bool result = true;
            
            string pathToApplicationsFolder = setupManager.GetPathToApplicationsFolder();

            if (!Directory.Exists(pathToApplicationsFolder))
            {
                await WritePageContent(response,
                pathToApplicationsFolder
                + " " 
                + "ScriptFolderNotFoundAddendum", // SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;
            }

            DirectoryInfo appRootFolder
                = new DirectoryInfo(pathToApplicationsFolder);

            DirectoryInfo[] appFolders = appRootFolder.GetDirectories();

            foreach (DirectoryInfo appFolder in appFolders)
            {
                // skip cloudscribe-setup since we set that up first
                if (
                    (!string.Equals(appFolder.Name, "cloudscribe-setup", StringComparison.CurrentCultureIgnoreCase))
                    && (appFolder.Name.ToLower() != ".svn")
                    && (appFolder.Name.ToLower() != "_svn")
                    )
                {
                    await CreateInitialSchema(response, appFolder.Name);
                    await UpgradeSchema(response, appFolder.Name);

                }

            }
            
            return result;

        }

        

        private async Task<bool> CreateInitialSchema(HttpResponse response, string applicationName)
        {
            Guid appID = setupManager.GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = setupManager.GetSchemaVersion(appID);
            Version zeroVersion = new Version(0, 0, 0, 0);

            if (currentSchemaVersion > zeroVersion) { return true; } //already installed only run upgrade scripts

            Version versionToStopAt = null; // null because we don't stop on install we start with the highest version in the folder which is also the last one

            string pathToScriptFolder = setupManager.GetPathToInstallScriptFolder(applicationName);
            
            if (!Directory.Exists(pathToScriptFolder))
            {
                await WritePageContent(response,
                pathToScriptFolder + " " 
                + "ScriptFolderNotFoundAddendum", //SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;

            }

            bool result = await RunSetupScript(
                response,
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

            return result;

        }

        private async Task<bool> UpgradeSchema(HttpResponse response, string applicationName)
        {

            Version versionToStopAt = setupManager.GetCodeVersion(applicationName);
            Guid appID = setupManager.GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = setupManager.GetSchemaVersion(appID);

            if(versionToStopAt != null)
            {
                if(versionToStopAt <= currentSchemaVersion) { return false; }

            }

            string pathToScriptFolder = setupManager.GetPathToUpgradeScriptFolder(applicationName);


            if (!Directory.Exists(pathToScriptFolder))
            {
                //await WritePageContent(response, pathToScriptFolder + " not found");
                return false;

            }

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.sql");


            if (scriptFiles.Length == 0)
            {
                string warning = string.Format(
                    "NoUpgradeScriptsFound", //SetupResources.NoUpgradeScriptsFound,
                    applicationName);

                return false;

            }

            bool result = await RunUpgradeScripts(
                response,
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

            return result;

        }

        private async Task<bool> RunSetupScript(
            HttpResponse response,
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                await WritePageContent(response,
                pathToScriptFolder + " " 
                + "ScriptFolderNotFoundAddendum", // SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;
            }

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.sql");
            Array.Sort(scriptFiles, SetupManager.CompareFileNames);


            if (scriptFiles.Length == 0)
            {
                await WritePageContent(response,
                "NoScriptsFilesFound" //SetupResources.NoScriptsFilesFoundMessage
                + " " + pathToScriptFolder,
                false);

                return false;

            }

            // We only want to run the highest version script from the install folder
            // normally there is only 1 script in this folder, but if someone upgrades and then starts with a clean db
            // there can be more than one script because of the previous installs so we need to make sure we only run the highest version found
            // since we sorted it the highest version is the last item in the array
            FileInfo scriptFile = scriptFiles[(scriptFiles.Length - 1)];

            Version currentSchemaVersion = setupManager.GetSchemaVersion(applicationId);
            Version scriptVersion = setupManager.ParseVersionFromFileName(scriptFile.Name);

            if (
                (scriptVersion != null)
                && (scriptVersion > currentSchemaVersion)
                && (versionToStopAt == null || (scriptVersion <= versionToStopAt))
                )
            {
                result = await ProcessScript(
                    response,
                    scriptFile,
                    applicationId,
                    applicationName,
                    currentSchemaVersion);
            }

            return result;

        }
        
        private async Task<bool> RunUpgradeScripts(
            HttpResponse response,
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                await WritePageContent(response,
                pathToScriptFolder + " " 
                + "ScriptFolderNotFound", // SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;
            }

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.sql");
            Array.Sort(scriptFiles, SetupManager.CompareFileNames);

            if (scriptFiles.Length == 0)
            {
                //WritePageContent(
                //SetupResource.NoScriptsFilesFoundMessage 
                //+ " " + pathToScriptFolder,
                //false);

                return false;

            }

            Version currentSchemaVersion
                = setupManager.GetSchemaVersion(applicationId);

            foreach (FileInfo scriptFile in scriptFiles)
            {
                Version scriptVersion
                    = setupManager.ParseVersionFromFileName(scriptFile.Name);

                if (
                    (scriptVersion != null)
                    && (scriptVersion > currentSchemaVersion)
                    && (versionToStopAt == null || (scriptVersion <= versionToStopAt))
                    )
                {
                    result = await ProcessScript(
                        response,
                        scriptFile,
                        applicationId,
                        applicationName,
                        currentSchemaVersion);

                    if(!result) { return false; }
                }

            }

            return result;

        }

        private async Task<bool> ProcessScript(
            HttpResponse response,
            FileInfo scriptFile,
            Guid applicationId,
            string applicationName,
            Version currentSchemaVersion)
        {
            string message = string.Format(
                        "RunningScript {0} {1}", //SetupResources.RunningScriptMessage,
                        applicationName,
                        scriptFile.Name.Replace(".sql", string.Empty));

            await WritePageContent(response,
                message,
                true);

            string overrideConnectionString = setupManager.GetOverrideConnectionString(applicationName);

            string errorMessage
                = setupManager.RunScript(
                    applicationId,
                    scriptFile,
                    overrideConnectionString);

            if (errorMessage.Length > 0)
            {
                await WritePageContent(response, errorMessage, true);
                return false;

            }

            Version newVersion
                = setupManager.ParseVersionFromFileName(scriptFile.Name);

            if (
                (applicationName != null)
                && (newVersion != null)
                )
            {
                setupManager.UpdateSchemaVersion(
                    applicationId,
                    applicationName,
                    newVersion.Major,
                    newVersion.Minor,
                    newVersion.Build,
                    newVersion.Revision);
                
                    currentSchemaVersion = newVersion;
                    return true;
               
            }

            return false;
        }

        


        private async Task ProbeSystem(HttpResponse response)
        {
            await WritePageContent(response,
                "ProbingSystem", //SetupResources.ProbingSystemMessage,
                false);
            
            setupManager.EnsureDatabaseIfPossible();
            
            canAccessDatabase = setupManager.CanAccessDatabase();
            
            if (canAccessDatabase)
            {
                await WritePageContent(response,
                    setupManager.DBPlatform
                    + " " + "DatabaseConnectionOK", // SetupResources.DatabaseConnectionOKMessage,
                    false);
            }
            else
            {
                string dbError = string.Format(
                    "FailedToConnectToDatabase", //SetupResources.FailedToConnectToDatabase,
                    setupManager.DBPlatform);

                await WritePageContent(response, "<div>" + dbError + "</div>", false);

                showConnectionError = setupOptions.ShowConnectionError;
                
                if (showConnectionError)
                {
                    await WritePageContent(response,
                        "<div>" + setupManager.GetDbConnectionError()
                        + "</div>",
                        false);
                }
            }

            if (canAccessDatabase)
            {
                canAlterSchema = setupManager.CanAlterSchema();

                if (canAlterSchema)
                {
                    await WritePageContent(response,
                        "DatabaseCanAlterSchema", //SetupResources.DatabaseCanAlterSchemaMessage,
                        false);
                }
                else
                {

                    if (setupOptions.TryAnywayIfFailedAlterSchemaTest)
                    {
                        canAlterSchema = true;
                    }
                    else
                    {
                        await WritePageContent(response,
                       "<div>" + "CantAlterSchemaWarning" // SetupResources.CantAlterSchemaWarning
                       + "</div>",
                       false);
                    }
                }

                setupSchemaHasBeenCreated = setupManager.SchemaTableExists();

                if (setupSchemaHasBeenCreated)
                {
                    await WritePageContent(response,
                        "SetupSystemSchemaAlreadyExists", //SetupResources.DatabaseSchemaAlreadyExistsMessage,
                        false);

                    needSetupSchemaUpgrade = setupManager.NeedsUpgrade("cloudscribe-setup");

                    if (needSetupSchemaUpgrade)
                    {
                        await WritePageContent(response,
                            "SetupSystemSchemaNeedsUpgrade", //SetupResources.DatabaseSchemaNeedsUpgradeMessage,
                            false);
                    }
                    else
                    {
                        await WritePageContent(response,
                            "SetupSystemSchemaUpToDate", //SetupResources.DatabaseSchemaUpToDateMessage,
                            false);
                    }

                    

                }
                else
                {
                    await WritePageContent(response,
                        "SetupSchemaNotCreatedYet", //SetupResources.DatabaseSchemaNotCreatedYetMessage,
                        false);
                }

            }

           
        }

        

        private bool CoreSystemIsReady()
        {
            if (!canAccessDatabase) return false;

            if (!setupManager.SchemaTableExists()) return false;

            if (setupManager.NeedsUpgrade("cloudscribe-core")) { return false; }

            return true;
        }

        

        private async Task ShowSetupStatus(HttpResponse response)
        {

            StringBuilder statusMessage = new StringBuilder();

            if(CoreSystemIsReady())
            {
                if(exceptionsRendered)
                {
                    statusMessage.Append("<hr /><div>"
                        + "SetupSchemaIsUpToDateButExceptionsOccured" // SetupResources.SetupSuccessMessage 
                        + "</div>");
                }
                else
                {
                    statusMessage.Append("<hr /><div>"
                        + "SetupSuccess" // SetupResources.SetupSuccessMessage 
                        + "</div>");
                }
                
            }
            else
            {
                statusMessage.Append("<hr /><div>"
                + "UnexpectedSetupResult" // SetupResources.SetupSuccessMessage 
                + "</div>");
            }
            

            statusMessage.Append("<a href='" + "/" // TODO: was using Page.ResolveUrl("~/")
                + "' title='" + "Home" //SetupResources.HomeLink 
                + "'>"
                + "Home" // SetupResources.HomeLink 
                + "</a>");

            statusMessage.Append("<br /><br />");

            statusMessage.Append("<div class='settingrow'>");
            statusMessage.Append("<span class='settinglabel'>");
            //successMessage.Append(SetupResources.DatabasePlatform);
            statusMessage.Append("DatabasePlatform");
            statusMessage.Append("</span>");
            statusMessage.Append(setupManager.DBPlatform);
            statusMessage.Append("</div>");

            if (setupSchemaHasBeenCreated)
            { 
                cloudscribeCoreDbSchemaVersion = setupManager.GetCloudscribeSchemaVersion();
                cloudscribeCoreCodeVersion = setupManager.GetCloudscribeCodeVersion();
                
                statusMessage.Append("<div class='settingrow'>");
                statusMessage.Append("<span class='settinglabel'>");
                //successMessage.Append(SetupResources.Version);
                statusMessage.Append("Version");
                statusMessage.Append("</span>");
                statusMessage.Append(cloudscribeCoreCodeVersion.ToString());
                statusMessage.Append("</div>");

                statusMessage.Append("<div class='settingrow'>");
                statusMessage.Append("<span class='settinglabel'>");
                //successMessage.Append(SetupResources.DatabaseStatus);
                statusMessage.Append("DatabaseStatus");
                statusMessage.Append("</span>");

                if (cloudscribeCoreCodeVersion > cloudscribeCoreDbSchemaVersion)
                {
                    //successMessage.Append(SetupResources.SchemaUpgradeNeededMessage);
                    statusMessage.Append("SchemaUpgradeNeeded");
                }

                if (cloudscribeCoreCodeVersion < cloudscribeCoreDbSchemaVersion)
                {
                    //successMessage.Append(SetupResources.CodeUpgradeNeededMessage);
                    statusMessage.Append("CodeUpgradeNeeded");
                }

                if (cloudscribeCoreCodeVersion == cloudscribeCoreDbSchemaVersion)
                {
                    //successMessage.Append(SetupResources.InstallationUpToDateMessage);
                    statusMessage.Append("InstallationUpToDate");

                }

                statusMessage.Append("</div>");
            }

            await WritePageContent(response, statusMessage.ToString(), false);

        }

        private async Task WriteException(HttpResponse response, Exception ex)
        {
            exceptionsRendered = true; // this is true if this method was called so we can at least say error happened
            if(!setupOptions.ShowErrors) { return; }
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
