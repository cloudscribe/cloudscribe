// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Microsoft/Joe Audette
// Created:                 2017-06-09
// Last Modified:           2017-07-18
// 


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using System.Collections;

namespace cloudscribe.Web.StaticFiles
{
    /// <summary>
    /// modified from https://github.com/aspnet/FileSystem/blob/dev/src/Microsoft.Extensions.FileProviders.Embedded/EmbeddedFileProvider.cs
    /// with changes by Joe Audette to handle embedded resources with - in the folder name
    /// - in folder names must be converted to _ but - in filename must be left as is
    /// for example /cr/js/ckeditor461/skins/moono-lisa/editor.css?t=GB8C
    /// must be changed to /cr/js/ckeditor461/skins/moono_lisa/editor.css?t=GB8C
    /// this is used in conjunction with StaticFileMiddleware to serve embedded static resources
    /// I reported the problem here: https://github.com/aspnet/FileSystem/issues/277
    /// turns out it is a duplicsate of this issue https://github.com/aspnet/FileSystem/issues/184
    /// 
    /// Looks up files using embedded resources in the specified assembly.
    /// This file provider is case sensitive.
    /// </summary>
    public class EmbeddedFileResolvingProvider : IFileProvider
    {
        private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
            .Where(c => c != '/' && c != '\\').ToArray();

        private readonly Assembly _assembly;
        private readonly string _baseNamespace;
        private readonly DateTimeOffset _lastModified;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedFileProvider" /> class using the specified
        /// assembly and empty base namespace.
        /// </summary>
        /// <param name="assembly">The assembly that contains the embedded resources.</param>
        public EmbeddedFileResolvingProvider(Assembly assembly)
            : this(assembly, assembly?.GetName()?.Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedFileProvider" /> class using the specified
        /// assembly and base namespace.
        /// </summary>
        /// <param name="assembly">The assembly that contains the embedded resources.</param>
        /// <param name="baseNamespace">The base namespace that contains the embedded resources.</param>
        public EmbeddedFileResolvingProvider(Assembly assembly, string baseNamespace)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            _baseNamespace = string.IsNullOrEmpty(baseNamespace) ? string.Empty : baseNamespace + ".";
            _assembly = assembly;

            _lastModified = DateTimeOffset.UtcNow;

            if (!string.IsNullOrEmpty(_assembly.Location))
            {
                try
                {
                    _lastModified = File.GetLastWriteTimeUtc(_assembly.Location);
                }
                catch (PathTooLongException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        // from documentation you would think that - should always be mapped to _ which we do above
        // but from observation that seems only true for folder segments
        // for files it seems sometimes I have to swap _ back to - or I get a null resourceStream

        //https://stackoverflow.com/questions/14705211/how-is-net-renaming-my-embedded-resources
        // identifiers can't start with a digit, so _ is prepended by resource builder
        // so far have not run into things named that way in my static resources
        // made this an interface so it can be easily replaced if it doesn't work for other scenarios

        public virtual string ResolveResourceIdentifier(string inputIdentifier)
        {
            if (string.IsNullOrWhiteSpace(inputIdentifier)) return inputIdentifier;

            var fileName = Path.GetFileName(inputIdentifier);

            //var identifier = inputIdentifier.Replace("/", ".");
            if (!inputIdentifier.Contains("-")) return inputIdentifier;

            // we need to not replace - from folder names but not file names

            var pathBeforeFileNaame = inputIdentifier.Replace(fileName, string.Empty).Replace("-", "_");

            return pathBeforeFileNaame + fileName;

        }

        /// <summary>
        /// Locates a file at the given path.
        /// </summary>
        /// <param name="subpath">The path that identifies the file. </param>
        /// <returns>
        /// The file information. Caller must check Exists property. A <see cref="NotFoundFileInfo" /> if the file could
        /// not be found.
        /// </returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            //JA fix for folders with - which must be converted to _
            // but file names with - should not be changed
            // /cr/js/ckeditor461/skins/moono-lisa/editor.css?t=GB8C
            // /cr/js/ckeditor461/plugins/cloudscribe-filedrop/plugin.js?t=GB8C

            subpath = ResolveResourceIdentifier(subpath);

            var builder = new StringBuilder(_baseNamespace.Length + subpath.Length);
            builder.Append(_baseNamespace);

            // Relative paths starting with a leading slash okay
            if (subpath.StartsWith("/", StringComparison.Ordinal))
            {
                builder.Append(subpath, 1, subpath.Length - 1);
            }
            else
            {
                builder.Append(subpath);
            }

            for (var i = _baseNamespace.Length; i < builder.Length; i++)
            {
                if (builder[i] == '/' || builder[i] == '\\')
                {
                    builder[i] = '.';
                }
            }

            var resourcePath = builder.ToString();
            if (HasInvalidPathChars(resourcePath))
            {
                return new NotFoundFileInfo(resourcePath);
            }

            var name = Path.GetFileName(subpath);

            if (_assembly.GetManifestResourceInfo(resourcePath) == null)
            {
                return new NotFoundFileInfo(name);
            }

            return new EmbeddedResourceFileInfo(_assembly, resourcePath, name, _lastModified);
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// This file provider uses a flat directory structure. Everything under the base namespace is considered to be one
        /// directory.
        /// </summary>
        /// <param name="subpath">The path that identifies the directory</param>
        /// <returns>
        /// Contents of the directory. Caller must check Exists property. A <see cref="NotFoundDirectoryContents" /> if no
        /// resources were found that match <paramref name="subpath" />
        /// </returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            // The file name is assumed to be the remainder of the resource name.
            if (subpath == null)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            // Relative paths starting with a leading slash okay
            if (subpath.StartsWith("/", StringComparison.Ordinal))
            {
                subpath = subpath.Substring(1);
            }

            // Non-hierarchal.
            if (!subpath.Equals(string.Empty))
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var entries = new List<IFileInfo>();

            // TODO: The list of resources in an assembly isn't going to change. Consider caching.
            var resources = _assembly.GetManifestResourceNames();
            for (var i = 0; i < resources.Length; i++)
            {
                var resourceName = resources[i];
                if (resourceName.StartsWith(_baseNamespace))
                {
                    entries.Add(new EmbeddedResourceFileInfo(
                        _assembly,
                        resourceName,
                        resourceName.Substring(_baseNamespace.Length),
                        _lastModified));
                }
            }

            return new EnumerableDirectoryContent(entries);
        }

        /// <summary>
        /// Embedded files do not change.
        /// </summary>
        /// <param name="pattern">This parameter is ignored</param>
        /// <returns>A <see cref="NullChangeToken" /></returns>
        public IChangeToken Watch(string pattern)
        {
            return NullChangeToken.Singleton;
        }

        private static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(_invalidFileNameChars) != -1;
        }
    }

    internal class EnumerableDirectoryContent : IDirectoryContents
    {
        private readonly IEnumerable<IFileInfo> _entries;

        public EnumerableDirectoryContent(IEnumerable<IFileInfo> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            _entries = entries;
        }

        public bool Exists
        {
            get { return true; }
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }
    }
}
