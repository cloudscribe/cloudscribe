// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-06-08
// Last Modified:           2017-06-08
// 

using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Web.Common.Helpers
{
    public interface IResourceHelper
    {
        string ResolveResourceIdentifier(string inputIdentifier);
        string GetMimeType(string extension);
    }
    public class ResourceHelper : IResourceHelper
    {
        public ResourceHelper(
            ILogger<ResourceHelper> logger
            )
        {
            fileTypeProvider = new FileExtensionContentTypeProvider();
            log = logger;
        }

        private FileExtensionContentTypeProvider fileTypeProvider;
        private ILogger log;

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

            inputIdentifier = inputIdentifier.Replace("/", ".");
            if (!inputIdentifier.Contains("-")) return inputIdentifier;

            // we need to not replace - from folder names but not file names
            
            var pathBeforeFileNaame = inputIdentifier.Replace(fileName, string.Empty).Replace("-", "_");

            return pathBeforeFileNaame + fileName;

        }

        // https://stackoverflow.com/questions/189850/what-is-the-javascript-mime-type-for-the-type-attribute-of-a-script-tag
        // application/javascript is preferred vs text/javascript

        public string GetMimeType(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return extension;

            var mimeType = string.Empty;
            var found = fileTypeProvider.TryGetContentType(extension, out mimeType);
            if (found)
            {
                log.LogDebug($"mimetype {mimeType} found for {extension}");
                return mimeType;
            }

            var result = "application/" + extension.Replace(".", string.Empty); ;

            log.LogWarning($"mimetype not found for {extension}, returning {result}");

            return result;
        }
    }
}
