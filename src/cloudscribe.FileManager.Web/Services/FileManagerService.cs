// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-02-15
// Last Modified:           2018-07-18
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
            _mediaPathResolver = mediaPathResolver;
            _imageResizer = imageResizer;
            _nameRules = fileManagerNameRules;
            _icons = iconsAccessor.Value;
            _sr = stringLocalizer;
            _log = logger;
        }

        private IImageResizer _imageResizer;
        private IMediaPathResolver _mediaPathResolver;
        private MediaRootPathInfo _rootPath;
        private FileManagerIcons _icons;
        private IFileManagerNameRules _nameRules;
        private IStringLocalizer<FileManagerStringResources> _sr;
        private ILogger _log;

        private async Task EnsureProjectSettings()
        {
            if (_rootPath != null) { return; }
            _rootPath = await _mediaPathResolver.Resolve().ConfigureAwait(false);
            if (_rootPath != null) { return; }
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

        public async Task<UploadResult> CropFile(
            ImageProcessingOptions options,
            string sourceFilePath,
            int offsetX,
            int offsetY,
            int widthToCrop,
            int heightToCrop,
            int finalWidth,
            int finalHeight
            )
        {
            if(string.IsNullOrWhiteSpace(sourceFilePath))
            {
                _log.LogError($"sourceFilePath not provided for crop");
                return new UploadResult
                {
                    ErrorMessage = _sr["There was an error logged during file processing"]
                };
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            string currentFsPath = _rootPath.RootFileSystemPath;
            string currentVirtualPath = _rootPath.RootVirtualPath;
            string[] virtualSegments = options.ImageDefaultVirtualSubPath.Split('/');

            if (!sourceFilePath.StartsWith(_rootPath.RootVirtualPath))
            {
                _log.LogError($"{sourceFilePath} not a sub path of root path {_rootPath.RootVirtualPath}");
                return new UploadResult
                {
                    ErrorMessage = _sr["There was an error logged during file processing"]
                };
            }

            var fileToCropName = Path.GetFileName(sourceFilePath);
            var fileToCropNameWithooutExtenstion = Path.GetFileNameWithoutExtension(sourceFilePath);
            var ext = Path.GetExtension(sourceFilePath);
            var mimeType = GetMimeType(ext);
            var isImage = IsWebImageFile(ext);
            if(!isImage)
            {
                _log.LogError($"{sourceFilePath} is not not an image file");
                return new UploadResult
                {
                    ErrorMessage = _sr["There was an error logged during file processing"]
                };
            }

            var fileToCropFolderVPath = sourceFilePath.Replace(fileToCropName, "");

            var virtualSubPath = fileToCropFolderVPath.Substring(_rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if(segments.Length <= 0)
            {
                _log.LogError($"{sourceFilePath} not found");
                return new UploadResult
                {
                    ErrorMessage = _sr["There was an error logged during file processing"]

                };
            }
            
            var requestedFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments));
            if (!Directory.Exists(requestedFsPath))
            {
                _log.LogError("directory not found for currentPath " + requestedFsPath);
                return new UploadResult
                {
                    ErrorMessage = _sr["There was an error logged during file processing"]

                };
            }
            
            currentVirtualPath = virtualSubPath;
            virtualSegments = segments;
            currentFsPath = Path.Combine(currentFsPath, Path.Combine(virtualSegments));
            var sourceFsPath = Path.Combine(currentFsPath, fileToCropName);
            var cropNameSegment = "-crop";
            int previousCropCount = 0;
            var targetFsPath = Path.Combine(currentFsPath, fileToCropNameWithooutExtenstion + cropNameSegment + ext);
            while(File.Exists(targetFsPath))
            {
                previousCropCount += 1;
                targetFsPath = Path.Combine(currentFsPath, fileToCropNameWithooutExtenstion + cropNameSegment + previousCropCount.ToString() + ext);
            };
            
            var didCrop = _imageResizer.CropExistingImage(
                sourceFsPath,
                targetFsPath,
                offsetX,
                offsetY,
                widthToCrop,
                heightToCrop,
                finalWidth,
                finalHeight
                );

            if(!didCrop)
            {
                _log.LogError($"failed to crop image {requestedFsPath}");
                return new UploadResult
                {
                    ErrorMessage = _sr["There was an error logged during file processing"]

                };
            }
            
            return new UploadResult
            {
                OriginalUrl = sourceFilePath,
                ResizedUrl = currentVirtualPath + Path.GetFileName(targetFsPath)
            };

        }

        public async Task<UploadResult> ProcessFile(
            IFormFile formFile,
            ImageProcessingOptions options,
            bool? resizeImages,
            int? maxWidth,
            int? maxHeight,
            string requestedVirtualPath = "",
            string newFileName = "",
            bool allowRootPath = true,
            bool createThumbnail = false,
            bool? keepOriginal = null
            )
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            string currentFsPath = _rootPath.RootFileSystemPath;
            string currentVirtualPath = _rootPath.RootVirtualPath;
            string[] virtualSegments = options.ImageDefaultVirtualSubPath.Split('/');
            bool doResize = resizeImages ?? options.AutoResize;

            if ((!string.IsNullOrEmpty(requestedVirtualPath)) && (requestedVirtualPath.StartsWith(_rootPath.RootVirtualPath)))
            {
                var virtualSubPath = requestedVirtualPath.Substring(_rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                if (segments.Length > 0)
                {
                    var requestedFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments));
                    if (!Directory.Exists(requestedFsPath))
                    {
                        //_log.LogError("directory not found for currentPath " + requestedFsPath);
                        // user has file system permission and could manually create the needed folder so auto ensure
                        // since it is a sub path of the root
                        EnsureSubFolders(_rootPath.RootFileSystemPath, segments);
                    }
                    
                    currentVirtualPath = requestedVirtualPath;
                    virtualSegments = segments;
                    currentFsPath = Path.Combine(currentFsPath, Path.Combine(virtualSegments));   
                }
            }
            else
            {

                // ensure the folders if no currentDir provided,
                // if it is provided it must be an existing path
                // options.ImageDefaultVirtualSubPath might not exist on first upload so need to ensure it
                if (!allowRootPath)
                {
                    currentVirtualPath = currentVirtualPath + options.ImageDefaultVirtualSubPath;
                    currentFsPath = Path.Combine(currentFsPath, Path.Combine(virtualSegments));
                    EnsureSubFolders(_rootPath.RootFileSystemPath, virtualSegments);
                }

            }
            string newName;
            if (!string.IsNullOrEmpty(newFileName))
            {
                newName = _nameRules.GetCleanFileName(newFileName);
            }
            else
            {
                newName = _nameRules.GetCleanFileName(Path.GetFileName(formFile.FileName));
            }

            var newUrl = currentVirtualPath + "/" + newName;
            var fsPath = Path.Combine(currentFsPath, newName);

            var ext = Path.GetExtension(newName);
            var webSizeName = Path.GetFileNameWithoutExtension(newName) + "-ws" + ext;

            string webUrl = currentVirtualPath + "/" + webSizeName;

            var thumbSizeName = Path.GetFileNameWithoutExtension(newName) + "-thumb" + ext;
            string thumbUrl = currentVirtualPath + "/" + thumbSizeName;

            var didResize = false;
            var didCreateThumb = false;

            try
            {
                using (var stream = new FileStream(fsPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                var mimeType = GetMimeType(ext);

                if ((doResize) && IsWebImageFile(ext))
                {
                    int resizeWidth = GetMaxWidth(maxWidth, options);
                    int resizeHeight = GetMaxWidth(maxHeight, options);

                    didResize = _imageResizer.ResizeImage(
                        fsPath,
                        currentFsPath,
                        webSizeName,
                        mimeType,
                        resizeWidth,
                        resizeHeight,
                        options.AllowEnlargement,
                        options.ResizeQuality
                        );
                }

                if (createThumbnail)
                {
                    didCreateThumb = _imageResizer.ResizeImage(
                        fsPath,
                        currentFsPath,
                        thumbSizeName,
                        mimeType,
                        options.ThumbnailImageMaxWidth,
                        options.ThumbnailImageMaxHeight,
                        false,
                        options.ResizeQuality
                        );
                }
                if(didResize)
                {
                    if (keepOriginal.HasValue)
                    {
                        if (keepOriginal.Value == false)
                        {
                            File.Delete(fsPath);
                            newUrl = string.Empty;
                        }
                    }
                    else if (!options.KeepOriginalImages) // use default if not explcitely passed
                    {
                        File.Delete(fsPath);
                        newUrl = string.Empty;
                    }
                }
                
            }
            catch (Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");

                return new UploadResult
                {
                    ErrorMessage = _sr["There was an error logged during file processing"]

                };
            }

            return new UploadResult
            {
                OriginalUrl = newUrl,
                ResizedUrl = didResize ? webUrl : string.Empty,
                ThumbUrl = didCreateThumb ? thumbUrl : string.Empty,
                Name = newName,
                Length = formFile.Length,
                Type = formFile.ContentType

            };
        }

        public async Task<string> GetRootVirtualPath()
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            return _rootPath.RootVirtualPath;
        }

        public async Task<OperationResult> CreateFolder(string requestedVirtualPath, string folderName)
        {
            OperationResult result;
            
            if (string.IsNullOrEmpty(folderName))
            {
                result = new OperationResult(false);
                result.Message = _sr["Folder name not provided"];
                _log.LogWarning($"CreateFolder: Folder name not provided");
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                requestedVirtualPath = _rootPath.RootVirtualPath;
            }

            var isRoot = (requestedVirtualPath == _rootPath.RootVirtualPath);
            string requestedFsPath;

            if (!isRoot)
            {
                if (!requestedVirtualPath.StartsWith(_rootPath.RootVirtualPath))
                {
                    result = new OperationResult(false);
                    result.Message = _sr["Invalid path"];
                    _log.LogWarning($"CreateFolder: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                    return result;
                }

                var virtualSubPath = requestedVirtualPath.Substring(_rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                if (segments.Length > 0)
                {
                    requestedFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments));
                    if (!Directory.Exists(requestedFsPath))
                    {
                        result = new OperationResult(false);
                        result.Message = _sr["Invalid path"];
                        _log.LogWarning($"CreateFolder: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                        return result;
                    }

                }
                else
                {
                    requestedFsPath = _rootPath.RootFileSystemPath;
                }
            }
            else
            {
                requestedFsPath = _rootPath.RootFileSystemPath;
            }
            
            var newFolderFsPath = Path.Combine(requestedFsPath, _nameRules.GetCleanFolderName(folderName));
            if (Directory.Exists(newFolderFsPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Folder already exists"];
                _log.LogWarning($"CreateFolder: {requestedVirtualPath} already exists");
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
                _log.LogError(MediaLoggingEvents.FOLDER_CREATION, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = _sr["Server error"];
                return result;
            }

        }

        public async Task<OperationResult> DeleteFolder(string requestedVirtualPath)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Path not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(_rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"DeleteFolder: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(_rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            { 
                // don't allow delete the root folder
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"DeleteFolder: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var requestedFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments));
            if (!Directory.Exists(requestedFsPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"DeleteFolder: {requestedVirtualPath} does not exist");
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
                _log.LogError(MediaLoggingEvents.FOLDER_DELETE, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = _sr["Server error"];
                return result;
            }

            

        }

        public async Task<OperationResult> RenameFolder(string requestedVirtualPath, string newNameSegment)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Path not provided"];
                return result;
            }

            if (string.IsNullOrEmpty(newNameSegment))
            {
                result = new OperationResult(false);
                result.Message = _sr["New name not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(_rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"RenameFolder: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(_rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            {
                // don't allow delete the root folder
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"RenameFolder: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var currentFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments));
            if (!Directory.Exists(currentFsPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                return result;
            }
            segments = segments.Take(segments.Count() - 1).ToArray();
            var cleanFolderName = _nameRules.GetCleanFolderName(newNameSegment);
            string newFsPath;
            if (segments.Length > 0)
            {
                newFsPath = Path.Combine(Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments)), cleanFolderName);
            }
            else
            {
                newFsPath = Path.Combine(_rootPath.RootFileSystemPath, cleanFolderName);
            }
            

            if (Directory.Exists(newFsPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Directory already exists"];
                _log.LogWarning($"RenameFolder: {requestedVirtualPath} already exists");
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
                _log.LogError(MediaLoggingEvents.FOLDER_RENAME, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = _sr["A error was logged while processing the request"];
                return result;
            }



        }

        public async Task<OperationResult> DeleteFile(string requestedVirtualPath)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Path not provided"];
                _log.LogWarning("Delete File: virtual path not provided");
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(_rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"DeleteFile: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(_rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            {
                // no file just root folder url
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"DeleteFile: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var requestedFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments));
            if (!File.Exists(requestedFsPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"DeleteFile: {requestedVirtualPath} does not exist");
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
                _log.LogError(MediaLoggingEvents.FILE_DELETE, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = _sr["A error was logged while processing the request"];
                return result;
            }


        }

        public async Task<OperationResult> RenameFile(string requestedVirtualPath, string newNameSegment)
        {
            OperationResult result;
            if (string.IsNullOrEmpty(requestedVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Path not provided"];
                return result;
            }

            if (string.IsNullOrEmpty(newNameSegment))
            {
                result = new OperationResult(false);
                result.Message = _sr["New name not provided"];
                return result;
            }

            await EnsureProjectSettings().ConfigureAwait(false);

            if (!requestedVirtualPath.StartsWith(_rootPath.RootVirtualPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"RenameFile: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var virtualSubPath = requestedVirtualPath.Substring(_rootPath.RootVirtualPath.Length);
            var segments = virtualSubPath.Split('/');

            if (segments.Length == 0)
            {
                // don't allow delete the root folder
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"RenameFile: {requestedVirtualPath} was not valid for root path {_rootPath.RootVirtualPath}");
                return result;
            }

            var currentFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments));
            var ext = Path.GetExtension(currentFsPath);

            if (!File.Exists(currentFsPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Invalid path"];
                _log.LogWarning($"RenameFile: {requestedVirtualPath} does not exist");
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
            var cleanFileName = _nameRules.GetCleanFileName(newNameSegment);
            
            string newFsPath;
            if (segments.Length > 0)
            {
                newFsPath = Path.Combine(Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments)), cleanFileName);
            }
            else
            {
                newFsPath = Path.Combine(_rootPath.RootFileSystemPath, cleanFileName);
            }


            if (File.Exists(newFsPath))
            {
                result = new OperationResult(false);
                result.Message = _sr["Directory already exists"];
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
                _log.LogError(MediaLoggingEvents.FOLDER_RENAME, ex, ex.Message + " " + ex.StackTrace);
                result = new OperationResult(false);
                result.Message = _sr["A error was logged while processing the request"];
                return result;
            }



        }

        


        public async Task<List<Node>> GetFileTree(string fileType, string virtualStartPath)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            var list = new List<Node>();

            if(!Directory.Exists(_rootPath.RootFileSystemPath)) 
            {
                _log.LogError("directory not found for RootFileSystemPath " + _rootPath.RootFileSystemPath);
                return list;
            }

            DirectoryInfo currentDirectory;
            IEnumerable<DirectoryInfo> folders;
            //bool isRoot = false;
            string currentFsPath = _rootPath.RootFileSystemPath;
            string currentVirtualPath = _rootPath.RootVirtualPath;

            if (!string.IsNullOrEmpty(virtualStartPath))
            {
                if(!virtualStartPath.StartsWith(_rootPath.RootVirtualPath))
                {
                    _log.LogError("virtualStartPath did not start with RootFileSystemPath " + virtualStartPath);
                    return list;
                }
                var virtualSubPath = virtualStartPath.Substring(_rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                currentFsPath = Path.Combine(_rootPath.RootFileSystemPath, Path.Combine(segments)); 
                if(!Directory.Exists(currentFsPath))
                {
                    _log.LogError("directory not found for currentPath " + currentFsPath);
                    return list;
                }
                currentDirectory = new DirectoryInfo(currentFsPath);
                currentVirtualPath = virtualStartPath;
            }
            else
            {
                //isRoot = true;
                currentDirectory = new DirectoryInfo(_rootPath.RootFileSystemPath);
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
                node.Icon = _icons.Folder;
                node.ExpandedIcon = _icons.FolderOpen;
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
                    return _icons.FileWord;

                case "xls":
                case "xlsx":
                    return _icons.FileExcel;

                
                case "ppt":
                case "pptx":
                    return _icons.FilePowerpoint;

                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                case "ico":
                    return _icons.FileImage;

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
                    return _icons.FileVideo;


                case "mp3":
                case "m4a":
                case "oga":
                case "spx":
                    return _icons.FileAudio;

               
                case "htm":
                case "html":
                case "css":
                case "js":
                    return _icons.FileCode;

                case "zip":
                case "tar":
                    return _icons.FileArchive;

                case "pdf":
                    return _icons.FilePdf;


                default:
                    return _icons.File;

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
