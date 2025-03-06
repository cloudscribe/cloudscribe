// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-02-14
// Last Modified:           2019-07-01
// 

using cloudscribe.FileManager.Web.Events;
using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Services;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Controllers
{
    public class FileManagerController : Controller
    {
        public FileManagerController(
            FileManagerService fileManagerService,
            IAuthorizationService authorizationService,
            IEnumerable<IHandleFilesUploaded> uploadHandlers,
            IOptions<AutomaticUploadOptions> autoUploadOptionsAccessor,
            IAntiforgery antiforgery,
            IResourceHelper resourceHelper,
            ILogger<FileManagerController> logger
            )
        {
            _fileManagerService = fileManagerService;
            _uploadHandlers = uploadHandlers;
            _authorizationService = authorizationService;
            _autoUploadOptions = autoUploadOptionsAccessor.Value;
            _antiforgery = antiforgery;
            _resourceHelper = resourceHelper;
            _log = logger;
        }

        private FileManagerService _fileManagerService;
        private readonly IEnumerable<IHandleFilesUploaded> _uploadHandlers;
        private IAuthorizationService _authorizationService;
        private AutomaticUploadOptions _autoUploadOptions;
        private readonly IAntiforgery _antiforgery;
        private IResourceHelper _resourceHelper;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private ILogger _log;

        [HttpGet]
        [Authorize(Policy = "FileManagerPolicy")]
        public async Task<IActionResult> Index(BrowseModel model)
        {
            model.InitialVirtualPath = await _fileManagerService.GetRootVirtualPath().ConfigureAwait(false);
            model.FileTreeServiceUrl = Url.Action("GetFileTreeJson", "FileManager", new { fileType = model.Type });
            model.UploadServiceUrl = Url.Action("Upload", "FileManager");
            model.FileDownloadServiceUrl = Url.Action("DownloadFile", "FileManager");
            model.CreateFolderServiceUrl = Url.Action("CreateFolder", "FileManager");
            model.DeleteFolderServiceUrl = Url.Action("DeleteFolder", "FileManager");
            model.RenameFolderServiceUrl = Url.Action("RenameFolder", "FileManager");
            model.DeleteFileServiceUrl = Url.Action("DeleteFile", "FileManager");
            model.RenameFileServiceUrl = Url.Action("RenameFile", "FileManager");
            var authResult = await _authorizationService.AuthorizeAsync(User, "FileManagerDeletePolicy");
            model.CanDelete = authResult.Succeeded;

            switch (model.Type)
            {
                case "image":
                    model.AllowedFileExtensions = _autoUploadOptions.ImageFileExtensions;
                    break;

                case "video":
                    model.AllowedFileExtensions = _autoUploadOptions.VideoFileExtensions;
                    break;

                case "audio":
                    model.AllowedFileExtensions = _autoUploadOptions.AudioFileExtensions;
                    break;

                default:
                    model.AllowedFileExtensions = _autoUploadOptions.AllowedFileExtensions;
                    break;
            }

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }


        [HttpGet]
        [Authorize(Policy = "FileManagerPolicy")]
        public async Task<IActionResult> FileDialog(BrowseModel model)
        {
            model.InitialVirtualPath = await _fileManagerService.GetRootVirtualPath().ConfigureAwait(false);
            model.FileTreeServiceUrl = Url.Action("GetFileTreeJson","FileManager", new { fileType = model.Type});
            model.UploadServiceUrl = Url.Action("Upload", "FileManager");
            model.CreateFolderServiceUrl = Url.Action("CreateFolder", "FileManager");
            model.DeleteFolderServiceUrl = Url.Action("DeleteFolder", "FileManager");
            model.RenameFolderServiceUrl = Url.Action("RenameFolder", "FileManager");
            model.DeleteFileServiceUrl = Url.Action("DeleteFile", "FileManager");
            model.RenameFileServiceUrl = Url.Action("RenameFile", "FileManager");
            var authResult = await _authorizationService.AuthorizeAsync(User, "FileManagerDeletePolicy");
            model.CanDelete = authResult.Succeeded;

            switch(model.Type)
            {
                case "image":
                    model.AllowedFileExtensions = _autoUploadOptions.ImageFileExtensions;
                    break;

                case "video":
                    model.AllowedFileExtensions = _autoUploadOptions.VideoFileExtensions;
                    break;

                case "audio":
                    model.AllowedFileExtensions = _autoUploadOptions.AudioFileExtensions;
                    break;

                default:
                    model.AllowedFileExtensions = _autoUploadOptions.AllowedFileExtensions;

                    break;
            }

            if(HttpContext.Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        //https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
        
        [HttpPost]
        [Authorize(Policy = "FileManagerPolicy")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Upload(
            bool? resizeImages,
            int? maxWidth,
            int? maxHeight,
            string currentDir = "",
            string croppedFileName = "",
            bool keepOriginal = false
            )
        {
            var theFiles = HttpContext.Request.Form.Files;
            var uploadList = new List<UploadResult>();
            if(resizeImages.HasValue)
            {
                if(resizeImages.Value == false)
                {
                    if(Path.HasExtension(currentDir)) //this will be true for cropped
                    {
                        currentDir = currentDir.Substring(0, currentDir.LastIndexOf("/"));
                    }
                }
            }
            string newFileName = string.Empty; ;
            if(theFiles.Count == 1 && !string.IsNullOrEmpty(croppedFileName))
            {
                newFileName = croppedFileName;
            }

            var allowRootPath = true;
            var createThumb = false;
            
            foreach (var formFile in theFiles)
            {
                try
                {
                    if (formFile.Length > 0)
                    {
                        var uploadResult = await _fileManagerService.ProcessFile(
                            formFile,
                            _autoUploadOptions,
                            resizeImages,
                            maxWidth,
                            maxHeight,
                            currentDir,
                            newFileName,
                            allowRootPath,
                            createThumb,
                            keepOriginal
                            ).ConfigureAwait(false);
                        
                        uploadList.Add(uploadResult);

                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(MediaLoggingEvents.FILE_PROCCESSING, ex, ex.StackTrace);
                }

            }

            foreach (var handler in _uploadHandlers)
            {
                try
                {
                    await handler.Handle(uploadList);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message}-{ex.StackTrace}");
                }
            }

            return Json(uploadList);
        }

        [HttpPost]
        [Authorize(Policy = "FileUploadPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DropFile()
        {
            var theFiles = HttpContext.Request.Form.Files;
            var uploadList = new List<UploadResult>();
            string newFileName = string.Empty;
            var allowRootPath = false;
            var createThumbnail = false;
            var requestedFilePath = Request.Form["targetPath"].ToString();

            if(requestedFilePath == "/media/user-avatars")
            {
                var userName = User.Identity.Name;
                if(!string.IsNullOrWhiteSpace(userName))
                {
                    var safeSegment = _fileManagerService.GetSafeFolderSegment(userName);
                    if(!string.IsNullOrWhiteSpace(safeSegment))
                    {
                        requestedFilePath += "/" + safeSegment;
                    }
                }
            }
            bool? resizeImages = null;
            int? maxWidth = null;
            int? maxHeight = null;
            var smaxHeight = Request.Form["maxHeight"];
            var smaxWidth = Request.Form["maxWidth"];
            var sCreateThumbnail = Request.Form["createThumbnail"];
            if (!string.IsNullOrWhiteSpace(smaxHeight) && !string.IsNullOrWhiteSpace(smaxWidth))
            {
                try
                {
                    maxWidth = Convert.ToInt32(smaxWidth);
                    maxHeight = Convert.ToInt32(smaxHeight);
                    resizeImages = true;
                }
                catch{}
            }
           
            var sResize = Request.Form["resizeImage"];
            if(!string.IsNullOrWhiteSpace(sResize))
            {
                var autoResize = true;
                bool.TryParse(sResize, out autoResize);
                if (!autoResize) { resizeImages = false; }
            }

            bool? keepOriginal = null;
            var sKeepOriginal = Request.Form["keepOriginal"];
            if (!string.IsNullOrWhiteSpace(sKeepOriginal))
            {
                // if passed this will override the default
                var tmpKeep = false;
                if(bool.TryParse(sKeepOriginal, out tmpKeep))
                {
                    keepOriginal = tmpKeep;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(sCreateThumbnail))
            {
                bool.TryParse(sCreateThumbnail, out createThumbnail);
            }

            var manageAuthResult = await _authorizationService.AuthorizeAsync(User, "FileManagerPolicy");
            var lessPermissionAllowedExtensions = _autoUploadOptions.AllowedLessPrivilegedFileExtensions.Split('|').ToList();

            
            foreach (var formFile in theFiles)
            {
                var ext = Path.GetExtension(formFile.FileName);
                var canSave = manageAuthResult.Succeeded || lessPermissionAllowedExtensions.Contains(ext, StringComparer.CurrentCultureIgnoreCase);
                if(!canSave)
                {
                    _log.LogWarning($"not allowing user {User.Identity.Name} to upload file {formFile.FileName} because the file extension is not allowed for less priviledged users");
                    continue;
                }

                try
                {
                    if (formFile.Length > 0)
                    {
                        var uploadResult = await _fileManagerService.ProcessFile(
                            formFile,
                            _autoUploadOptions,
                            resizeImages,
                            maxWidth,
                            maxHeight,
                            requestedFilePath,
                            newFileName,
                            allowRootPath,
                            createThumbnail,
                            keepOriginal
                            ).ConfigureAwait(false);

                        uploadList.Add(uploadResult);

                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(MediaLoggingEvents.FILE_PROCCESSING, ex, ex.StackTrace);
                }

            }

            foreach (var handler in _uploadHandlers)
            {
                try
                {
                    await handler.Handle(uploadList);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message}-{ex.StackTrace}");
                }
            }

            return Json(uploadList);
        }

        [HttpPost]
        [Authorize(Policy = "FileManagerPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CropServerImage(
            string sourceFilePath,
            int x1,
            int y1,
            int widthToCrop,
            int heightToCrop,
            int finalWidth,
            int finalHeight
            )
        {

            var result = await _fileManagerService.CropFile(
                _autoUploadOptions,
                sourceFilePath,
                x1,
                y1,
                widthToCrop,
                heightToCrop,
                finalWidth,
                finalHeight
                );

            var list = new List<UploadResult>()
            {
                result
            };

            foreach (var handler in _uploadHandlers)
            {
                try
                {
                    await handler.Handle(list);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message}-{ex.StackTrace}");
                }
            }

            return Json(result);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult js()
        {
            var baseSegment = "cloudscribe.FileManager.Web.js.";
           
            var requestPath = HttpContext.Request.Path.Value;
            _log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/filemanager/js/".Length) return NotFound();

            var seg = requestPath.Substring("/filemanager/js/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = _resourceHelper.GetMimeType(ext);

            return GetResult(baseSegment + seg, mimeType);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult css()
        {
            var baseSegment = "cloudscribe.FileManager.Web.css.";
           
            var requestPath = HttpContext.Request.Path.Value;
            _log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/filemanager/css/".Length) return NotFound();

            var seg = requestPath.Substring("/filemanager/css/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = _resourceHelper.GetMimeType(ext);

            return GetResult(baseSegment + seg, mimeType);
        }
        
        private IActionResult GetResult(string resourceName, string contentType)
        {
            var assembly = typeof(FileManagerController).GetTypeInfo().Assembly;
            resourceName = _resourceHelper.ResolveResourceIdentifier(resourceName);
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                _log.LogError("resource not found for " + resourceName);
                return NotFound();
            }
            
            _log.LogDebug("resource found for " + resourceName);
            
            var status = ETagGenerator.AddEtagForStream(HttpContext, resourceStream);
            if(status != null) { return status; } //304
            
            return new FileStreamResult(resourceStream, contentType);
        }

        [HttpPost]
        [Authorize(Policy = "FileManagerPolicy")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> CreateFolder(string currentVirtualPath, string newFolderName)
        {
            var result = await _fileManagerService.CreateFolder(currentVirtualPath, newFolderName).ConfigureAwait(false);
            return Json(result);

        }

        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFolder(string folderToDelete)
        {
            var result = await _fileManagerService.DeleteFolder(folderToDelete).ConfigureAwait(false);
            return Json(result);

        }

        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenameFolder(string folderToRename, string newNameSegment)
        {
            var result = await _fileManagerService.RenameFolder(folderToRename, newNameSegment).ConfigureAwait(false);
            return Json(result);

        }


        [HttpGet]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        public async Task<IActionResult> DownloadFile(string fileToDownload)
        {
            var result = await _fileManagerService.GetInfoForDownload(fileToDownload);
            if(result == null || string.IsNullOrEmpty(result.FileSystemPath))
            {
                return NotFound();
            }

            
            if(result.MimeType == "application/pdf")
            {
                return File(new FileStream(result.FileSystemPath, FileMode.Open), result.MimeType); //inline
            }

            return File(new FileStream(result.FileSystemPath, FileMode.Open), result.MimeType, result.FileName);

        }


        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFile(string fileToDelete)
        {
            var result = await _fileManagerService.DeleteFile(fileToDelete).ConfigureAwait(false);
            return Json(result);

        }

        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenameFile(string fileToRename, string newNameSegment)
        {
            var result = await _fileManagerService.RenameFile(fileToRename, newNameSegment).ConfigureAwait(false);
            return Json(result);

        }
       

        [HttpGet]
        [Authorize(Policy = "FileManagerPolicy")]
        public async Task<IActionResult> GetFileTreeJson(string fileType = "", string virtualStartPath = "")
        {
            var list = await _fileManagerService.GetFileTree(fileType, virtualStartPath).ConfigureAwait(false);

            return Json(list);
        }
    }
}
