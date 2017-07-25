// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-02-15
// Last Modified:           2017-05-14
// 

using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Models.TreeView;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Services
{
    public class FileManagerService
    {
        public FileManagerService(
            IMediaPathResolver mediaPathResolver,
            IImageResizer imageResizer,
            IFileManagerNameRules fileManagerNameRules,
            IStringLocalizer<FileManagerStringResources> stringLocalizer,
            IOptions<FileManagerIcons> iconsAccessor,
            ILogger<FileManagerService> logger
            )
        {
            this.mediaPathResolver = mediaPathResolver;
            this.imageResizer = imageResizer;
            nameRules = fileManagerNameRules;
            icons = iconsAccessor.Value;
            sr = stringLocalizer;
            log = logger;
        }

        private IImageResizer imageResizer;
        private IMediaPathResolver mediaPathResolver;
        private MediaRootPathInfo rootPath;
        private FileManagerIcons icons;
        private IFileManagerNameRules nameRules;
        private IStringLocalizer<FileManagerStringResources> sr;
        private ILogger log;

        private async Task EnsureProjectSettings()
        {
            if (rootPath != null) { return; }
            rootPath = await mediaPathResolver.Resolve().ConfigureAwait(false);
            if (rootPath != null) { return; }
        }

        private void EnsureSubFolders(string basePath, string[] segments)
        {
            var p = basePath;
            for (int i = 0; i < segments.Length; i++)
            {
                p = Path.Combine(p, segments[i]);
                if (!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
            }
        }

        private int GetMaxWidth(int? maxWidth, ImageProcessingOptions options)
        {
            if(maxWidth.HasValue)
            {
                if(maxWidth.Value >= options.ResizeMinAllowedWidth && maxWidth.Value <= options.ResizeMaxAllowedWidth)
                {
                    return maxWidth.Value;
                }
            }

            return options.WebSizeImageMaxWidth;
        }

        private int GetMaxHeight(int? maxHeight, ImageProcessingOptions options)
        {
            if (maxHeight.HasValue)
            {
                if (maxHeight.Value >= options.ResizeMinAllowedHeight && maxHeight.Value <= options.ResizeMaxAllowedHeight)
                {
                    return maxHeight.Value;
                }
            }

            return options.WebSizeImageMaxWidth;
        }

        public async Task<string> GetRootVirtualPath()
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            return rootPath.RootVirtualPath;
        }

        public async Task<OperationResult> CreateFolder(string requestedVirtualPath, string folderName)
        {
            OperationResult result;
            
            if (string.IsNullOrEmpty(folderName))
            {
                result = new OperationResult(false);
                result.Message = sr["Folder name not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                requestedVirtualPath = rootPath.RootVirtualPath;
            }

            var isRoot = (requestedVirtualPath == rootPath.RootVirtualPath);
            string requestedFsPath;

            if (!isRoot)
            {
                if (!requestedVirtualPath.StartsWith(rootPath.RootVirtualPath))
                {
                    result = new OperationResult(false);
                    result.Message = sr["Invalid path"];
                    return result;
                }

                var virtualSubPath = requestedVirtualPath.Substring(rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                if (segments.Length > 0)
                {
                    requestedFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments));
                    if (!Directory.Exists(requestedFsPath))
                    {
                        result = new OperationResult(false);
                        result.Message = sr["Invalid path"];
                        return result;
                    }

                }
                else
                {
                    requestedFsPath = rootPath.RootFileSystemPath;
                }
            }
            else
            {
                requestedFsPath = rootPath.RootFileSystemPath;
            }
            
            var newFolderFsPath = Path.Combine(requestedFsPath, nameRules.GetCleanFolderName(folderName));
            if (Directory.Exists(newFolderFsPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Folder already exists"];
                return result;
            }

            try
            {
                Directory.CreateDirectory(newFolderFsPath);
                result = new OperationResult(true);
                return result;
            }
            catch(IOException ex)
            {
                log.LogError(MediaLoggingEvents.FOLDER_CREATION, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = sr["Server error"];
                return result;
            }

        }

        public async Task<OperationResult> DeleteFolder(string requestedVirtualPath)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Path not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            { 
                // don't allow delete the root folder
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var requestedFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments));
            if (!Directory.Exists(requestedFsPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            try
            {
                Directory.Delete(requestedFsPath, true);
                result = new OperationResult(true);
                return result;
            }
            catch (IOException ex)
            {
                log.LogError(MediaLoggingEvents.FOLDER_DELETE, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = sr["Server error"];
                return result;
            }

            

        }

        public async Task<OperationResult> RenameFolder(string requestedVirtualPath, string newNameSegment)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Path not provided"];
                return result;
            }

            if (string.IsNullOrEmpty(newNameSegment))
            {
                result = new OperationResult(false);
                result.Message = sr["New name not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            {
                // don't allow delete the root folder
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var currentFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments));
            if (!Directory.Exists(currentFsPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }
            segments = segments.Take(segments.Count() - 1).ToArray();
            var cleanFolderName = nameRules.GetCleanFolderName(newNameSegment);
            string newFsPath;
            if (segments.Length > 0)
            {
                newFsPath = Path.Combine(Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments)), cleanFolderName);
            }
            else
            {
                newFsPath = Path.Combine(rootPath.RootFileSystemPath, cleanFolderName);
            }
            

            if (Directory.Exists(newFsPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Directory already exists"];
                return result;
            }

            try
            {
                
                Directory.Move(currentFsPath, newFsPath);
                result = new OperationResult(true);
                return result;
            }
            catch (IOException ex)
            {
                log.LogError(MediaLoggingEvents.FOLDER_RENAME, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = sr["A error was logged while processing the request"];
                return result;
            }



        }

        public async Task<OperationResult> DeleteFile(string requestedVirtualPath)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Path not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            {
                // no file just root folder url
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var requestedFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments));
            if (!File.Exists(requestedFsPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            try
            {
                File.Delete(requestedFsPath);
                result = new OperationResult(true);
                return result;
            }
            catch (IOException ex)
            {
                log.LogError(MediaLoggingEvents.FILE_DELETE, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = sr["A error was logged while processing the request"];
                return result;
            }


        }

        public async Task<OperationResult> RenameFile(string requestedVirtualPath, string newNameSegment)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Path not provided"];
                return result;
            }

            if (string.IsNullOrEmpty(newNameSegment))
            {
                result = new OperationResult(false);
                result.Message = sr["New name not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            {
                // don't allow delete the root folder
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            var currentFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments));
            var ext = Path.GetExtension(currentFsPath);

            if (!File.Exists(currentFsPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Invalid path"];
                return result;
            }

            // pop the last segment
            segments = segments.Take(segments.Count() - 1).ToArray();

            if (Path.HasExtension(newNameSegment) && Path.GetExtension(newNameSegment) == ext)
            {
                // all good
            }
            else
            {
                newNameSegment = Path.GetFileNameWithoutExtension(newNameSegment) + ext;
            }
            var cleanFileName = nameRules.GetCleanFileName(newNameSegment);
            
            string newFsPath;
            if (segments.Length > 0)
            {
                newFsPath = Path.Combine(Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments)), cleanFileName);
            }
            else
            {
                newFsPath = Path.Combine(rootPath.RootFileSystemPath, cleanFileName);
            }


            if (File.Exists(newFsPath))
            {
                result = new OperationResult(false);
                result.Message = sr["Directory already exists"];
                return result;
            }

            try
            {

                File.Move(currentFsPath, newFsPath);
                result = new OperationResult(true);
                return result;
            }
            catch (IOException ex)
            {
                log.LogError(MediaLoggingEvents.FOLDER_RENAME, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = sr["A error was logged while processing the request"];
                return result;
            }



        }

        public async Task<UploadResult> ProcessFile(
            IFormFile formFile,
            ImageProcessingOptions options,
            bool? resizeImages,
            int? maxWidth,
            int? maxHeight,
            string requestedVirtualPath = "",
            string newFileName = "",
            bool allowRootPath = true
            )
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            string currentFsPath = rootPath.RootFileSystemPath;
            string currentVirtualPath = rootPath.RootVirtualPath;
            string[] virtualSegments = options.ImageDefaultVirtualSubPath.Split('/');
            bool doResize = resizeImages.HasValue ? resizeImages.Value : options.AutoResize;

            if ((!string.IsNullOrEmpty(requestedVirtualPath)) && (requestedVirtualPath.StartsWith(rootPath.RootVirtualPath)))
            {

                var virtualSubPath = requestedVirtualPath.Substring(rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                if(segments.Length > 0)
                {
                    var requestedFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments));
                    if (!Directory.Exists(requestedFsPath))
                    {
                        log.LogError("directory not found for currentPath " + requestedFsPath);
                    }
                    else
                    {
                        currentVirtualPath = requestedVirtualPath;
                        virtualSegments = segments;
                        currentFsPath = Path.Combine(currentFsPath, Path.Combine(virtualSegments));
                    }
                }    
                
            }
            else
            {

                // only ensure the folders if no currentDir provided,
                // if it is provided it must be an existing path
                // options.ImageDefaultVirtualSubPath might not exist on first upload so need to ensure it
                if(!allowRootPath)
                {
                    currentVirtualPath = currentVirtualPath + options.ImageDefaultVirtualSubPath;
                    currentFsPath = Path.Combine(currentFsPath, Path.Combine(virtualSegments));
                    EnsureSubFolders(rootPath.RootFileSystemPath, virtualSegments);
                }
                
            }
            string newName;
            if (!string.IsNullOrEmpty(newFileName))
            {
                newName = nameRules.GetCleanFileName(newFileName);
            }
            else
            {
                newName = nameRules.GetCleanFileName(Path.GetFileName(formFile.FileName));
            }
            
            var newUrl = currentVirtualPath + "/" + newName;
            var fsPath = Path.Combine(currentFsPath, newName);

            var ext = Path.GetExtension(newName);
            var webSizeName = Path.GetFileNameWithoutExtension(newName) + "-ws" + ext;
            var webFsPath = Path.Combine(currentFsPath, webSizeName);
            string webUrl = string.Empty;
            var didResize = false;

            try
            {
                using (var stream = new FileStream(fsPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                if ((doResize) && IsWebImageFile(ext))
                {
                    var mimeType = GetMimeType(ext);
                    webUrl = currentVirtualPath + "/" + webSizeName;
                    int resizeWidth = GetMaxWidth(maxWidth, options);
                    int resizeHeight = GetMaxWidth(maxHeight, options);

                    didResize = imageResizer.ResizeImage(
                        fsPath,
                        currentFsPath,
                        webSizeName,
                        mimeType,
                        resizeWidth,
                        resizeHeight,
                        options.AllowEnlargement
                        );
                }

                return new UploadResult
                {
                    OriginalUrl = newUrl,
                    ResizedUrl = didResize? webUrl : string.Empty,
                    Name = newName,
                    Length = formFile.Length,
                    Type = formFile.ContentType

                };
            }
            catch (Exception ex)
            {
                log.LogError(MediaLoggingEvents.FILE_PROCCESSING, ex, ex.StackTrace);

                return new UploadResult
                {
                    ErrorMessage = sr["There was an error logged during file processing"]

                };
            }
        }


        public async Task<List<Node>> GetFileTree(string fileType, string virtualStartPath)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            var list = new List<Node>();

            if(!Directory.Exists(rootPath.RootFileSystemPath)) 
            {
                log.LogError("directory not found for RootFileSystemPath " + rootPath.RootFileSystemPath);
                return list;
            }

            DirectoryInfo currentDirectory;
            IEnumerable<DirectoryInfo> folders;
            //bool isRoot = false;
            string currentFsPath = rootPath.RootFileSystemPath;
            string currentVirtualPath = rootPath.RootVirtualPath;

            if (!string.IsNullOrEmpty(virtualStartPath))
            {
                if(!virtualStartPath.StartsWith(rootPath.RootVirtualPath))
                {
                    log.LogError("virtualStartPath did not start with RootFileSystemPath " + virtualStartPath);
                    return list;
                }
                var virtualSubPath = virtualStartPath.Substring(rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                currentFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments)); 
                if(!Directory.Exists(currentFsPath))
                {
                    log.LogError("directory not found for currentPath " + currentFsPath);
                    return list;
                }
                currentDirectory = new DirectoryInfo(currentFsPath);
                currentVirtualPath = virtualStartPath;
            }
            else
            {
                //isRoot = true;
                currentDirectory = new DirectoryInfo(rootPath.RootFileSystemPath);
            }
            
            folders = from folder in currentDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly)
                      select folder;


            foreach (var folder in folders)
            {
                var node = new Node();
                node.Text = folder.Name;
                node.Type = "d";
                node.VirtualPath = currentVirtualPath + "/" + folder.Name;
                node.Id = node.VirtualPath; // TODO: maybe just use id
                node.Created = folder.CreationTimeUtc;
                node.Modified = folder.LastWriteTimeUtc;
                node.Icon = icons.Folder;
                node.ExpandedIcon = icons.FolderOpen;
                node.LazyLoad = true;
                list.Add(node);
            }
            var rootFiles = Directory.GetFiles(currentFsPath);
            foreach (var filePath in rootFiles)
            {
                var file = new FileInfo(filePath);
                var node = new Node();
                node.Text = file.Name;
                node.Type = "f";
                node.VirtualPath = currentVirtualPath + "/" + file.Name;
                node.Id = node.VirtualPath;  // TODO: maybe just use id
                node.Size = file.Length;
                // TODO: timezome adjustment
                node.Created = file.CreationTimeUtc;
                node.Modified = file.LastWriteTimeUtc;
                node.CanPreview = IsWebImageFile(file.Extension);
                node.Icon = GetIconCssClass(file.Extension);
                //node.ExpandedIcon = node.Icon;
                //file.
                if(fileType == "image")
                {
                    if(node.CanPreview) list.Add(node);
                }
                else
                {
                    list.Add(node);
                }
                
            }

            return list;

        }



        public static bool IsWebImageFile(string fileExtension)
        {
            if (string.Equals(fileExtension, ".gif", StringComparison.OrdinalIgnoreCase)) { return true; }
            if (string.Equals(fileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)) { return true; }
            if (string.Equals(fileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)) { return true; }
            if (string.Equals(fileExtension, ".png", StringComparison.OrdinalIgnoreCase)) { return true; }

            return false;
        }

        public string GetIconCssClass(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return string.Empty; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);

            switch (fileType)
            {
                case "doc":
                case "docx":
                    return icons.FileWord;

                case "xls":
                case "xlsx":
                    return icons.FileExcel;

                
                case "ppt":
                case "pptx":
                    return icons.FilePowerpoint;

                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                case "ico":
                    return icons.FileImage;

                case "wmv":
                case "mpg":
                case "mpeg":
                case "mp4":
                case "m4v":
                case "oog":
                case "ogv":
                case "webm":
                case "mov":
                case "flv":
                    return icons.FileVideo;


                case "mp3":
                case "m4a":
                case "oga":
                case "spx":
                    return icons.FileAudio;

               
                case "htm":
                case "html":
                case "css":
                case "js":
                    return icons.FileCode;

                case "zip":
                case "tar":
                    return icons.FileArchive;

                case "pdf":
                    return icons.FilePdf;


                default:
                    return icons.File;

            }

            


        }

        public string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return string.Empty; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);

            switch (fileType)
            {
                case "doc":
                case "docx":
                    return "application/msword";

                case "xls":
                case "xlsx":
                    return "application/vnd.ms-excel";

                case "exe":
                    return "application/octet-stream";

                case "ppt":
                case "pptx":
                    return "application/vnd.ms-powerpoint";

                case "jpg":
                case "jpeg":
                    return "image/jpeg";

                case "gif":
                    return "image/gif";

                case "png":
                    return "image/png";

                case "bmp":
                    return "image/bmp";

                case "wmv":
                    return "video/x-ms-wmv";

                case "mpg":
                case "mpeg":
                    return "video/mpeg";

                case "mp4":
                    return "video/mp4";

                case "mp3":
                    return "audio/mp3";

                case "m4a":
                    return "audio/m4a";

                case "m4v":
                    return "video/m4v";

                case "oog":
                case "ogv":
                    return "video/ogg";
                case "oga":
                case "spx":
                    return "audio/ogg";

                case "webm":
                    return "video/webm";

                case "mov":
                    return "video/quicktime";

                case "flv":
                    return "video/x-flv";

                case "ico":
                    return "image/x-icon";

                case "htm":
                case "html":
                    return "text/html";

                case "css":
                    return "text/css";

                case "eps":
                    return "application/postscript";

            }

            return "application/" + fileType;


        }



        public bool IsNonAttacmentFileType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return false; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);
            if (fileType == "pdf") { return true; }
            //if (fileType == "wmv") { return true; } //necessary?


            return false;
        }

    }
}
