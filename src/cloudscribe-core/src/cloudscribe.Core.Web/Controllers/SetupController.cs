// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2015-08-03
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
//using cloudscribe.Resources;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using System;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace cloudscribe.Setup.Controllers
{
    public class SetupController : Controller
    {
       
        public SetupController(
            IHostingEnvironment env,
            IApplicationEnvironment appEnv,
            ILoggerFactory loggerFactory,
            ConfigHelper configuration,
            IDb dbImplementation,
            SiteManager siteManager,
            ISiteRepository siteRepositoryImplementation,
            IUserRepository userRepositoryImplementation
        )
        {
            if (env == null) { throw new ArgumentNullException(nameof(env)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (dbImplementation == null) { throw new ArgumentNullException(nameof(dbImplementation)); }
            if (siteRepositoryImplementation == null) { throw new ArgumentNullException(nameof(siteRepositoryImplementation)); }
            if (userRepositoryImplementation == null) { throw new ArgumentNullException(nameof(userRepositoryImplementation)); }
            //if (versionProviderFactory == null) { throw new ArgumentNullException(nameof(versionProviderFactory)); }

            hostingEnvironment = env;
            config = configuration;
            db = dbImplementation;
            siteRepository = siteRepositoryImplementation;
            userRepository = userRepositoryImplementation;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(SetupController).FullName);
            appBasePath = appEnv.ApplicationBasePath;
            this.siteManager = siteManager;
            //versionProviders = db.VersionProviders;
        }

        private string appBasePath;
        private SiteManager siteManager;
        private IHostingEnvironment hostingEnvironment;
        private ILoggerFactory logFactory;
        private ILogger log;
        //private IVersionProviderFactory versionProviders;
        private ConfigHelper config;
        private bool setupIsDisabled = false;
        private bool dataFolderIsWritable = false;
        private bool canAccessDatabase = false;
        private bool schemaHasBeenCreated = false;
        private bool canAlterSchema = false;
        private bool showConnectionError = false;
        private int existingSiteCount = 0;
        private bool needSchemaUpgrade = false;
       // private int scriptTimeout;
        private DateTime startTime;
        private string dbPlatform = string.Empty;
        private Version dbCodeVersion = new Version(0,0);
        private Version dbSchemaVersion = new Version(0,0);
        private IDb db;
        private ISiteRepository siteRepository;
        private IUserRepository userRepository;

        



        public async Task<IActionResult> Index()
        {
            //scriptTimeout = Server.ScriptTimeout;
            //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            

            //IOwinContext owinContext = HttpContext.GetOwinContext();
            //StandardKernel ninjectKernel = owinContext.Get<StandardKernel>();
            //siteRepository = ninjectKernel.Get<ISiteRepository>();
            //userRepository = ninjectKernel.Get<IUserRepository>();
            //db = ninjectKernel.Get<IDb>();
            setupIsDisabled = config.DisableSetup();


            bool isAdmin = User.IsInRole("Admins");
            bool result;

            if (setupIsDisabled && !isAdmin)
            {
                log.LogInformation("returning 404 becuase setup is disabled and user is not logged in as an admin");
                Response.StatusCode = 404;
                //HttpContext.ApplicationInstance.CompleteRequest();
               
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

            if (LockForSetup())
            {
                try
                {
                    await ProbeSystem(Response);
                    result = await RunSetup(Response);

                    if (CoreSystemIsReady())
                    {
                        await ShowSetupSuccess(Response);
                    }
                }
                finally
                {
                    ClearSetupLock();
                }

            }
            else
            {
                await WritePageContent(Response,
                    "SetupAlreadyInProgress" //SetupResources.SetupAlreadyInProgress
                    );
            }

            await WritePageContent(Response,
                "SetupEnabled" //SetupResources.SetupEnabledMessage
                );

            await WritePageFooter(Response);

            //HttpContext.ApplicationInstance.CompleteRequest();

            return new EmptyResult();
        }

        private async Task<bool> RunSetup(HttpResponse response)
        {

            bool result = true;
            #region setup cloudscribe-core

            if (!schemaHasBeenCreated)
            {
                if (canAlterSchema)
                {

                    schemaHasBeenCreated = await CreateInitialSchema(response, "cloudscribe-core");
                    //schemaHasBeenCreated = DatabaseHelper.SchemaHasBeenCreated();
                    if (schemaHasBeenCreated)
                    {
                        //recheck
                        needSchemaUpgrade = SetupHelper.NeedsUpgrade("cloudscribe-core", db);
                    }

                }
            }

            if (
                (schemaHasBeenCreated)
                && (needSchemaUpgrade)
                && (canAlterSchema)
                )
            {
                needSchemaUpgrade = await UpgradeSchema(response, "cloudscribe-core");

            }

            if (!CoreSystemIsReady()) return false;

            //existingSiteCount = DatabaseHelper.ExistingSiteCount();
            if (existingSiteCount == 0)
            {
                await WritePageContent(response,
                    "CreatingSite" //SetupResources.CreatingSiteMessage
                    , true);


                //SiteSettings newSite = await NewSiteHelper.CreateNewSite(config, siteRepository, true);
                SiteSettings newSite = await siteManager.CreateNewSite(config, true);

                await WritePageContent(response,
                    "CreatingRolesAndAdminUser" //SetupResources.CreatingRolesAndAdminUserMessage
                    , true);

                result = await CreateAdminUser(newSite);
            }

            if (result)
            {

            }

            // look for new features or settings to install
            //SetupFeatures("mojoportal-core");


            #endregion

            #region setup other applications

            // install other apps

            //string pathToApplicationsFolder
            //    = hostingEnvironment.MapPath(
            //    "~/Config/applications/");

            string pathToApplicationsFolder = appBasePath + "/config/applications".Replace("/",Path.DirectorySeparatorChar.ToString());

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
                if (
                    (!string.Equals(appFolder.Name, "cloudscribe-core", StringComparison.CurrentCultureIgnoreCase))
                    && (appFolder.Name.ToLower() != ".svn")
                    && (appFolder.Name.ToLower() != "_svn")
                    )
                {
                    await CreateInitialSchema(response, appFolder.Name);
                    await UpgradeSchema(response, appFolder.Name);
                    SetupFeatures(response, appFolder.Name);
                }

            }

            #endregion

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

            // added 2013-10-18 
            //if (AppSettings.TryEnsureCustomMachineKeyOnSetup)
            //{
            //    try
            //    {
            //        //WebConfigSettings.EnsureCustomMachineKey();
            //    }
            //    catch (Exception ex)
            //    {
            //        log.LogError("tried to ensure a custom machinekey in Web.config but an error occurred.", ex);
            //    }
            //}

            return result;

        }

        //private static void SyncDefinitions(object o)
        //{
        //    ModuleDefinition.SyncDefinitions();
        //}

        private async Task<bool> CreateInitialSchema(HttpResponse response, string applicationName)
        {
            Guid appID = db.GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = db.GetSchemaVersion(appID);
            Version zeroVersion = new Version(0, 0, 0, 0);

            if (currentSchemaVersion > zeroVersion) { return true; } //already installed only run upgrade scripts

            Version versionToStopAt = null;
            //Guid mojoAppGuid = new Guid("077e4857-f583-488e-836e-34a4b04be855");
            //if (appID == mojoAppGuid)
            //{
            //    //versionToStopAt = DatabaseHelper.DBCodeVersion (); ;
            //}

            //string pathToScriptFolder = hostingEnvironment.MapPath(
            //    string.Format(config.SetupInstallScriptPathFormat(),
            //    applicationName,
            //    db.DBPlatform.ToLowerInvariant())
            //    );

            string pathToScriptFolder = appBasePath
               + string.Format(config.SetupInstallScriptPathFormat().Replace("/",Path.DirectorySeparatorChar.ToString()),
               applicationName,
               db.DBPlatform.ToLowerInvariant())
               ;

            //String pathToScriptFolder
            //    = hostingEnvironment.MapPath(
            //    "~/Config/applications/" + applicationName
            //    + "/SchemaInstallScripts/"
            //        + db.DBPlatform.ToLowerInvariant()
            //        + "/");

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

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");
            Array.Sort(scriptFiles, SetupHelper.CompareFileNames);


            if (scriptFiles.Length == 0)
            {
                await WritePageContent(response,
                "NoScriptsFilesFound" //SetupResources.NoScriptsFilesFoundMessage
                + " " + pathToScriptFolder,
                false);

                return false;

            }

            // We only want to run the highest version script from the /SchemaInstallationScripts/dbplatform folder
            // normally there is only 1 script in this folder, but if someone upgrades and then starts with a clean db
            // there can be more than one script because of the previous installs so we nned to make sure we only run the highest version found
            // since we sorted it the highest version is the last item in the array
            FileInfo scriptFile = scriptFiles[(scriptFiles.Length - 1)];

            Version currentSchemaVersion
                = db.GetSchemaVersion(applicationId);


            Version scriptVersion
                = SetupHelper.ParseVersionFromFileName(scriptFile.Name);

            if (
                (scriptVersion != null)
                && (scriptVersion > currentSchemaVersion)
                && (versionToStopAt == null || (scriptVersion <= versionToStopAt))
                )
            {
                string message = string.Format(
                    "RunningScript", //SetupResources.RunningScriptMessage,
                    applicationName,
                    scriptFile.Name.Replace(".config", string.Empty));

                await WritePageContent(response,
                    message,
                    true);

                string overrideConnectionString = GetOverrideConnectionString(applicationName);

                string errorMessage
                    = RunScript(
                        applicationId,
                        scriptFile,
                        overrideConnectionString);

                if (errorMessage.Length > 0)
                {
                    await WritePageContent(response, errorMessage, true);
                    return false;

                }

                //if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
                //{
                //    mojoSetup.DoPostScriptTasks(scriptVersion, null);
                //}

                Version newVersion
                    = SetupHelper.ParseVersionFromFileName(scriptFile.Name);

                if (
                    (applicationName != null)
                    && (newVersion != null)
                    )
                {
                    if (!db.UpdateSchemaVersion(
                        applicationId,
                        applicationName,
                        newVersion.Major,
                        newVersion.Minor,
                        newVersion.Build,
                        newVersion.Revision))
                    {
                        db.AddSchemaVersion(
                            applicationId,
                            applicationName,
                            newVersion.Major,
                            newVersion.Minor,
                            newVersion.Build,
                            newVersion.Revision);
                    }

                    db.AddSchemaScriptHistory(
                        applicationId,
                        scriptFile.Name,
                        DateTime.UtcNow,
                        false,
                        string.Empty,
                        string.Empty);

                    if (errorMessage.Length == 0)
                    {
                        currentSchemaVersion = newVersion;

                    }

                }
            }


            return result;

        }

        private string RunScript(
            Guid applicationId,
            FileInfo scriptFile,
            String overrideConnectionInfo)
        {
            // returning empty string indicates success
            // else return error message
            string resultMessage = string.Empty;

            if (scriptFile == null) return resultMessage;



            try
            {
                bool result = db.RunScript(
                    scriptFile, overrideConnectionInfo);

                if (!result)
                {
                    resultMessage = "script failed with no error message";
                }

            }
            catch (DbException ex)
            {
                resultMessage = ex.ToString();
            }

            return resultMessage;

        }

        private string GetOverrideConnectionString(string applicationName)
        {
            string overrideConnectionString = config.GetOrDefault(applicationName + "_ConnectionString", string.Empty);

            if (string.IsNullOrEmpty(overrideConnectionString)) { return null; }

            return overrideConnectionString;
        }

        private async Task<bool> UpgradeSchema(HttpResponse response, string applicationName)
        {
            
            Version versionToStopAt = null;
            IVersionProvider appVersionProvider = db.VersionProviders.Get(applicationName);

            //if (VersionProviderManager.Providers[applicationName] != null)
            //{
            //    VersionProvider appVersionProvider = VersionProviderManager.Providers[applicationName];
            //    versionToStopAt = appVersionProvider.GetCodeVersion();

            //}
            Guid appID;
            if (appVersionProvider != null)
            {
                versionToStopAt = appVersionProvider.GetCodeVersion();
                appID = appVersionProvider.ApplicationId;
            }
            else
            {
                appID = db.GetOrGenerateSchemaApplicationId(applicationName);
            }

            
            Version currentSchemaVersion = db.GetSchemaVersion(appID);


            //String pathToScriptFolder
            //    = hostingEnvironment.MapPath(
            //    "~/Config/applications/" + applicationName
            //        + "/SchemaUpgradeScripts/"
            //        + db.DBPlatform.ToLowerInvariant()
            //        + "/");

            //string pathToScriptFolder = hostingEnvironment.MapPath(
            //    string.Format(config.SetupUpgradeScriptPathFormat(),
            //    applicationName,
            //    db.DBPlatform.ToLowerInvariant())
            //    );

            string pathToScriptFolder = appBasePath
                + string.Format(config.SetupUpgradeScriptPathFormat().Replace("/", Path.DirectorySeparatorChar.ToString()),
                applicationName,
                db.DBPlatform.ToLowerInvariant())
                ;


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

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");


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

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");
            Array.Sort(scriptFiles, SetupHelper.CompareFileNames);

            if (scriptFiles.Length == 0)
            {
                //WritePageContent(
                //SetupResource.NoScriptsFilesFoundMessage 
                //+ " " + pathToScriptFolder,
                //false);

                return false;

            }


            Version currentSchemaVersion
                = db.GetSchemaVersion(applicationId);

            foreach (FileInfo scriptFile in scriptFiles)
            {
                Version scriptVersion
                    = SetupHelper.ParseVersionFromFileName(scriptFile.Name);

                if (
                    (scriptVersion != null)
                    && (scriptVersion > currentSchemaVersion)
                    && (versionToStopAt == null || (scriptVersion <= versionToStopAt))
                    // commented out 2007-08-26
                    // script is still logged if it fails but version
                    // isn't updated. This was blocking the script from
                    // running again unless user deleted row from mp_SchemaScriptHistory
                    //&& (!DatabaseHelper.SchemaScriptHasBeenRun(
                    //        applicationID, 
                    //        scriptFile.Name)
                    //    )
                    )
                {
                    string message = string.Format(
                        "RunningScript {0} {1}", //SetupResources.RunningScriptMessage,
                        applicationName,
                        scriptFile.Name.Replace(".config", string.Empty));

                    await WritePageContent(response,
                        message,
                        true);

                    string overrideConnectionString = GetOverrideConnectionString(applicationName);

                    string errorMessage
                        = RunScript(
                            applicationId,
                            scriptFile,
                            overrideConnectionString);

                    if (errorMessage.Length > 0)
                    {
                        await WritePageContent(response, errorMessage, true);
                        return false;

                    }

                    //if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    mojoSetup.DoPostScriptTasks(scriptVersion, null);
                    //}

                    Version newVersion
                        = SetupHelper.ParseVersionFromFileName(scriptFile.Name);

                    if (
                        (applicationName != null)
                        && (newVersion != null)
                        )
                    {
                        db.UpdateSchemaVersion(
                            applicationId,
                            applicationName,
                            newVersion.Major,
                            newVersion.Minor,
                            newVersion.Build,
                            newVersion.Revision);

                        db.AddSchemaScriptHistory(
                            applicationId,
                            scriptFile.Name,
                            DateTime.UtcNow,
                            false,
                            string.Empty,
                            string.Empty);

                        if (errorMessage.Length == 0)
                        {
                            currentSchemaVersion = newVersion;

                        }

                    }
                }

            }

            return result;

        }

        private async Task<bool> CreateAdminUser(SiteSettings newSite)
        {

            //bool result = await NewSiteHelper.CreateRequiredRolesAndAdminUser(
            //    newSite,
            //    siteRepository,
            //    userRepository,
            //    config
            //    );

            bool result = await siteManager.CreateRequiredRolesAndAdminUser(
                newSite,
                config
                );

            return result;

        }


        private async Task ProbeSystem(HttpResponse response)
        {
            await WritePageContent(response,
                "ProbingSystem", //SetupResources.ProbingSystemMessage,
                false);

            dbPlatform = db.DBPlatform;
            //dataFolderIsWritable = mojoSetup.DataFolderIsWritable();

            if ((dbPlatform == "SqlCe") | (dbPlatform == "SQLite"))
            {
                db.EnsureDatabase();
            }

            if (dataFolderIsWritable)
            {
                await WritePageContent(response,
                    "FileSystemPermissionsOK", //SetupResources.FileSystemPermissionsOKMesage,
                    false);
            }
            else
            {
                await WritePageContent(response,
                    "FileSystemPermissionProblems", //SetupResources.FileSystemPermissionProblemsMessage,
                    false);

                //WritePageContent(
                //    "<div>" + GetFolderDetailsHtml() + "</div>",
                //    false);
            }

            canAccessDatabase = db.CanAccessDatabase();

            /*LUIS SILVA ForceDatabaseCreation for MS SQL 2012-06-13*/

            // add this to user.config <add key="TryToCreateMsSqlDatabase" value="true"/>
            // and make sure the connection string is configured with a user that has permission to create the database

            if ((!canAccessDatabase) && (dbPlatform == "MSSQL") && config.TryToCreateMsSqlDatabase())
            {
                await WritePageContent(response, dbPlatform + " " 
                    + "TryingToCreateDatabase" // SetupResources.TryingToCreateDatabase
                    , false);
                db.EnsureDatabase();
                canAccessDatabase = db.CanAccessDatabase();
                if (canAccessDatabase)
                {
                    await WritePageContent(response, dbPlatform + " " 
                        + "DatabaseCreationSucceeded" //SetupResources.DatabaseCreationSucceeded
                        , false);
                }

            }


            if (canAccessDatabase)
            {
                await WritePageContent(response,
                    dbPlatform
                    + " " + "DatabaseConnectionOK", // SetupResources.DatabaseConnectionOKMessage,
                    false);
            }
            else
            {
                string dbError = string.Format(
                    "FailedToConnectToDatabase", //SetupResources.FailedToConnectToDatabase,
                    dbPlatform);

                await WritePageContent(response, "<div>" + dbError + "</div>", false);

                showConnectionError = config.ShowConnectionErrorOnSetup();


                if (showConnectionError)
                {
                    await WritePageContent(response,
                        "<div>" + db.GetConnectionError(null).ToString()
                        + "</div>",
                        false);
                }
            }


            if (canAccessDatabase)
            {
                canAlterSchema = db.CanAlterSchema(null);

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

                schemaHasBeenCreated = db.SitesTableExists();

                if (schemaHasBeenCreated)
                {
                    await WritePageContent(response,
                        "DatabaseSchemaAlreadyExists", //SetupResources.DatabaseSchemaAlreadyExistsMessage,
                        false);


                    needSchemaUpgrade = SetupHelper.NeedsUpgrade("cloudscribe-core", db);

                    if (needSchemaUpgrade)
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

                    existingSiteCount = db.ExistingSiteCount();

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

        private bool CoreSystemIsReady()
        {
            bool result = true;

            if (!canAccessDatabase) return false;

            if (!db.SitesTableExists()) return false;

            if (SetupHelper.NeedsUpgrade("cloudscribe-core", db)) { return false; }




            return result;
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

        private async Task ShowSetupSuccess(HttpResponse response)
        {

            StringBuilder successMessage = new StringBuilder();
            successMessage.Append("<hr /><div>" 
                + "SetupSuccess" // SetupResources.SetupSuccessMessage 
                + "</div>");

            successMessage.Append("<a href='" + "/" // TODO: was using Page.ResolveUrl("~/")
                + "' title='" + "Home" //SetupResources.HomeLink 
                + "'>"
                + "Home" // SetupResources.HomeLink 
                + "</a>");

            successMessage.Append("<br /><br />");

            successMessage.Append("<div class='settingrow'>");
            successMessage.Append("<span class='settinglabel'>");
            //successMessage.Append(SetupResources.DatabasePlatform);
            successMessage.Append("DatabasePlatform");
            successMessage.Append("</span>");
            successMessage.Append(db.DBPlatform);
            successMessage.Append("</div>");

            if (schemaHasBeenCreated)
            {
                Guid appID = db.GetOrGenerateSchemaApplicationId("cloudscribe-core");
                dbSchemaVersion = db.GetSchemaVersion(appID);

                //dbCodeVersion = DatabaseHelper.DBCodeVersion();
                //dbSchemaVersion = DatabaseHelper.DBSchemaVersion();
                IVersionProvider coreVersionProvider = db.VersionProviders.Get("cloudscribe-core");
                //if (VersionProviderManager.Providers["cloudscribe-core"] != null)
                if(coreVersionProvider != null)
                {
                    dbCodeVersion = coreVersionProvider.GetCodeVersion();
                }

                successMessage.Append("<div class='settingrow'>");
                successMessage.Append("<span class='settinglabel'>");
                //successMessage.Append(SetupResources.Version);
                successMessage.Append("Version");
                successMessage.Append("</span>");
                successMessage.Append(dbCodeVersion.ToString());
                successMessage.Append("</div>");

                successMessage.Append("<div class='settingrow'>");
                successMessage.Append("<span class='settinglabel'>");
                //successMessage.Append(SetupResources.DatabaseStatus);
                successMessage.Append("DatabaseStatus");
                successMessage.Append("</span>");

                if (dbCodeVersion > dbSchemaVersion)
                {
                    //successMessage.Append(SetupResources.SchemaUpgradeNeededMessage);
                    successMessage.Append("SchemaUpgradeNeeded");
                }

                if (dbCodeVersion < dbSchemaVersion)
                {
                    //successMessage.Append(SetupResources.CodeUpgradeNeededMessage);
                    successMessage.Append("CodeUpgradeNeeded");
                }

                if (dbCodeVersion == dbSchemaVersion)
                {
                    //successMessage.Append(SetupResources.InstallationUpToDateMessage);
                    successMessage.Append("InstallationUpToDate");

                }

                successMessage.Append("</div>");
            }

            await WritePageContent(response, successMessage.ToString(), false);

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

        private bool LockForSetup()
        {

            //if (HttpContext.Application["UpgradeInProgress"] != null)
            //{
            //    bool upgradeInProgress = (bool)HttpContext.Application["UpgradeInProgress"];
            //    if (upgradeInProgress) return false;

            //}

            //HttpContext.Application["UpgradeInProgress"] = true;
            return true;
        }

        private void ClearSetupLock()
        {
            //HttpContext.Application["UpgradeInProgress"] = false;
        }



    }
}
