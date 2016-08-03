// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-03
// Last Modified:			2016-08-03
// 

using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    /// <summary>
    /// this is for querying of a type across NoDb projects, ie in multiple project folders
    /// the methods are overrides so the method signatures take a projectid,
    /// but it is ignored and empty string or null can be passed in
    /// we want to use a nodb project per site in cloudscribe core so for certain things like
    /// SiteSettings and SiteHosts we need to query across projects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CrossProjectQueries<T> : BasicQueries<T> where T : class
    {
        public CrossProjectQueries(
            ILogger<BasicQueries<T>> logger,
            IStoragePathResolver<T> pathResolver,
            IStringSerializer<T> serializer,
            IStoragePathOptionsResolver storageOptionsResolver
            ) : base(logger, pathResolver, serializer)
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


        public override async Task<T> FetchAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            //override logic to look across site folders

            var projectsFolder = await GetProjectsFolderPath().ConfigureAwait(false);

            if (!Directory.Exists(projectsFolder))
            {
                return null;
            }

            var typeName = typeof(T).Name.ToLowerInvariant().Trim();
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

            return null;

        }

        public override async Task<int> GetCountAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            //override logic to look across site folders
            var count = 0;

            var projectsFolder = await GetProjectsFolderPath().ConfigureAwait(false);
            if (!Directory.Exists(projectsFolder))
            {
                return count;
            }

            var typeName = typeof(T).Name.ToLowerInvariant().Trim();
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
            
        }


        public override async Task<IEnumerable<T>> GetAllAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath(projectId).ConfigureAwait(false);

            var list = new List<T>();

            //override logic to look across site folders

            var projectsFolder = await GetProjectsFolderPath().ConfigureAwait(false);
            if (!Directory.Exists(projectsFolder))
            {
                return list;
            }

            var typeName = typeof(T).Name.ToLowerInvariant().Trim();
            var projectsDir = new DirectoryInfo(projectsFolder);
            foreach (var projectDir in projectsDir.GetDirectories())
            {
                var typeFolderPath = Path.Combine(projectDir.FullName, typeName);
                if (Directory.Exists(typeFolderPath))
                {
                    foreach (string file in Directory.EnumerateFiles(
                        typeFolderPath,
                        "*" + serializer.ExpectedFileExtension,
                        SearchOption.AllDirectories) 
                        )
                    {
                        var key = Path.GetFileNameWithoutExtension(file);
                        var obj = LoadObject(file, key);
                        list.Add(obj);
                    }

                }

            }

            return list;

        }


    }
}
