// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-11
// Last Modified:			2015-08-12
// 

using cloudscribe.Core.Models;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using System;
using System.IO;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SetupManager
    {

        public SetupManager(
            IApplicationEnvironment appEnv,
            ILogger<SetupManager> logger,
            ConfigHelper configuration,
            IDb dbImplementation,
            IVersionProviderFactory versionProviderFactory,
            SiteManager siteManager)
        {
            appBasePath = appEnv.ApplicationBasePath;
            this.siteManager = siteManager;
            log = logger;
            db = dbImplementation;
            config = configuration;
            this.versionProviderFactory = versionProviderFactory;

        }

        private string appBasePath;
        private SiteManager siteManager;
        private ConfigHelper config;
        private IDb db;
        private ILogger log;
        private IVersionProviderFactory versionProviderFactory;

        public string DBPlatform
        {
            get { return db.DBPlatform; }
        }

        public void EnsureDatabaseIfPossible()
        {
            if ((db.DBPlatform == "SqlCe") | (db.DBPlatform == "SQLite"))
            {
                db.EnsureDatabase();
            }

            if ((db.DBPlatform == "MSSQL") && config.TryToCreateMsSqlDatabase())
            {
                if (!db.CanAccessDatabase())
                {
                    db.EnsureDatabase();
                }

            }
        }

        public string GetOverrideConnectionString(string applicationName)
        {
            string overrideConnectionString = config.GetOrDefault("Data:" + applicationName + ":ConnectionString", string.Empty);

            if (string.IsNullOrEmpty(overrideConnectionString)) { return null; }

            return overrideConnectionString;
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

        public bool SiteTableExists()
        {
            return db.SitesTableExists();
        }

        public int ExistingSiteCount()
        {
            return db.ExistingSiteCount();
        }

        public bool NeedsUpgrade(string applicationName)
        {
            IVersionProvider provider = versionProviderFactory.Get(applicationName);

            if (provider == null) { return true; }

            Version codeVersion = provider.GetCodeVersion();

            Guid appId = db.GetOrGenerateSchemaApplicationId(applicationName);
            Version schemaVersion = db.GetSchemaVersion(appId);

            bool result = false;
            if (codeVersion > schemaVersion) { result = true; }

            return result;
        }

        public string GetPathToApplicationsFolder()
        {
            return appBasePath + "/config/applications".Replace("/", Path.DirectorySeparatorChar.ToString());
        }

        public string GetPathToInstallScriptFolder(string applicationName)
        {
            return appBasePath
               + string.Format(config.SetupInstallScriptPathFormat().Replace("/", Path.DirectorySeparatorChar.ToString()),
               applicationName,
               db.DBPlatform.ToLowerInvariant())
               ;
        }

        public string GetPathToUpgradeScriptFolder(string applicationName)
        {
            return appBasePath
                + string.Format(config.SetupUpgradeScriptPathFormat().Replace("/", Path.DirectorySeparatorChar.ToString()),
                applicationName,
                db.DBPlatform.ToLowerInvariant())
                ;
        }

        public Version GetCodeVersion(string applicationName)
        {
            IVersionProvider coreVersionProvider = versionProviderFactory.Get(applicationName);
            
            if (coreVersionProvider != null)
            {
                return coreVersionProvider.GetCodeVersion();
            }

            return null;
        }

        public Version GetCloudscribeCodeVersion()
        {
            IVersionProvider coreVersionProvider = versionProviderFactory.Get("cloudscribe-core");
            //if (VersionProviderManager.Providers["cloudscribe-core"] != null)
            if (coreVersionProvider != null)
            {
                return coreVersionProvider.GetCodeVersion();
            }

            return new Version(0, 0, 0, 0);
        }

        public Version GetCloudscribeSchemaVersion()
        {
            Guid appID = GetOrGenerateSchemaApplicationId("cloudscribe-core");
            return GetSchemaVersion(appID);
        }

        public Guid GetOrGenerateSchemaApplicationId(string applicationName)
        {
            IVersionProvider versionProvider = versionProviderFactory.Get(applicationName);
            if (versionProvider != null) { return versionProvider.ApplicationId; }

            return db.GetOrGenerateSchemaApplicationId(applicationName);
        }

        public Version GetSchemaVersion(Guid applicationId)
        {
            return db.GetSchemaVersion(applicationId);
        }

        public Version ParseVersionFromFileName(string fileName)
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

        public bool UpdateSchemaVersion(
            Guid applicationId,
            string applicationName,
            int major,
            int minor,
            int build,
            int revision)
        {
            return db.UpdateSchemaVersion(
                applicationId,
                applicationName,
                major,
                minor,
                build,
                revision);

        }

        public bool AddSchemaVersion(
          Guid applicationId,
          string applicationName,
          int major,
          int minor,
          int build,
          int revision)
        {
            return db.AddSchemaVersion(
                applicationId,
                applicationName,
                major,
                minor,
                build,
                revision);
        }

        public int AddSchemaScriptHistory(
            Guid applicationId,
            string scriptFile,
            DateTime runTime,
            bool errorOccurred,
            string errorMessage,
            string scriptBody)
        {
            return db.AddSchemaScriptHistory(applicationId, scriptFile, runTime, errorOccurred, errorMessage, scriptBody);
        }

        ///
        /// returning empty string indicates success
        /// else return error message
        public string RunScript(
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

        public static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }

    }
}
