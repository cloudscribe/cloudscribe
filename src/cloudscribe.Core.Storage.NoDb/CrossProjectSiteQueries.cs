// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-02
// Last Modified:			2016-08-02
// 


using cloudscribe.Core.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class CrossProjectSiteQueries : BasicQueries<SiteSettings>
    {
        /// <summary>
        /// here we are not using the proejctid if any passed into the methods, since the goal is to search across projects
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="storageOptionsResolver"></param>
        /// <param name="pathResolver"></param>
        /// <param name="serializer"></param>
        public CrossProjectSiteQueries(
            ILogger<BasicQueries<SiteSettings>> logger,
            IStoragePathOptionsResolver storageOptionsResolver,
            IStoragePathResolver<SiteSettings> pathResolver,
            IStringSerializer<SiteSettings> serializer)
            :base(logger, pathResolver, serializer)
        {
            this.storageOptionsResolver = storageOptionsResolver;
        }

        private IStoragePathOptionsResolver storageOptionsResolver;

        private async Task<string> GetProjectsFolderPath()
        {
            var pathOptions = await storageOptionsResolver.Resolve("").ConfigureAwait(false);
            var projectsFolder = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator)
                + pathOptions.FolderSeparator
                + pathOptions.ProjectsFolderName
                ;

            return projectsFolder;
        }

        public override async Task<SiteSettings> FetchAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            //override logic to look across site folders
            
            var projectsFolder = await GetProjectsFolderPath().ConfigureAwait(false);

            if (!Directory.Exists(projectsFolder))
            {
                return null;
            }

            var typeName = typeof(SiteSettings).Name.ToLowerInvariant().Trim();
            var projectsDir = new DirectoryInfo(projectsFolder);
            foreach (var projectDir in projectsDir.GetDirectories())
            {
                var typeFolderPath = Path.Combine(projectDir.FullName, typeName);
                if (Directory.Exists(typeFolderPath))
                {
                    var pathToFile = Path.Combine(typeFolderPath, key + serializer.ExpectedFileExtension);
                    if (File.Exists(pathToFile))
                    {
                        return LoadObject(pathToFile, key);
                    }
                }

            }



            //var pathToFile = await pathResolver.ResolvePath(
            //projectId,
            //key,
            //serializer.ExpectedFileExtension
            //).ConfigureAwait(false);

            //if (!File.Exists(pathToFile)) return null;

            //return LoadObject(pathToFile, key);

            return null;

        }

        public override async Task<int> GetCountAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            //override logic to look across site folders
            var count = 0;

            var projectsFolder = await GetProjectsFolderPath().ConfigureAwait(false);
            if (!Directory.Exists(projectsFolder))
            {
                return count;
            }

            var typeName = typeof(SiteSettings).Name.ToLowerInvariant().Trim();
            var projectsDir = new DirectoryInfo(projectsFolder);
            foreach (var projectDir in projectsDir.GetDirectories())
            {
                var typeFolderPath = Path.Combine(projectDir.FullName, typeName);
                if (Directory.Exists(typeFolderPath))
                {
                    var typeDirectory = new DirectoryInfo(typeFolderPath);
                    count += typeDirectory.GetFileSystemInfos("*" + serializer.ExpectedFileExtension).Length;

                }

            }

            return count;

            //var pathToFolder = await pathResolver.ResolvePath(projectId).ConfigureAwait(false);
            //if (!Directory.Exists(pathToFolder)) return 0;

            //var directory = new DirectoryInfo(pathToFolder);
            //return directory.GetFileSystemInfos("*" + serializer.ExpectedFileExtension).Length;
        }


        public override async Task<IEnumerable<SiteSettings>> GetAllAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var list = new List<SiteSettings>();

            //override logic to look across site folders

            var projectsFolder = await GetProjectsFolderPath().ConfigureAwait(false);
            if (!Directory.Exists(projectsFolder))
            {
                return list;
            }

            var typeName = typeof(SiteSettings).Name.ToLowerInvariant().Trim();
            var projectsDir = new DirectoryInfo(projectsFolder);
            foreach (var projectDir in projectsDir.GetDirectories())
            {
                var typeFolderPath = Path.Combine(projectDir.FullName, typeName);
                if (Directory.Exists(typeFolderPath))
                {
                    foreach (string file in Directory.EnumerateFiles(
                        typeFolderPath,
                        "*" + serializer.ExpectedFileExtension,
                        SearchOption.AllDirectories) // this is needed for blog posts which are nested in year/month folders
                        )
                    {
                        var key = Path.GetFileNameWithoutExtension(file);
                        var obj = LoadObject(file, key);
                        list.Add(obj);
                    }

                }

            }


            //var pathToFolder = await pathResolver.ResolvePath(projectId).ConfigureAwait(false);
            
            
            //if (!Directory.Exists(pathToFolder)) return list;
            //foreach (string file in Directory.EnumerateFiles(
            //    pathToFolder,
            //    "*" + serializer.ExpectedFileExtension,
            //    SearchOption.AllDirectories) // this is needed for blog posts which are nested in year/month folders
            //    )
            //{
            //    var key = Path.GetFileNameWithoutExtension(file);
            //    var obj = LoadObject(file, key);
            //    list.Add(obj);
            //}

            return list;

        }

    }
}
