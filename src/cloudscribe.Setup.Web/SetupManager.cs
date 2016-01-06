// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2016-01-06
// 

using cloudscribe.Core.Models.Setup;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Setup.Web
{
    public class SetupManager
    {

        public SetupManager(
            IApplicationEnvironment appEnv,
            IHttpContextAccessor contextAccessor,
            ILogger<SetupManager> logger,
            IOptions<SetupOptions> setupOptionsAccessor,
            IDbSetup dbImplementation,
            IVersionProviderFactory versionProviderFactory
            )
        {
            appBasePath = appEnv.ApplicationBasePath;
            log = logger;
            db = dbImplementation;
            this.versionProviderFactory = versionProviderFactory;
            setupOptions = setupOptionsAccessor.Value;
            _context = contextAccessor?.HttpContext;
        }

        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;
        private SetupOptions setupOptions;

        private string appBasePath;
        private IDbSetup db;
        private ILogger log;
        private IVersionProviderFactory versionProviderFactory;

        #region Public Members

        public string DBPlatform
        {
            get { return db.DBPlatform; }
        }
     
        public bool CanAccessDatabase()
        {
            return db.CanAccessDatabase();
        }

        public string GetDbConnectionError()
        {
            return db.GetConnectionError(null).ToString();
        }

        public bool CanAlterSchema()
        {
            return db.CanAlterSchema(null);
        }

        public bool SchemaTableExists()
        {
            return db.SchemaTableExists();
        }
        
        public bool NeedsUpgrade(string applicationName)
        {
            IVersionProvider provider = versionProviderFactory.Get(applicationName);

            if (provider == null)
            {
                log.LogInformation("IVersionProvider not found for " + applicationName);
                return true;
            }

            Version codeVersion = provider.CurrentVersion;
            
            Version schemaVersion = db.GetSchemaVersion(provider.ApplicationId);

            bool result = false;
            if (codeVersion > schemaVersion)
            {
                //log.LogInformation(applicationName + " needs upgrade");
                result = true;
            }

            return result;
        }
        
        public Version GetCodeVersion(string applicationName)
        {
            IVersionProvider versionProvider = versionProviderFactory.Get(applicationName);
            
            if (versionProvider != null)
            {
                return versionProvider.CurrentVersion;
            }

            return null;
        }

        public async Task<List<VersionItem>> GetInstalledSchemaList()
        {
            List<VersionItem> list = new List<VersionItem>();

            using (DbDataReader reader = await db.SchemaVersionGetAll(CancellationToken))
            {
                while(reader.Read())
                {
                    VersionItem v = new VersionItem
                    {
                        ApplicationId = new Guid(reader["ApplicationID"].ToString()),
                        Name = reader["ApplicationName"].ToString(),
                        CurrentVersion = new Version(
                            Convert.ToInt32(reader["Major"]),
                            Convert.ToInt32(reader["Minor"]),
                            Convert.ToInt32(reader["Build"]),
                            Convert.ToInt32(reader["Revision"])
                            )
                    };
                    list.Add(v);
                }
            }

                
            return list;
        }
        
        public Version GetSchemaVersion(string applicationName)
        {
            Guid appId = GetOrGenerateSchemaApplicationId(applicationName);
            Version v = GetSchemaVersion(appId);
            Version zeroVersion = new Version(0, 0, 0, 0);
            if(v > zeroVersion) { return v; }
            // if not found return null to be consistent with GetCodeVersion
            return null;
        }
        
        public Version GetSchemaVersion(Guid applicationId)
        {
            return db.GetSchemaVersion(applicationId);
        }
        

        public async Task ProbeSystem(Func<string, bool, Task> output)
        {
            await output(
                "ProbingSystem", //SetupResources.ProbingSystemMessage,
                false);

            EnsureDatabaseIfPossible();

            bool canAccessDatabase = CanAccessDatabase();

            if (canAccessDatabase)
            {
                await output(
                    DBPlatform
                    + " " + "DatabaseConnectionOk", // SetupResources.DatabaseConnectionOKMessage,
                    false);
            }
            else
            {
                string dbError = string.Format(
                    "FailedToConnectToDatabase", //SetupResources.FailedToConnectToDatabase,
                    DBPlatform);

                await output("<div>" + dbError + "</div>", false);

                bool showConnectionError = setupOptions.ShowConnectionError;

                if (showConnectionError)
                {
                    await output(
                        "<div>" + GetDbConnectionError()
                        + "</div>",
                        false);
                }
            }

            if (canAccessDatabase)
            {
                bool canAlterSchema = CanAlterSchema();

                if (canAlterSchema)
                {
                    await output(
                        "DatabaseSchemaPermissionsOk", //SetupResources.DatabaseCanAlterSchemaMessage,
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
                        await output(
                       "<div>" + "CantAlterSchemaWarning" // SetupResources.CantAlterSchemaWarning
                       + "</div>",
                       false);
                    }
                }

                bool setupSchemaHasBeenCreated = SchemaTableExists();

                if (setupSchemaHasBeenCreated)
                {
                    await output(
                        "SetupSystemSchemaExists", //SetupResources.DatabaseSchemaAlreadyExistsMessage,
                        false);

                    bool needSetupSchemaUpgrade = NeedsUpgrade("cloudscribe-setup");

                    if (needSetupSchemaUpgrade)
                    {
                        await output(
                            "SetupSystemSchemaNeedsUpgrade", //SetupResources.DatabaseSchemaNeedsUpgradeMessage,
                            false);
                    }
                    else
                    {
                        await output(
                            "SetupSystemSchemaUpToDate", //SetupResources.DatabaseSchemaUpToDateMessage,
                            false);
                    }



                }
                else
                {
                    await output(
                        "SetupSchemaNotCreatedYet", //SetupResources.DatabaseSchemaNotCreatedYetMessage,
                        false);
                }

            }
        }

        public async Task<bool> SetupCloudscribeSetup(Func<string, bool, Task> output)
        {
            bool setupSchemaHasBeenCreated = false;
            if (!SchemaTableExists())
            {
                setupSchemaHasBeenCreated = await CreateInitialSchema(output, "cloudscribe-setup");
            }

            bool needSetupSchemaUpgrade = NeedsUpgrade("cloudscribe-setup");

            if (needSetupSchemaUpgrade)
            {
                needSetupSchemaUpgrade = await UpgradeSchema(output, "cloudscribe-setup");
            }


            return SchemaTableExists() && !needSetupSchemaUpgrade;
        }

        public async Task<bool> SetupOtherApplications(Func<string, bool, Task> output)
        {
            bool result = true;

            string pathToApplicationsFolder = GetPathToApplicationsFolder();

            if (!Directory.Exists(pathToApplicationsFolder))
            {
                await output(
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
                    await CreateInitialSchema(output, appFolder.Name);
                    await UpgradeSchema(output, appFolder.Name);

                }

            }

            return result;

        }

        #endregion

        #region Private Methods

        private async Task<bool> CreateInitialSchema(Func<string, bool, Task> output, string applicationName)
        {
            Guid appId = GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = GetSchemaVersion(appId);
            Version zeroVersion = new Version(0, 0, 0, 0);
            if (currentSchemaVersion == null)
            {
                currentSchemaVersion = new Version(0, 0, 0, 0);
            }

            if (currentSchemaVersion > zeroVersion) { return true; } //already installed only run upgrade scripts

            Version versionToStopAt = null; // null because we don't stop on install we start with the highest version in the folder which is also the last one

            string pathToScriptFolder = GetPathToInstallScriptFolder(applicationName);

            if (!Directory.Exists(pathToScriptFolder))
            {
                await output(
                pathToScriptFolder + " "
                + "ScriptFolderNotFoundAddendum", //SetupResources.ScriptFolderNotFoundAddendum,
                false);

                return false;

            }


            bool result = true;

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.sql");
            Array.Sort(scriptFiles, SetupManager.CompareFileNames);


            if (scriptFiles.Length == 0)
            {
                await output(
                "NoScriptsFilesFound" //SetupResources.NoScriptsFilesFoundMessage
                + " " + pathToScriptFolder,
                false);

                return false;

            }

            // We only want to run the highest version script from the install folder
            // normally there is only 1 script in this folder, but if someone upgrades and then starts with a clean db
            // there can be more than one script because of the previous installs so we need to make sure we only run the highest version found
            // since we sorted it the highest version is the last item in the array
            FileInfo highestVersionScriptFile = scriptFiles[(scriptFiles.Length - 1)];

            //Version currentSchemaVersion = setupManager.GetSchemaVersion(applicationId);
            Version scriptVersion = ParseVersionFromFileName(highestVersionScriptFile.Name);

            if (
                (scriptVersion != null)
                && (scriptVersion > currentSchemaVersion)
                && (versionToStopAt == null || (scriptVersion <= versionToStopAt))
                )
            {
                result = await ProcessScript(
                    output,
                    highestVersionScriptFile,
                    appId,
                    applicationName,
                    currentSchemaVersion);
            }



            return result;

        }

        private async Task<bool> UpgradeSchema(Func<string, bool, Task> output, string applicationName)
        {

            Version versionToStopAt = GetCodeVersion(applicationName);
            Guid appID = GetOrGenerateSchemaApplicationId(applicationName);
            Version currentSchemaVersion = GetSchemaVersion(appID);
            if (currentSchemaVersion == null)
            {
                currentSchemaVersion = new Version(0, 0, 0, 0);
            }

            if (versionToStopAt != null)
            {
                if (versionToStopAt <= currentSchemaVersion) { return false; }

            }

            string pathToScriptFolder = GetPathToUpgradeScriptFolder(applicationName);


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

                await output(warning, false);

                return false;

            }

            bool result = await RunUpgradeScripts(
                output,
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

            return result;

        }

        private async Task<bool> RunUpgradeScripts(
            Func<string, bool, Task> output,
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                await output(
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

            Version currentSchemaVersion = GetSchemaVersion(applicationId);

            if (currentSchemaVersion == null)
            {
                currentSchemaVersion = new Version(0, 0, 0, 0);
            }

            foreach (FileInfo scriptFile in scriptFiles)
            {
                Version scriptVersion = ParseVersionFromFileName(scriptFile.Name);

                if (
                    (scriptVersion != null)
                    && (scriptVersion > currentSchemaVersion)
                    && (versionToStopAt == null || (scriptVersion <= versionToStopAt))
                    )
                {
                    result = await ProcessScript(
                        output,
                        scriptFile,
                        applicationId,
                        applicationName,
                        currentSchemaVersion);

                    if (!result) { return false; }
                }

            }

            return result;

        }

        private async Task<bool> ProcessScript(
            Func<string, bool, Task> output,
            FileInfo scriptFile,
            Guid applicationId,
            string applicationName,
            Version currentSchemaVersion)
        {
            string message = string.Format(
                        "RunningScript {0} {1}", //SetupResources.RunningScriptMessage,
                        applicationName,
                        scriptFile.Name.Replace(".sql", string.Empty));

            await output(message, true);

            string overrideConnectionString = GetOverrideConnectionString(applicationName);

            string errorMessage
                = RunScript(
                    applicationId,
                    scriptFile,
                    overrideConnectionString);

            if (errorMessage.Length > 0)
            {
                await output(errorMessage, true);
                return false;

            }

            Version newVersion = ParseVersionFromFileName(scriptFile.Name);

            if (
                (applicationName != null)
                && (newVersion != null)
                )
            {
                UpdateSchemaVersion(
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

       
        /// returning empty string indicates success
        /// else return error message
        private string RunScript(
            Guid applicationId,
            FileInfo scriptFile,
            string overrideConnectionString)
        {
            // returning empty string indicates success
            // else return error message
            string resultMessage = string.Empty;

            if (scriptFile == null) return resultMessage;

            try
            {
                bool result = db.RunScript(scriptFile, overrideConnectionString);

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

        private string GetPathToApplicationsFolder()
        {
            var appVPath = appBasePath + setupOptions.ConfigBasePath + "/applications";
            return appVPath.Replace("/", Path.DirectorySeparatorChar.ToString());
        }

        private string GetPathToInstallScriptFolder(string applicationName)
        {
            return appBasePath
               + string.Format(setupOptions.InstallScriptPathFormat.Replace("/", Path.DirectorySeparatorChar.ToString()),
               applicationName,
               db.DBPlatform.ToLowerInvariant())
               ;
        }

        private string GetPathToUpgradeScriptFolder(string applicationName)
        {
            return appBasePath
                + string.Format(setupOptions.UpgradeScriptPathFormat.Replace("/", Path.DirectorySeparatorChar.ToString()),
                applicationName,
                db.DBPlatform.ToLowerInvariant())
                ;
        }

        private static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }



        private void EnsureDatabaseIfPossible()
        {
            if ((db.DBPlatform == "SqlCe") | (db.DBPlatform == "SQLite"))
            {
                db.EnsureDatabase();
            }

            if ((db.DBPlatform == "MSSQL") && setupOptions.TryToCreateMsSqlDatabase)
            {
                if (!db.CanAccessDatabase())
                {
                    db.EnsureDatabase();
                }

            }
        }

        private Version ParseVersionFromFileName(string fileName)
        {
            Version version = null;

            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;
            bool success = true;

            if (fileName != null)
            {
                char[] separator = { '.' };
                string[] args = fileName.Replace(".sql", String.Empty).Split(separator);
                if (args.Length >= 4)
                {
                    if (!(int.TryParse(args[0], out major)))
                    {
                        major = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[1], out minor)))
                    {
                        minor = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[2], out build)))
                    {
                        build = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[3], out revision)))
                    {
                        revision = 0;
                        success = false;
                    }

                    if (success)
                    {
                        version = new Version(major, minor, build, revision);
                    }

                }

            }


            return version;

        }

        private bool UpdateSchemaVersion(
            Guid applicationId,
            string applicationName,
            int major,
            int minor,
            int build,
            int revision)
        {
            if (!db.UpdateSchemaVersion(
                applicationId,
                applicationName,
                major,
                minor,
                build,
                revision))
            {
                return db.AddSchemaVersion(
                applicationId,
                applicationName,
                major,
                minor,
                build,
                revision);
            }

            return true;

        }

        private Guid GetOrGenerateSchemaApplicationId(string applicationName)
        {
            IVersionProvider versionProvider = versionProviderFactory.Get(applicationName);
            if (versionProvider != null) { return versionProvider.ApplicationId; }

            return db.GetOrGenerateSchemaApplicationId(applicationName);
        }

        private string GetOverrideConnectionString(string applicationName)
        {
            return null;
            //string overrideConnectionString = config.GetOrDefault("Data:" + applicationName + ":ConnectionString", string.Empty);

            //if (string.IsNullOrEmpty(overrideConnectionString)) { return null; }

            //return overrideConnectionString;
        }

        #endregion

    }
}
