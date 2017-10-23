// Author:                  Joe Audette
// Created:                 2017-10-22
// Last Modified:           2017-10-22
// based on https://github.com/aspnet/FileSystem/blob/dev/src/FS.Composite/CompositeFileProvider.cs

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Composite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;


namespace Microsoft.Extensions.Configuration // so we don't need another using in startup
{
    /// <summary>
    /// The idea here is to be able to serve pre-gzipped js and css
    /// I use webpack to pre gzip js and css bundles for production
    /// If there is a request for a js or css file this will check if there exists the same file with .gz
    /// and if so that will be returned instead.
    /// 
    /// To be even more usefull, added logic to try to auto create the .gz file if it does not exist
    /// 
    /// Usage:
    ///   app.UseStaticFiles(new StaticFileOptions()
    ///   {
    ///        OnPrepareResponse = GzipMappingFileProvider.OnPrepareResponse,
    ///        FileProvider = new GzipMappingFileProvider(
    ///            loggerFactory,
    ///            true,
    ///            Environment.WebRootFileProvider
    ///           )
    ///  });
    /// 
    /// 
    /// </summary>
    public class GzipMappingFileProvider : IFileProvider
    {
        private readonly IFileProvider[] _fileProviders;
        private ILogger _log = null;
        private bool _autoGenGzip;

        /// <summary>
        /// Initializes a new instance of the <see cref="GzipMappingFileProvider" /> class using a collection of file provider.
        /// </summary>
        /// <param name="loggerFactory">The ILoggerFactory. </param>
        /// <param name="autoGenerateMissingGzipFiles">bool indicating whether to try auto creating misssing .gz files </param>
        /// <param name="fileProviders">The collection of <see cref="IFileProvider" /></param>
        public GzipMappingFileProvider(
            ILoggerFactory loggerFactory,
            bool autoGenerateMissingGzipFiles,
            params IFileProvider[] fileProviders
            
            )
        {
            _fileProviders = fileProviders ?? new IFileProvider[0];
            if(loggerFactory != null)
            {
                _log = loggerFactory.CreateLogger<GzipMappingFileProvider>();
            }
            _autoGenGzip = autoGenerateMissingGzipFiles;
        }
        
        private IFileInfo TryAutoCreateGzip(string subpath, IFileProvider fileProvider, IFileInfo inputFile)
        {
            if (inputFile == null) { return inputFile; }
            if (!_autoGenGzip) { return inputFile; }
            if (!(fileProvider is PhysicalFileProvider)) { return inputFile; }
            if (inputFile.Length <= 10240) { return inputFile; }

            try
            {
                using (var inputStream = inputFile.CreateReadStream())
                {
                    using (var outputStream = File.Open(inputFile.PhysicalPath + ".gz", FileMode.Create))
                    {
                        using (var gs = new GZipStream(outputStream, CompressionMode.Compress))
                        {
                            inputStream.CopyTo(gs);
                        }

                    }
                }

                // return the new fileInfo
                var gzfileInfo = fileProvider.GetFileInfo(subpath + ".gz");
                if (gzfileInfo != null && gzfileInfo.Exists)
                {
                    return gzfileInfo;
                }

            }
            catch (IOException ex)
            {
                if(_log != null)
                {
                    _log.LogError(ex.Message + " " + ex.StackTrace);
                }
            }

            

            return inputFile;
        }

        #region IFileProvider

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
                            if(gzfileInfo.LastModified < fileInfo.LastModified)
                            {
                                return TryAutoCreateGzip(subpath, fileProvider, fileInfo);
                            }

                            return gzfileInfo;
                        }
                        else
                        {
                            return TryAutoCreateGzip(subpath,fileProvider, fileInfo);
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
                    response.Headers[HeaderNames.ContentType] = "application/javascript";
                }
                return;
            }
  
        }

        #endregion
    }
}
