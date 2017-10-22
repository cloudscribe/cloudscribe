using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders.Composite;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;

namespace Microsoft.Extensions.Configuration // so we don't need another using in startup
{
    /// <summary>
    /// The idea here is to be able to serve pre-gzipped js and css
    /// I use webpack to pre gzip js and css bundles for production
    /// If there is a request for a js or css file this will check if there exists the same file with .gz
    /// and if so that will be returned instead.
    /// 
    /// Usage:
    ///   app.UseStaticFiles(new StaticFileOptions()
    ///   {
    ///       OnPrepareResponse = GzipMappingFileProvider.OnPrepareResponse,
    ///       FileProvider = new GzipMappingFileProvider(Environment.WebRootFileProvider)
    ///   });
    /// 
    /// forked from https://github.com/aspnet/FileSystem/blob/dev/src/FS.Composite/CompositeFileProvider.cs
    /// </summary>
    public class GzipMappingFileProvider : IFileProvider
    {
        private readonly IFileProvider[] _fileProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeFileProvider" /> class using a collection of file provider.
        /// </summary>
        /// <param name="fileProviders">The collection of <see cref="IFileProvider" /></param>
        public GzipMappingFileProvider(params IFileProvider[] fileProviders)
        {
            _fileProviders = fileProviders ?? new IFileProvider[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeFileProvider" /> class using a collection of file provider.
        /// </summary>
        /// <param name="fileProviders">The collection of <see cref="IFileProvider" /></param>
        public GzipMappingFileProvider(IEnumerable<IFileProvider> fileProviders)
        {
            if (fileProviders == null)
            {
                throw new ArgumentNullException(nameof(fileProviders));
            }
            _fileProviders = fileProviders.ToArray();
        }

        /// <summary>
        /// Locates a file at the given path.
        /// </summary>
        /// <param name="subpath">The path that identifies the file. </param>
        /// <returns>The file information. Caller must check Exists property. This will be the first existing <see cref="IFileInfo"/> returned by the provided <see cref="IFileProvider"/> or a not found <see cref="IFileInfo"/> if no existing files is found.</returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            foreach (var fileProvider in _fileProviders)
            {
                var fileInfo = fileProvider.GetFileInfo(subpath);
                if (fileInfo != null && fileInfo.Exists)
                {
                    if (subpath.EndsWith(".min.css") || subpath.EndsWith(".min.js"))
                    {
                        // check if .gz version of the file exists and if so return that
                        //https://stackoverflow.com/questions/11653488/serving-gzipped-content-directly-bad-thing-to-do
                        var gzfileInfo = fileProvider.GetFileInfo(subpath + ".gz");
                        if (gzfileInfo != null && gzfileInfo.Exists)
                        {
                            return gzfileInfo;
                        }

                    }

                    return fileInfo;
                }
            }
            return new NotFoundFileInfo(subpath);
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// </summary>
        /// <param name="subpath">The path that identifies the directory</param>
        /// <returns>Contents of the directory. Caller must check Exists property.
        /// The content is a merge of the contents of the provided <see cref="IFileProvider"/>.
        /// When there is multiple <see cref="IFileInfo"/> with the same Name property, only the first one is included on the results.</returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var directoryContents = new CompositeDirectoryContents(_fileProviders, subpath);
            return directoryContents;
        }

        /// <summary>
        /// Creates a <see cref="IChangeToken"/> for the specified <paramref name="pattern"/>.
        /// </summary>
        /// <param name="pattern">Filter string used to determine what files or folders to monitor. Example: **/*.cs, *.*, subFolder/**/*.cshtml.</param>
        /// <returns>An <see cref="IChangeToken"/> that is notified when a file matching <paramref name="pattern"/> is added, modified or deleted.
        /// The change token will be notified when one of the change token returned by the provided <see cref="IFileProvider"/> will be notified.</returns>
        public IChangeToken Watch(string pattern)
        {
            // Watch all file providers
            var changeTokens = new List<IChangeToken>();
            foreach (var fileProvider in _fileProviders)
            {
                var changeToken = fileProvider.Watch(pattern);
                if (changeToken != null)
                {
                    changeTokens.Add(changeToken);
                }
            }

            // There is no change token with active change callbacks
            if (changeTokens.Count == 0)
            {
                return NullChangeToken.Singleton;
            }

            return new CompositeChangeToken(changeTokens);
        }

        /// <summary>
        /// Gets the list of configured <see cref="IFileProvider" /> instances.
        /// </summary>
        public IEnumerable<IFileProvider> FileProviders => _fileProviders;

        /// <summary>
        /// without this the browser did not seem to understand the gzipped file
        /// </summary>
        /// <param name="context"></param>
        public static void OnPrepareResponse(StaticFileResponseContext context)
        {
            var file = context.File;
            var response = context.Context.Response;
            
            if (file.Name.EndsWith(".gz"))
            {
                response.Headers[HeaderNames.ContentEncoding] = "gzip";
                if (file.Name.EndsWith(".css.gz"))
                {
                    response.Headers[HeaderNames.ContentType] = "text/css";
                }
                if (file.Name.EndsWith(".js.gz"))
                {
                    response.Headers[HeaderNames.ContentType] = "text/javascript";
                }
                return;
            }
  
        }
    }
}
