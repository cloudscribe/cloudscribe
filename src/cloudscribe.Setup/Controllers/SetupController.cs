// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2015-01-10
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Resources;
using log4net;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;
using System;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cloudscribe.Setup.Controllers
{
    public class SetupController : Controller
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(SetupController));

        private bool setupIsDisabled = false;
        private bool dataFolderIsWritable = false;
        private bool canAccessDatabase = false;
        private bool schemaHasBeenCreated = false;
        private bool canAlterSchema = false;
        private bool showConnectionError = false;
        private int existingSiteCount = 0;
        private bool needSchemaUpgrade = false;
        private int scriptTimeout;
        private DateTime startTime;
        private string dbPlatform = string.Empty;
        private Version dbCodeVersion = new Version();
        private Version dbSchemaVersion = new Version();
        private IDb db;
        private ISiteRepository siteRepository;
        private IUserRepository userRepository;


        public async Task<ActionResult> Test()
        {
            scriptTimeout = Server.ScriptTimeout;
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);

            IOwinContext owinContext = HttpContext.GetOwinContext();
            StandardKernel ninjectKernel = owinContext.Get<StandardKernel>();
            siteRepository = ninjectKernel.Get<ISiteRepository>();
            userRepository = ninjectKernel.Get<IUserRepository>();
            db = ninjectKernel.Get<IDb>();

            Response.BufferOutput = true;

            setupIsDisabled = AppSettings.DisableSetup;

            Server.ScriptTimeout = int.MaxValue;
            startTime = DateTime.UtcNow;
            bool isAdmin = false;
            //bool result;
            //try
            //{
            //    //isAdmin = WebUser.IsAdmin;
            //}
            //catch { }

            WritePageHeader(Response);

            if (setupIsDisabled && !isAdmin)
            {
                WritePageContent(Response, SetupResources.SetupDisabledMessage);
            }
            else
            {
                if (setupIsDisabled && isAdmin)
                {
                    WritePageContent(Response, SetupResources.RunningSetupForAdminUser);

                }

                if (LockForSetup())
                {
                    try
                    {
                        //ProbeSystem(Response);
                        //result = await RunSetup(Response);
                        Response.Write("<ul>");

                        for (int x = 1; x < 11; x++ )
                        {
                            Response.Write("<li>" + x.ToInvariantString() + "</li>");
                            Response.Flush();
                            bool fun = await DoSomethingSlow();
                        }

                            Response.Write("/<ul>");
                    }
                    finally
                    {
                        ClearSetupLock();
                    }

                }
                else
                {
                    WritePageContent(Response, SetupResources.SetupAlreadyInProgress);
                }

                WritePageContent(Response, SetupResources.SetupEnabledMessage);


            }





            WritePageFooter(Response);

            HttpContext.ApplicationInstance.CompleteRequest();

            return new EmptyResult();
        }

        private async Task<bool> DoSomethingSlow()
        {
            await Task.Delay(5000);

            return true;
        }

        public async Task<ActionResult> Index()
        {
            scriptTimeout = Server.ScriptTimeout;
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);

            IOwinContext owinContext = HttpContext.GetOwinContext();
            StandardKernel ninjectKernel = owinContext.Get<StandardKernel>();
            siteRepository = ninjectKernel.Get<ISiteRepository>();
            userRepository = ninjectKernel.Get<IUserRepository>();
            db = ninjectKernel.Get<IDb>();

            Response.BufferOutput = true;

            setupIsDisabled = AppSettings.DisableSetup;

            Server.ScriptTimeout = int.MaxValue;
            startTime = DateTime.UtcNow;
            bool isAdmin = false;
            bool result;
            //try
            //{
            //    //isAdmin = WebUser.IsAdmin;
            //}
            //catch { }

            WritePageHeader(Response);

            if (setupIsDisabled && !isAdmin)
            {
                WritePageContent(Response, SetupResources.SetupDisabledMessage);
            }
            else
            {
                if (setupIsDisabled && isAdmin)
                {
                    WritePageContent(Response, SetupResources.RunningSetupForAdminUser);

                }

                if (LockForSetup())
                {
                    try
                    {
                        ProbeSystem(Response);
                        result = await RunSetup(Response);

                        if (CoreSystemIsReady())
                        {
                            ShowSetupSuccess(Response);
                        }
                    }
                    finally
                    {
                        ClearSetupLock();
                    }

                }
                else
                {
                    WritePageContent(Response, SetupResources.SetupAlreadyInProgress);
                }

                WritePageContent(Response, SetupResources.SetupEnabledMessage);


            }
           




            WritePageFooter(Response);

            HttpContext.ApplicationInstance.CompleteRequest();

            return new EmptyResult();     
        }

        private async Task<bool> RunSetup(HttpResponseBase response)
        {

            bool result = true;
            #region setup cloudscribe-core

            if (!schemaHasBeenCreated)
            {
                if (canAlterSchema)
                {

                    schemaHasBeenCreated = CreateInitialSchema(response, "cloudscribe-core");
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
                needSchemaUpgrade = UpgradeSchema(response, "cloudscribe-core");

            }

            if (!CoreSystemIsReady()) return false;

            //existingSiteCount = DatabaseHelper.ExistingSiteCount();
            if (existingSiteCount == 0)
            {
                WritePageContent(response, SetupResources.CreatingSiteMessage, true);
                SiteSettings newSite = NewSiteHelper.CreateNewSite(siteRepository);
                //mojoSetup.CreateDefaultSiteFolders(newSite.SiteId);
                //mojoSetup.CreateOrRestoreSiteSkins(newSite.SiteId);
                WritePageContent(response, SetupResources.CreatingRolesAndAdminUserMessage, true);

                result = await CreateAdminUser(newSite);
            }

            if(result)
            {

            }

            // look for new features or settings to install
            //SetupFeatures("mojoportal-core");


            #endregion

            #region setup other applications

            // install other apps

            String pathToApplicationsFolder
                = Server.MapPath(
                "~/Config/applications/");

            if (!Directory.Exists(pathToApplicationsFolder))
            {
                WritePageContent(response,
                pathToApplicationsFolder
                + " " + SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;
            }

            DirectoryInfo appRootFolder
                = new DirectoryInfo(pathToApplicationsFolder);

            DirectoryInfo[] appFolders = appRootFolder.GetDirectories();

            foreach (DirectoryInfo appFolder in appFolders)
            {
                if (
                    (!string.Equals(appFolder.Name, "cloudscribe-core", StringComparison.InvariantCultureIgnoreCase))
                    && (appFolder.Name.ToLower() != ".svn")
                    && (appFolder.Name.ToLower() != "_svn")
                    )
                {
                    CreateInitialSchema(response, appFolder.Name);
                    UpgradeSchema(response, appFolder.Name);
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
            if (AppSettings.TryEnsureCustomMachineKeyOnSetup)
            {
                try
                {
                    //WebConfigSettings.EnsureCustomMachineKey();
                }
                catch (Exception ex)
                {
                    log.Error("tried to ensure a custom machinekey in Web.config but an error occurred.", ex);
                }
            }

            return result;

        }

        //private static void SyncDefinitions(object o)
        //{
        //    ModuleDefinition.SyncDefinitions();
        //}

        private bool CreateInitialSchema(HttpResponseBase response, string applicationName)
        {
            Guid appID = db.GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = db.GetSchemaVersion(appID);
            Version zeroVersion = new Version(0, 0, 0, 0);

            if (currentSchemaVersion > zeroVersion) { return true; } //already installed only run upgrade scripts

            Version versionToStopAt = null;
            //Guid mojoAppGuid = new Guid("077e4857-f583-488e-836e-34a4b04be855");
            //if (appID == mojoAppGuid)
            //{
            //    //versionToStopAt = DatabaseHelper.DBCodeVersion(); ;
            //}


            String pathToScriptFolder
                = Server.MapPath(
                "~/Config/applications/" + applicationName
                + "/SchemaInstallScripts/"
                    + db.DBPlatform.ToLowerInvariant()
                    + "/");

            if (!Directory.Exists(pathToScriptFolder))
            {
                WritePageContent(response,
                pathToScriptFolder + " " + SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;

            }

            return RunSetupScript(
                response,
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

        }

        private bool RunSetupScript(
            HttpResponseBase response,
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                WritePageContent(response,
                pathToScriptFolder + " " + SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;
            }

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");
            Array.Sort(scriptFiles, SetupHelper.CompareFileNames);


            if (scriptFiles.Length == 0)
            {
                WritePageContent(response,
                SetupResources.NoScriptsFilesFoundMessage
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
                    SetupResources.RunningScriptMessage,
                    applicationName,
                    scriptFile.Name.Replace(".config", string.Empty));

                WritePageContent(response,
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
                    WritePageContent(response, errorMessage, true);
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
            string overrideConnectionString = AppSettings.GetString(applicationName + "_ConnectionString", string.Empty);

            if (string.IsNullOrEmpty(overrideConnectionString)) { return null; }

            return overrideConnectionString;
        }

        private bool UpgradeSchema(HttpResponseBase response, string applicationName)
        {
            Guid appID = db.GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = db.GetSchemaVersion(appID);
            Version versionToStopAt = null;

            if (VersionProviderManager.Providers[applicationName] != null)
            {
                VersionProvider appVersionProvider = VersionProviderManager.Providers[applicationName];
                versionToStopAt = appVersionProvider.GetCodeVersion();

            }

            String pathToScriptFolder
                = Server.MapPath(
                "~/Config/applications/" + applicationName
                    + "/SchemaUpgradeScripts/"
                    + db.DBPlatform.ToLowerInvariant()
                    + "/");

            if (!Directory.Exists(pathToScriptFolder))
            {
                //string warning = string.Format(
                //    SetupResource.SchemaUpgradeFolderNotFound,
                //    applicationName, pathToScriptFolder);

                //log.Warn(warning);

                //WritePageContent(warning);
                return false;

            }

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");


            if (scriptFiles.Length == 0)
            {
                string warning = string.Format(
                    SetupResources.NoUpgradeScriptsFound,
                    applicationName);

                return false;

            }



            bool result = RunUpgradeScripts(
                response,
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

            return result;

        }

        private bool RunUpgradeScripts(
            HttpResponseBase response,
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                WritePageContent(response,
                pathToScriptFolder + " " + SetupResources.ScriptFolderNotFoundAddendum,
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
                        SetupResources.RunningScriptMessage,
                        applicationName,
                        scriptFile.Name.Replace(".config", string.Empty));

                    WritePageContent(response,
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
                        WritePageContent(response, errorMessage, true);
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
            
            bool result =  await NewSiteHelper.CreateRequiredRolesAndAdminUser(
                newSite,
                siteRepository,
                userRepository
                );

            return result;

        }


        private void ProbeSystem(HttpResponseBase response)
        {
            WritePageContent(response,
                SetupResources.ProbingSystemMessage,
                false);

            dbPlatform = db.DBPlatform;
            //dataFolderIsWritable = mojoSetup.DataFolderIsWritable();

            if ((dbPlatform == "SqlCe") | (dbPlatform == "SQLite"))
            {
                db.EnsureDatabase();
            }

            if (dataFolderIsWritable)
            {
                WritePageContent(response,
                    SetupResources.FileSystemPermissionsOKMesage,
                    false);
            }
            else
            {
                WritePageContent(response,
                    SetupResources.FileSystemPermissionProblemsMessage,
                    false);

                //WritePageContent(
                //    "<div>" + GetFolderDetailsHtml() + "</div>",
                //    false);
            }

            canAccessDatabase = db.CanAccessDatabase();

            /*LUIS SILVA ForceDatabaseCreation for MS SQL 2012-06-13*/

            // add this to user.config <add key="TryToCreateMsSqlDatabase" value="true"/>
            // and make sure the connection string is configured with a user that has permission to create the database

            if ((!canAccessDatabase) && (dbPlatform == "MSSQL") && AppSettings.TryToCreateMsSqlDatabase)
            {
                WritePageContent(response, dbPlatform + " " + SetupResources.TryingToCreateDatabase, false);
                db.EnsureDatabase();
                canAccessDatabase = db.CanAccessDatabase();
                if (canAccessDatabase)
                {
                    WritePageContent(response, dbPlatform + " " + SetupResources.DatabaseCreationSucceeded, false);
                }

            }


            if (canAccessDatabase)
            {
                WritePageContent(response,
                    dbPlatform
                    + " " + SetupResources.DatabaseConnectionOKMessage,
                    false);
            }
            else
            {
                string dbError = string.Format(
                    SetupResources.FailedToConnectToDatabase,
                    dbPlatform);

                WritePageContent(response, "<div>" + dbError + "</div>", false);

                showConnectionError = AppSettings.GetBool("ShowConnectionErrorOnSetup", false);


                if (showConnectionError)
                {
                    WritePageContent(response,
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
                    WritePageContent(response,
                        SetupResources.DatabaseCanAlterSchemaMessage,
                        false);
                }
                else
                {

                    if (AppSettings.SetupTryAnywayIfFailedAlterSchemaTest)
                    {
                        canAlterSchema = true;
                    }
                    else
                    {
                        WritePageContent(response,
                       "<div>" + SetupResources.CantAlterSchemaWarning
                       + "</div>",
                       false);
                    }
                }

                schemaHasBeenCreated = db.SitesTableExists();

                if (schemaHasBeenCreated)
                {
                    WritePageContent(response,
                        SetupResources.DatabaseSchemaAlreadyExistsMessage,
                        false);


                    needSchemaUpgrade = SetupHelper.NeedsUpgrade("cloudscribe-core", db);

                    if (needSchemaUpgrade)
                    {
                        WritePageContent(response,
                            SetupResources.DatabaseSchemaNeedsUpgradeMessage,
                            false);
                    }
                    else
                    {
                        WritePageContent(response,
                            SetupResources.DatabaseSchemaUpToDateMessage,
                            false);
                    }

                    existingSiteCount = db.ExistingSiteCount();

                    WritePageContent(response,
                        string.Format(
                        SetupResources.ExistingSiteCountMessage,
                        existingSiteCount.ToString()),
                        false);

                }
                else
                {
                    WritePageContent(response,
                        SetupResources.DatabaseSchemaNotCreatedYetMessage,
                        false);
                }

            }

            if (!SetupHelper.RunningInFullTrust())
            {
                // inform of Medium trust configuration issues
                WritePageContent(response,
                    "<b>" + SetupResources.MediumTrustGeneralMessage + "</b><br />"
                    + GetDataAccessMediumTrustMessage() + "<br /><br />",
                    false);

            }
        }

        private string GetDataAccessMediumTrustMessage()
        {
            string message = string.Empty;
            string dbPlatform = db.DBPlatform;
            switch (dbPlatform)
            {
                case "MySQL":
                    message = SetupResources.MediumTrustMySQLMessage;
                    break;

                case "pgsql":
                    message = SetupResources.MediumTrustnpgsqlMessage;
                    break;


            }

            return message;

        }

        private bool CoreSystemIsReady()
        {
            bool result = true;

            if (!canAccessDatabase) return false;

            if (!db.SitesTableExists()) return false;

            if (SetupHelper.NeedsUpgrade("cloudscribe-core", db)) { return false; }




            return result;
        }

        private void SetupFeatures(HttpResponseBase response, string applicationName)
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

        private void ShowSetupSuccess(HttpResponseBase response)
        {

            StringBuilder successMessage = new StringBuilder();
            successMessage.Append("<hr /><div>" + SetupResources.SetupSuccessMessage + "</div>");
            successMessage.Append("<a href='" + "/" // TODO: was using Page.ResolveUrl("~/")
                + "' title='" + SetupResources.HomeLink + "'>"
                + SetupResources.HomeLink + "</a>");

            successMessage.Append("<br /><br />");

            successMessage.Append("<div class='settingrow'>");
            successMessage.Append("<span class='settinglabel'>");
            successMessage.Append(SetupResources.DatabasePlatform);
            successMessage.Append("</span>");
            successMessage.Append(db.DBPlatform);
            successMessage.Append("</div>");

            if (schemaHasBeenCreated)
            {
                Guid appID = db.GetOrGenerateSchemaApplicationId("cloudscribe-core");
                dbSchemaVersion = db.GetSchemaVersion(appID);

                //dbCodeVersion = DatabaseHelper.DBCodeVersion();
                //dbSchemaVersion = DatabaseHelper.DBSchemaVersion();
                if (VersionProviderManager.Providers["cloudscribe-core"] != null)
                {
                    VersionProvider coreVersionProvider = VersionProviderManager.Providers["cloudscribe-core"];
                    dbCodeVersion = coreVersionProvider.GetCodeVersion();
                }

                successMessage.Append("<div class='settingrow'>");
                successMessage.Append("<span class='settinglabel'>");
                successMessage.Append(SetupResources.Version);
                successMessage.Append("</span>");
                successMessage.Append(dbCodeVersion.ToString());
                successMessage.Append("</div>");

                successMessage.Append("<div class='settingrow'>");
                successMessage.Append("<span class='settinglabel'>");
                successMessage.Append(SetupResources.DatabaseStatus);
                successMessage.Append("</span>");

                if (dbCodeVersion > dbSchemaVersion)
                {
                    successMessage.Append(SetupResources.SchemaUpgradeNeededMessage);
                }

                if (dbCodeVersion < dbSchemaVersion)
                {
                    successMessage.Append(SetupResources.CodeUpgradeNeededMessage);
                }

                if (dbCodeVersion == dbSchemaVersion)
                {
                    successMessage.Append(SetupResources.InstallationUpToDateMessage);

                }

                successMessage.Append("</div>");
            }

            WritePageContent(response, successMessage.ToString(), false);

        }


        private void WritePageContent(HttpResponseBase response, string message)
        {
            WritePageContent(response, message, false);
        }

        private void WritePageContent(HttpResponseBase response, string message, bool showTime)
        {

            if (showTime)
            {
                response.Write(
                    string.Format("{0} - {1}",
                    message,
                    DateTime.UtcNow.Subtract(startTime)));
            }
            else
            {
                response.Write(message);
            }
            response.Write("<br/>");
            response.Flush();

        }

        private void WritePageHeader(HttpResponseBase response)
        {
            
            string setupTemplatePath = AppSettings.SetupHeaderConfigPath;
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                setupTemplatePath = AppSettings.SetupHeaderConfigPathRtl;
            }

            string fsPath = Server.MapPath(setupTemplatePath);

            if (System.IO.File.Exists(fsPath))
            {
                string sHtml = string.Empty;
                using (StreamReader oStreamReader = System.IO.File.OpenText(fsPath))
                {
                    sHtml = oStreamReader.ReadToEnd();
                }
                response.Write(sHtml);
            }

            response.Flush();
        }

        private void WritePageFooter(HttpResponseBase response)
        {
            response.Write("</body>");
            response.Write("</html>");
            response.Flush();
        }

        private bool LockForSetup()
        {

            if (HttpContext.Application["UpgradeInProgress"] != null)
            {
                bool upgradeInProgress = (bool)HttpContext.Application["UpgradeInProgress"];
                if (upgradeInProgress) return false;

            }

            HttpContext.Application["UpgradeInProgress"] = true;
            return true;
        }

        private void ClearSetupLock()
        {
            HttpContext.Application["UpgradeInProgress"] = false;
        }

    }
}
