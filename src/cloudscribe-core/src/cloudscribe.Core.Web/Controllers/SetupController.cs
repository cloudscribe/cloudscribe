// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2015-08-13
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
//using cloudscribe.Resources;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using System;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.Setup.Controllers
{
    public class SetupController : Controller
    {
       
        public SetupController(
            IApplicationEnvironment appEnv,
            ILogger<SetupController> logger,
            ConfigHelper configuration,
            SetupManager setupManager,
            SiteManager siteManager 
        )
        {
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (setupManager == null) { throw new ArgumentNullException(nameof(setupManager)); }
            if (siteManager == null) { throw new ArgumentNullException(nameof(siteManager)); }

            config = configuration;
            log = logger;
            appBasePath = appEnv.ApplicationBasePath;
            this.siteManager = siteManager;
            this.setupManager = setupManager;

        }

        private SetupManager setupManager;
        private string appBasePath;
        private SiteManager siteManager;
        private ILogger log;
        private ConfigHelper config;
        private bool setupIsDisabled = false;
        //private bool dataFolderIsWritable = false;
        private bool canAccessDatabase = false;
        private bool schemaHasBeenCreated = false;
        private bool canAlterSchema = false;
        private bool showConnectionError = false;
        private int existingSiteCount = 0;
        private bool needCoreSchemaUpgrade = false;
        private Version cloudscribeCoreCodeVersion = new Version(0,0);
        private Version cloudscribeCoreDbSchemaVersion = new Version(0,0);
        // private int scriptTimeout;
        private DateTime startTime;


        private static object Lock = new object();



        public async Task<IActionResult> Index()
        {
            //scriptTimeout = Server.ScriptTimeout;
            //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            
            setupIsDisabled = config.DisableSetup();


            bool isAdmin = User.IsInRole("Admins");
            bool result;

            if (setupIsDisabled && !isAdmin)
            {
                log.LogInformation("returning 404 becuase setup is disabled and user is not logged in as an admin");
                Response.StatusCode = 404;
                return new EmptyResult();
            }



            //Response.BufferOutput = true;
           // Server.ScriptTimeout = int.MaxValue;
            startTime = DateTime.UtcNow;
            
            await WritePageHeader(Context.Response);

            if (setupIsDisabled && isAdmin)
            {
                await WritePageContent(Response,
                    "RunningSetupForAdminUser" //SetupResources.RunningSetupForAdminUser
                    );

            }

            int lockTimeoutMilliseconds = config.GetOrDefault("AppSetings:SetupLockTimeoutMilliseconds", 60000); // 1 minute

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


                    result = await SetupCloudscribeCore(Response);

                    if(result)
                    {
                        result = await SetupOtherApplications(Response);
                    }

                
                    await ShowSetupStatus(Response);
                    
                }
                finally
                {
                    //ClearSetupLock();
                    //Monitor.Exit(Lock);
                }

            //}
            

            //await WritePageContent(Response,
            //    "SetupEnabled" //SetupResources.SetupEnabledMessage
            //    );

            await WritePageFooter(Response);

            //HttpContext.ApplicationInstance.CompleteRequest();

            return new EmptyResult();
        }

        private async Task<bool> SetupCloudscribeCore(HttpResponse response)
        {

            bool result = true;
            
            if (!schemaHasBeenCreated)
            {
                if (canAlterSchema)
                {

                    schemaHasBeenCreated = await CreateInitialSchema(response, "cloudscribe-core");

                    if (schemaHasBeenCreated)
                    {
                        //recheck
                        needCoreSchemaUpgrade = setupManager.NeedsUpgrade("cloudscribe-core");
                    }

                }
            }

            if (
                (schemaHasBeenCreated)
                && (needCoreSchemaUpgrade)
                && (canAlterSchema)
                )
            {
                needCoreSchemaUpgrade = await UpgradeSchema(response, "cloudscribe-core");
            }

            if (!CoreSystemIsReady()) return false;

            existingSiteCount = setupManager.ExistingSiteCount();

            if (existingSiteCount == 0)
            {
                await WritePageContent(response,
                    "CreatingSite" //SetupResources.CreatingSiteMessage
                    , true);

                SiteSettings newSite = await siteManager.CreateNewSite(config, true);

                await WritePageContent(response,
                    "CreatingRolesAndAdminUser" //SetupResources.CreatingRolesAndAdminUserMessage
                    , true);

                result = await siteManager.CreateRequiredRolesAndAdminUser(newSite, config);
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
                // skip cloudscribe-core since we set that up first
                if (
                    (!string.Equals(appFolder.Name, "cloudscribe-core", StringComparison.CurrentCultureIgnoreCase))
                    && (appFolder.Name.ToLower() != ".svn")
                    && (appFolder.Name.ToLower() != "_svn")
                    )
                {
                    await CreateInitialSchema(response, appFolder.Name);
                    await UpgradeSchema(response, appFolder.Name);

                    // this doesn't currently do anything
                    // we will use it to plugin other setup taks for cms etc
                    SetupFeatures(response, appFolder.Name);
                }

            }

           

            //WritePageContent(SetupResources.EnsuringFeaturesInAdminSites, true);
            //ModuleDefinition.EnsureInstallationInAdminSites();

            //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            //if (siteSettings != null)
            //{
            //    if (PageSettings.GetCountOfPages(siteSettings.SiteId) == 0)
            //    {
            //        WritePageContent(SetupResource.CreatingDefaultContent);
            //        //SetupContentPages(siteSettings);
            //        mojoSetup.SetupDefaultContentPages(siteSettings);
            //    }

            //    try
            //    {
            //        int userCount = SiteUser.UserCount(siteSettings.SiteId);
            //        if (userCount == 0) { mojoSetup.EnsureRolesAndAdminUser(siteSettings); }

            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error("EnsureAdminUserAndRoles", ex);
            //    }

            //    mojoSetup.EnsureSkins(siteSettings.SiteId);
            //}

            // in case control type controlsrc, regex or sort changed on the definition
            // update instance properties to match
            //ThreadPool.QueueUserWorkItem(new WaitCallback(SyncDefinitions), null);
            //ModuleDefinition.SyncDefinitions();

            //SiteSettings.EnsureExpandoSettings();

            //mojoSetup.EnsureAdditionalSiteFolders();

          

            return result;

        }

        

        private async Task<bool> CreateInitialSchema(HttpResponse response, string applicationName)
        {
            Guid appID = setupManager.GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = setupManager.GetSchemaVersion(appID);
            Version zeroVersion = new Version(0, 0, 0, 0);

            if (currentSchemaVersion > zeroVersion) { return true; } //already installed only run upgrade scripts

            Version versionToStopAt = null; // because we don't stop on install we start with the highest version in the folder which is also the last one

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
            string pathToScriptFolder = setupManager.GetPathToUpgradeScriptFolder(applicationName);


            if (!Directory.Exists(pathToScriptFolder))
            {
                //string warning = string.Format(
                //    SetupResource.SchemaUpgradeFolderNotFound,
                //    applicationName, pathToScriptFolder);

                //log.Warn(warning);

                await WritePageContent(response, pathToScriptFolder + "not found");
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

                setupManager.AddSchemaScriptHistory(
                    applicationId,
                    scriptFile.Name,
                    DateTime.UtcNow,
                    false,
                    string.Empty,
                    string.Empty);

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

            //dbPlatform = setupManager.DBPlatform;

            setupManager.EnsureDatabaseIfPossible();
            //dataFolderIsWritable = mojoSetup.DataFolderIsWritable();


            //if (dataFolderIsWritable)
            //{
            //    await WritePageContent(response,
            //        "FileSystemPermissionsOK", //SetupResources.FileSystemPermissionsOKMesage,
            //        false);
            //}
            //else
            //{
            //    await WritePageContent(response,
            //        "FileSystemPermissionProblems", //SetupResources.FileSystemPermissionProblemsMessage,
            //        false);

            //    //WritePageContent(
            //    //    "<div>" + GetFolderDetailsHtml() + "</div>",
            //    //    false);
            //}

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

                showConnectionError = config.ShowConnectionErrorOnSetup();


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

                    if (config.SetupTryAnywayIfFailedAlterSchemaTest())
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

                schemaHasBeenCreated = setupManager.SiteTableExists();

                if (schemaHasBeenCreated)
                {
                    await WritePageContent(response,
                        "DatabaseSchemaAlreadyExists", //SetupResources.DatabaseSchemaAlreadyExistsMessage,
                        false);


                    needCoreSchemaUpgrade = setupManager.NeedsUpgrade("cloudscribe-core");

                    if (needCoreSchemaUpgrade)
                    {
                        await WritePageContent(response,
                            "DatabaseSchemaNeedsUpgrade", //SetupResources.DatabaseSchemaNeedsUpgradeMessage,
                            false);
                    }
                    else
                    {
                        await WritePageContent(response,
                            "DatabaseSchemaUpToDate", //SetupResources.DatabaseSchemaUpToDateMessage,
                            false);
                    }

                    existingSiteCount = setupManager.ExistingSiteCount();

                    await WritePageContent(response,
                        string.Format(
                        "ExistingSiteCount", //SetupResources.ExistingSiteCountMessage,
                        existingSiteCount.ToString()),
                        false);

                }
                else
                {
                    await WritePageContent(response,
                        "DatabaseSchemaNotCreatedYet", //SetupResources.DatabaseSchemaNotCreatedYetMessage,
                        false);
                }

            }

            //if (!SetupHelper.RunningInFullTrust())
            //{
            //    // inform of Medium trust configuration issues
            //    WritePageContent(response,
            //        "<b>" + SetupResources.MediumTrustGeneralMessage + "</b><br />"
            //        + GetDataAccessMediumTrustMessage() + "<br /><br />",
            //        false);

            //}
        }

        

        private bool CoreSystemIsReady()
        {
            if (!canAccessDatabase) return false;

            if (!setupManager.SiteTableExists()) return false;

            if (setupManager.NeedsUpgrade("cloudscribe-core")) { return false; }

            return true;
        }

        

        private async Task ShowSetupStatus(HttpResponse response)
        {

            StringBuilder statusMessage = new StringBuilder();

            if(CoreSystemIsReady())
            {
                statusMessage.Append("<hr /><div>"
                + "SetupSuccess" // SetupResources.SetupSuccessMessage 
                + "</div>");
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

            if (schemaHasBeenCreated)
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
       
            //response.Flush();

        }

        private async Task WritePageHeader(HttpResponse response)
        {

            string setupTemplatePath = config.SetupHeaderConfigPath().Replace("/", Path.DirectorySeparatorChar.ToString());
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                setupTemplatePath = config.SetupHeaderConfigPathRtl().Replace("/", Path.DirectorySeparatorChar.ToString());
            }

            //string fsPath = hostingEnvironment.MapPath(setupTemplatePath);
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

            //response.Flush();
        }

        private async Task WritePageFooter(HttpResponse response)
        {
            await response.WriteAsync("</body>");
            await response.WriteAsync("</html>");
            //response.Flush();
        }



        private void SetupFeatures(HttpResponse response, string applicationName)
        {
            //ContentFeatureConfiguration appFeatureConfig
            //    = ContentFeatureConfiguration.GetConfig(applicationName);

            //WritePageContent(
            //    string.Format(SetupResource.ConfigureFeaturesMessage, 
            //    applicationName));

            //foreach (ContentFeature feature in appFeatureConfig.ContentFeatures)
            //{
            //    if (feature.SupportedDatabases.Contains(dbPlatform))
            //    {
            //        SetupFeature(feature);
            //    }

            //}
        }

        //private async Task<bool> CreateAdminUser(SiteSettings newSite)
        //{

        //    //bool result = await NewSiteHelper.CreateRequiredRolesAndAdminUser(
        //    //    newSite,
        //    //    siteRepository,
        //    //    userRepository,
        //    //    config
        //    //    );

        //    bool result = await siteManager.CreateRequiredRolesAndAdminUser(
        //        newSite,
        //        config
        //        );

        //    return result;

        //}

        //private string GetDataAccessMediumTrustMessage()
        //{
        //    string message = string.Empty;
        //    string dbPlatform = db.DBPlatform;
        //    switch (dbPlatform)
        //    {
        //        case "MySQL":
        //            message = SetupResources.MediumTrustMySQLMessage;
        //            break;

        //        case "pgsql":
        //            message = SetupResources.MediumTrustnpgsqlMessage;
        //            break;


        //    }

        //    return message;

        //}

        //private string RunScript(
        //    Guid applicationId,
        //    FileInfo scriptFile,
        //    String overrideConnectionInfo)
        //{
        //    // returning empty string indicates success
        //    // else return error message
        //    string resultMessage = string.Empty;

        //    if (scriptFile == null) return resultMessage;



        //    try
        //    {
        //        bool result = db.RunScript(scriptFile, overrideConnectionInfo);

        //        if (!result)
        //        {
        //            resultMessage = "script failed with no error message";
        //        }

        //    }
        //    catch (DbException ex)
        //    {
        //        resultMessage = ex.ToString();
        //    }

        //    return resultMessage;

        //}

        //private string GetOverrideConnectionString(string applicationName)
        //{
        //    string overrideConnectionString = config.GetOrDefault(applicationName + "_ConnectionString", string.Empty);

        //    if (string.IsNullOrEmpty(overrideConnectionString)) { return null; }

        //    return overrideConnectionString;
        //}


    }
}
