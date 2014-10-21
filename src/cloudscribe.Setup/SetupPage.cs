// Author:					Joe Audette
// Created:				    2006-09-30
// Last Modified:		    2014-10-21
//
// You must not remove this notice, or any other, from this software.

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web;
using cloudscribe.Resources;
using log4net;
using System;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject;


namespace cloudscribe.Setup
{
    /// <summary>
    /// This is the setup page for initial installation and upgrades.
    /// It can create an initial site if none exists, run upgrade scripts for the core and features and configure 
    /// default settings or add new settings to features.
    /// </summary>
    public class SetupPage : Page
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(SetupPage));

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

        


        protected void Page_Load(object sender, EventArgs e)
        {
            scriptTimeout = Server.ScriptTimeout;
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);

            IOwinContext owinContext = Context.GetOwinContext();
            StandardKernel ninjectKernel = owinContext.Get<StandardKernel>();
            siteRepository = ninjectKernel.Get<ISiteRepository>();
            userRepository = ninjectKernel.Get<IUserRepository>();
            db = ninjectKernel.Get<IDb>();

            // TODO: pass in by dependency injection?
            //db = new cloudscribe.DbHelpers.MSSQL.Db();
            // TODO: dependency injection
            //siteRepository = SiteContext.GetSiteRepository();
            //userRepository = SiteContext.GetUserRepository();
            


            setupIsDisabled = AppSettings.DisableSetup;

            Server.ScriptTimeout = int.MaxValue;
            startTime = DateTime.UtcNow;
            bool isAdmin = false;
            try
            {
                //isAdmin = WebUser.IsAdmin;
            }
            catch { }

            WritePageHeader();

            if (setupIsDisabled && !isAdmin)
            {
                WritePageContent(SetupResources.SetupDisabledMessage);
            }
            else
            {
                if (setupIsDisabled && isAdmin)
                {
                    WritePageContent(SetupResources.RunningSetupForAdminUser);

                }

                if (LockForSetup())
                {
                    try
                    {
                        ProbeSystem();
                        RunSetup();

                        if (CoreSystemIsReady())
                        {
                            ShowSetupSuccess();
                        }
                    }
                    finally
                    {
                        ClearSetupLock();
                    }

                }
                else
                {
                    WritePageContent(SetupResources.SetupAlreadyInProgress);
                }

                WritePageContent(SetupResources.SetupEnabledMessage);


            }

            WritePageFooter();

            //restore Script timeout
            Server.ScriptTimeout = scriptTimeout;

        }

        private void RunSetup()
        {
            #region setup cloudscribe-core

            if (!schemaHasBeenCreated)
            {
                if (canAlterSchema)
                {

                    CreateInitialSchema("cloudscribe-core");
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
                needSchemaUpgrade = UpgradeSchema("cloudscribe-core");

            }

            if (!CoreSystemIsReady()) return;

            //existingSiteCount = DatabaseHelper.ExistingSiteCount();
            if (existingSiteCount == 0)
            {
                CreateSiteAndAdminUser();
            }


            // look for new features or settings to install
            //SetupFeatures("mojoportal-core");


            #endregion

            #region setup other applications

            // install other apps

            String pathToApplicationsFolder
                = HttpContext.Current.Server.MapPath(
                "~/Config/applications/");

            if (!Directory.Exists(pathToApplicationsFolder))
            {
                WritePageContent(
                pathToApplicationsFolder
                + " " + SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return;
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
                    CreateInitialSchema(appFolder.Name);
                    UpgradeSchema(appFolder.Name);
                    SetupFeatures(appFolder.Name);
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

        }

        //private static void SyncDefinitions(object o)
        //{
        //    ModuleDefinition.SyncDefinitions();
        //}

        private bool CreateInitialSchema(string applicationName)
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
                = HttpContext.Current.Server.MapPath(
                "~/Config/applications/" + applicationName
                + "/SchemaInstallScripts/"
                    + db.DBPlatform.ToLowerInvariant()
                    + "/");

            if (!Directory.Exists(pathToScriptFolder))
            {
                WritePageContent(
                pathToScriptFolder + " " + SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;

            }

            return RunSetupScript(
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

        }

        private bool RunSetupScript(
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                WritePageContent(
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
                WritePageContent(
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

                WritePageContent(
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
                    WritePageContent(errorMessage, true);
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
                    if(!db.UpdateSchemaVersion(
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

        private bool UpgradeSchema(string applicationName)
        {
            Guid appID = db.GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = db.GetSchemaVersion(appID);
            Version versionToStopAt = null;
            
            if(VersionProviderManager.Providers[applicationName] != null)
            {
                VersionProvider appVersionProvider = VersionProviderManager.Providers[applicationName];
                versionToStopAt = appVersionProvider.GetCodeVersion();
                
            }

            String pathToScriptFolder
                = HttpContext.Current.Server.MapPath(
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
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

            return result;

        }

        private bool RunUpgradeScripts(
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                WritePageContent(
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

                    WritePageContent(
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
                        WritePageContent(errorMessage, true);
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

        private void CreateSiteAndAdminUser()
        {
            WritePageContent(SetupResources.CreatingSiteMessage, true);
            SiteSettings newSite = SetupHelper.CreateNewSite(siteRepository);
            //mojoSetup.CreateDefaultSiteFolders(newSite.SiteId);
            //mojoSetup.CreateOrRestoreSiteSkins(newSite.SiteId);
            WritePageContent(SetupResources.CreatingRolesAndAdminUserMessage, true);
            SetupHelper.CreateRequiredRolesAndAdminUser(
                newSite,
                siteRepository,
                userRepository
                );

        }



        private void SetupFeatures(string applicationName)
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

        //private void SetupFeature(ContentFeature feature)
        //{
        //    WritePageContent(
        //            string.Format(SetupResource.ConfigureFeatureMessage,
        //            ResourceHelper.GetResourceString(
        //            feature.ResourceFile,
        //            feature.FeatureNameReasourceKey))
        //            , true);

        //    ModuleDefinition moduleDefinition = new ModuleDefinition(feature.FeatureGuid);
        //    moduleDefinition.ControlSrc = feature.ControlSource;
        //    moduleDefinition.DefaultCacheTime = feature.DefaultCacheTime;
        //    moduleDefinition.FeatureName = feature.FeatureNameReasourceKey;
        //    moduleDefinition.Icon = feature.Icon;
        //    moduleDefinition.IsAdmin = feature.ExcludeFromFeatureList;
        //    moduleDefinition.SortOrder = feature.SortOrder;
        //    moduleDefinition.ResourceFile = feature.ResourceFile;
        //    moduleDefinition.IsCacheable = feature.IsCacheable;
        //    moduleDefinition.IsSearchable = feature.IsSearchable;
        //    moduleDefinition.SearchListName = feature.SearchListNameResourceKey;
        //    moduleDefinition.SupportsPageReuse = feature.SupportsPageReuse;
        //    moduleDefinition.DeleteProvider = feature.DeleteProvider;
        //    moduleDefinition.PartialView = feature.PartialView;
        //    moduleDefinition.Save();

        //    foreach (ContentFeatureSetting featureSetting in feature.Settings)
        //    {

        //        ModuleDefinition.UpdateModuleDefinitionSetting(
        //            moduleDefinition.FeatureGuid,
        //            moduleDefinition.ModuleDefId,
        //            featureSetting.ResourceFile,
        //            featureSetting.GroupNameKey,
        //            featureSetting.ResourceKey,
        //            featureSetting.DefaultValue,
        //            featureSetting.ControlType,
        //            featureSetting.RegexValidationExpression,
        //            featureSetting.ControlSrc,
        //            featureSetting.HelpKey,
        //            featureSetting.SortOrder);

        //    }



        //}



        private void ShowSetupSuccess()
        {

            StringBuilder successMessage = new StringBuilder();
            successMessage.Append("<hr /><div>" + SetupResources.SetupSuccessMessage + "</div>");
            successMessage.Append("<a href='" + Page.ResolveUrl("~/")
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

            WritePageContent(successMessage.ToString(), false);

        }


        private void WritePageContent(string message)
        {
            WritePageContent(message, false);
        }

        private void WritePageContent(string message, bool showTime)
        {

            if (showTime)
            {
                HttpContext.Current.Response.Write(
                    string.Format("{0} - {1}",
                    message,
                    DateTime.UtcNow.Subtract(startTime)));
            }
            else
            {
                HttpContext.Current.Response.Write(message);
            }
            HttpContext.Current.Response.Write("<br/>");
            HttpContext.Current.Response.Flush();

        }


        private void WritePageHeader()
        {
            if (HttpContext.Current == null) return;

            string setupTemplatePath = AppSettings.SetupHeaderConfigPath;
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                setupTemplatePath = AppSettings.SetupHeaderConfigPathRtl;
            }

            string fsPath = HttpContext.Current.Server.MapPath(setupTemplatePath);

            if (File.Exists(fsPath))
            {
                string sHtml = string.Empty;
                using (StreamReader oStreamReader = File.OpenText(fsPath))
                {
                    sHtml = oStreamReader.ReadToEnd();
                }
                Response.Write(sHtml);
            }

            Response.Flush();
        }

        private void WritePageFooter()
        {
            Response.Write("</body>");
            Response.Write("</html>");
            Response.Flush();
        }


        private void ProbeSystem()
        {
            WritePageContent(
                SetupResources.ProbingSystemMessage,
                false);

            dbPlatform = db.DBPlatform;
            //dataFolderIsWritable = mojoSetup.DataFolderIsWritable();

            //if (dbPlatform == "SqlCe")
            //{
            //    DatabaseHelper.EnsureDatabase();
            //}

            if (dataFolderIsWritable)
            {
                WritePageContent(
                    SetupResources.FileSystemPermissionsOKMesage,
                    false);
            }
            else
            {
                WritePageContent(
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
                WritePageContent(dbPlatform + " " + SetupResources.TryingToCreateDatabase, false);
                db.EnsureDatabase();
                canAccessDatabase = db.CanAccessDatabase();
                if (canAccessDatabase)
                {
                    WritePageContent(dbPlatform + " " + SetupResources.DatabaseCreationSucceeded, false);
                }

            }


            if (canAccessDatabase)
            {
                WritePageContent(
                    dbPlatform
                    + " " + SetupResources.DatabaseConnectionOKMessage,
                    false);
            }
            else
            {
                string dbError = string.Format(
                    SetupResources.FailedToConnectToDatabase,
                    dbPlatform);

                WritePageContent("<div>" + dbError + "</div>", false);

                showConnectionError = AppSettings.GetBool("ShowConnectionErrorOnSetup", false);


                if (showConnectionError)
                {
                    WritePageContent(
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
                    WritePageContent(
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
                        WritePageContent(
                       "<div>" + SetupResources.CantAlterSchemaWarning
                       + "</div>",
                       false);
                    }
                }

                schemaHasBeenCreated = db.SitesTableExists();

                if (schemaHasBeenCreated)
                {
                    WritePageContent(
                        SetupResources.DatabaseSchemaAlreadyExistsMessage,
                        false);


                    needSchemaUpgrade = SetupHelper.NeedsUpgrade("cloudscribe-core", db);

                    if (needSchemaUpgrade)
                    {
                        WritePageContent(
                            SetupResources.DatabaseSchemaNeedsUpgradeMessage,
                            false);
                    }
                    else
                    {
                        WritePageContent(
                            SetupResources.DatabaseSchemaUpToDateMessage,
                            false);
                    }

                    existingSiteCount = db.ExistingSiteCount();

                    WritePageContent(
                        string.Format(
                        SetupResources.ExistingSiteCountMessage,
                        existingSiteCount.ToString()),
                        false);

                }
                else
                {
                    WritePageContent(
                        SetupResources.DatabaseSchemaNotCreatedYetMessage,
                        false);
                }

            }

            if (!SetupHelper.RunningInFullTrust())
            {
                // inform of Medium trust configuration issues
                WritePageContent(
                    "<b>" + SetupResources.MediumTrustGeneralMessage + "</b><br />"
                    + GetDataAccessMediumTrustMessage() + "<br /><br />",
                    false);

            }
        }

        private bool CoreSystemIsReady()
        {
            bool result = true;

            if (!canAccessDatabase) return false;

            if (!db.SitesTableExists()) return false;

            if (SetupHelper.NeedsUpgrade("cloudscribe-core", db)) { return false; }




            return result;
        }

        private bool LockForSetup()
        {
            if (Application["UpgradeInProgress"] != null)
            {
                bool upgradeInProgress = (bool)Application["UpgradeInProgress"];
                if (upgradeInProgress) return false;

            }

            Application["UpgradeInProgress"] = true;
            return true;
        }

        private void ClearSetupLock()
        {
            Application["UpgradeInProgress"] = false;
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

        private string GetLuceneMediumTrustMessage()
        {
            string result = SetupResources.MediumTrustLuceneConfigPreambleMessage
                    + "<br /><br />"
                    + Server.HtmlEncode(GetLuceneExampleMediumTrustConfig())
                    + "<br /><br />";

            return result;

        }

        private string GetLuceneExampleMediumTrustConfig()
        {
            string example = "<add key=\"Lucene.Net.lockdir\" value=\""
                + GetPathToIndexFolder() + "\" />";

            return example;

        }

        private string GetPathToIndexFolder()
        {
            String result = Server.MapPath("~/files/Sites/1/index");
            return result;

        }

        //private string GetFolderDetailsHtml()
        //{
        //    StringBuilder folderErrors = new StringBuilder();
        //    string crlf = "\r\n";
        //    folderErrors.Append(
        //        SetupResource.DataFolderNotWritableMessage.Replace(crlf, "<br />")
        //        + "<h3>" + SetupResource.FolderDetailsLabel + "</h3>");

        //    String pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/test.config");
        //    try
        //    {
        //        mojoSetup.TouchTestFile(pathToTestFile);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        folderErrors.Append("<li>" + SetupResource.DataRootNotWritableMessage + "</li>");
        //    }

        //    pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/test.config");
        //    try
        //    {
        //        mojoSetup.TouchTestFile(pathToTestFile);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        folderErrors.Append("<li>" + SetupResource.DataSiteFolderNotWritableMessage + "</li>");
        //    }

        //    pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/systemfiles/test.config");
        //    try
        //    {
        //        mojoSetup.TouchTestFile(pathToTestFile);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        folderErrors.Append("<li>" + SetupResource.DataSystemFilesFolderNotWritableMessage + "</li>");
        //    }

        //    pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/index/test.config");
        //    try
        //    {
        //        mojoSetup.TouchTestFile(pathToTestFile);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        folderErrors.Append("<li>" + SetupResource.DataSiteIndexFolderNotWritableMessage + "</li>");
        //    }

        //    pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/test.config");
        //    try
        //    {
        //        mojoSetup.TouchTestFile(pathToTestFile);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        folderErrors.Append("<li>" + SetupResource.DataSharedFilesFolderNotWritableMessage + "</li>");
        //    }

        //    pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/History/test.config");
        //    try
        //    {
        //        mojoSetup.TouchTestFile(pathToTestFile);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        folderErrors.Append("<li>" + SetupResource.DataSharedFilesHistoryFolderNotWritableMessage + "</li>");

        //    }

        //    return folderErrors.ToString();

        //}


        void SetupPage_Error(object sender, EventArgs e)
        {
            Exception rawException = Server.GetLastError();
            Server.ClearError();
            Response.Clear();
            Response.Write(SetupHelper.BuildHtmlErrorPage(rawException));
            Response.End();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.Error += new EventHandler(SetupPage_Error);

        }

    }
}
