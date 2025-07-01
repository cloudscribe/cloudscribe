using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.IpAddresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    public class PermittedIpAddressesController : Controller
    {
        protected IStringLocalizer StringLocalizer { get; private set; }
        protected SiteUserManager<SiteUser> UserManager { get; private set; }
        protected ISiteContext CurrentSite { get; private set; }
        protected IPermittedIpService _permittedIpService;
        protected UIOptions UIOptions { get; private set; }
        private ILogger _log;

        public PermittedIpAddressesController(IStringLocalizer<CloudscribeCore> localizer, SiteContext currentSite, SiteUserManager<SiteUser> userManager, IPermittedIpService permittedIpService, ILogger<PermittedIpAddressesController> logger, IOptions<UIOptions> uiOptionsAccessor)
        {
            StringLocalizer = localizer;
            CurrentSite = currentSite;
            UserManager = userManager;
            _permittedIpService = permittedIpService;
            _log = logger;
            UIOptions = uiOptionsAccessor.Value;
        }

        [Authorize(Policy = PolicyConstants.AdminMenuPolicy)]
        public virtual async Task<IActionResult > Index(string? status, string? q, int pageNumber = 1, int pageSize = -1)
        {
            ViewData["Title"] = StringLocalizer["Permitted IP Addresses"];
            ViewBag.status = status;
            int itemsPerPage = UIOptions.DefaultPageSize_IpAddresses;

            if (pageSize > 0)
                itemsPerPage = pageSize;

            try
            {
                if (q != null)
                {
                    return await SearchPermittedIpAddresses(q, pageNumber, pageSize);
                }
                else
                {
                    PaginatedIpAddressesViewModel permittedIps = new PaginatedIpAddressesViewModel
                    {
                        BlockedPermittedIpAddresses = await _permittedIpService.GetPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    var loc = await UserManager.GetUserLocations(User.GetUserSiteIdAsGuid(), User.GetUserIdAsGuid(), 1, 1);

                    foreach (var item in loc.Data)
                    {
                        ViewBag.UsersIpAddress = item.IpAddress ?? StringLocalizer["Unknown"];
                    }

                    return View(permittedIps);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error getting permitted IP Addresses");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = StringLocalizer["There has been an error. Please try again later or contact your administrator."] });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> AddPermittedIpAddress(IpAddressesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                return RedirectToAction("Index", new { status = StringLocalizer["Error: The Model is invalid"] });
            }

            BlockedPermittedIpAddressesModel ipAddressModel = new BlockedPermittedIpAddressesModel
            {
                Id = Guid.NewGuid(),
                IpAddress = model.IpAddress,
                Reason = model.Reason,
                CreatedDate = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                SiteId = User.GetUserSiteIdAsGuid(),
                IsPermitted = true
            };
            try
            {
                await _permittedIpService.AddPermittedIpAddress(ipAddressModel, CancellationToken.None);

                return RedirectToAction("Index", new { status = StringLocalizer["Success! IP Address added."] });
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error adding permitted IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = StringLocalizer["There has been an error. Please try again later or contact your administrator."] });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> UpdatePermittedIpAddress(IpAddressesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                return RedirectToAction("Index", new { status = StringLocalizer["Error: The Model is invalid"] });
            }

            BlockedPermittedIpAddressesModel ipAddressModel = new BlockedPermittedIpAddressesModel
            {
                Id = model.Id,
                IpAddress = model.IpAddress,
                Reason = model.Reason,
                CreatedDate = model.CreatedDate,
                LastUpdated = DateTime.UtcNow,
                SiteId = User.GetUserSiteIdAsGuid(),
                IsPermitted = true
            };

            try
            {
                await _permittedIpService.UpdatePermittedIpAddress(ipAddressModel, CancellationToken.None);

                return RedirectToAction("Index", new { status = StringLocalizer["Success! IP Address updated."] });
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error updating permitted IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = StringLocalizer["There has been an error. Please try again later or contact your administrator."] });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> DeletePermittedIpAddress(string ipAddressId)
        {
            if (ipAddressId == string.Empty)
            {
                _log.LogError("Invalid IP Address");
                return RedirectToAction("Index", new { status = StringLocalizer["Error: Invalid IP Address"] });
            }

            Guid siteId = User.GetUserSiteIdAsGuid();

            if (siteId == Guid.Empty)
            {
                _log.LogError("Invalid Site ID");
                return RedirectToAction("Index", new { status = StringLocalizer["Error: Invalid Site ID"] });
            }

            Guid parsedId = new Guid(ipAddressId.Replace("_", "-"));

            try
            {
                await _permittedIpService.DeletePermittedIpAddress(parsedId, siteId, CancellationToken.None);

                return RedirectToAction("Index", new { status = StringLocalizer["Success! The IP Address has been removed"] });
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, "Error deleting IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = StringLocalizer["There has been an error.Please try again later or contact your administrator."] });
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public async virtual Task<IActionResult> SearchPermittedIpAddresses(string searchTerm, int pageNumber = 1, int pageSize = -1, CancellationToken cancellationToken = default)
        {
            PaginatedIpAddressesViewModel permittedIps;
            int itemsPerPage = UIOptions.DefaultPageSize_IpAddresses;

            if (pageSize > 0)
                itemsPerPage = pageSize;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ViewBag.status = StringLocalizer["Please enter a search term"];

                try
                {
                    permittedIps = new PaginatedIpAddressesViewModel
                    {
                        BlockedPermittedIpAddresses = await _permittedIpService.GetPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    return View("Index", permittedIps);
                }
                catch (Exception e)
                {
                    _log.LogError(e, "Error searching permitted IP Addresses");

                    if (e.Message != null)
                    {
                        return RedirectToAction("Index", new { status = $"{e.Message}" });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { status = StringLocalizer["There has been an error. Please try again later or contact your administrator."] });
                    }
                }
            }

            try
            {
                permittedIps = new PaginatedIpAddressesViewModel
                {
                    BlockedPermittedIpAddresses = await _permittedIpService.SearchPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, searchTerm, CancellationToken.None)
                };
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error searching permitted IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = StringLocalizer["There has been an error. Please try again later or contact your administrator."] });
                }
            }

            if (permittedIps.BlockedPermittedIpAddresses.Data.Count == 0)
            {
                ViewBag.status = StringLocalizer["No results found for the search term. Showing all IP Addresses"];

                permittedIps = new PaginatedIpAddressesViewModel
                {
                    BlockedPermittedIpAddresses = await _permittedIpService.GetPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                };

                return View("Index", permittedIps);
            }
            else
            {
                permittedIps.SearchTerm = searchTerm;
                ViewBag.status = StringLocalizer[$"{permittedIps.BlockedPermittedIpAddresses.TotalItems} result(s) found for '{searchTerm}'"];

                return View("Index", permittedIps);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public async Task<IActionResult> BulkUploadPermittedIpAddress(BulkUploadIpAddressesModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                return RedirectToAction("Index", new { status = StringLocalizer["Error: The Model is invalid"] });
            }

            Stream ipAddresses = model.BulkIpAddresses.OpenReadStream();

            if (ipAddresses == null)
            {
                _log.LogError("No IP Addresses found in the uploaded file");
                return RedirectToAction("Index", new { status = StringLocalizer["Error: No IP Addresses found in the uploaded file"] });
            }

            var errors = new List<string>();
            int successCount = 0;

            using (StreamReader reader = new StreamReader(ipAddresses))
            {
                string content = await reader.ReadToEndAsync();

                if (!string.IsNullOrWhiteSpace(content))
                {
                    string[] entries = content.Split(new[] { ',' }, StringSplitOptions.None);

                    for (int i = 0; i < entries.Length; i += 2)
                    {
                        string ip = entries[i]?.Trim();
                        string reason = (i + 1 < entries.Length) ? entries[i + 1]?.Trim() : string.Empty;

                        if (!string.IsNullOrWhiteSpace(ip))
                        {
                            ValidationResult validationResult = ValidateIpAddress.IpAddressValidation(ip);

                            if (validationResult != null)
                            {
                                errors.Add($"{validationResult.ErrorMessage} {ip}");
                                _log.LogError($"{validationResult.ErrorMessage} {ip}");
                                continue;
                            }

                            var ipAddressModel = new BlockedPermittedIpAddressesModel
                            {
                                Id = Guid.NewGuid(),
                                IpAddress = ip,
                                Reason = string.IsNullOrEmpty(reason) ? string.Empty : reason,
                                CreatedDate = DateTime.UtcNow,
                                LastUpdated = DateTime.UtcNow,
                                SiteId = User.GetUserSiteIdAsGuid(),
                                IsPermitted = true
                            };

                            try
                            {
                                await _permittedIpService.AddPermittedIpAddress(ipAddressModel, CancellationToken.None);
                                successCount++;
                            }
                            catch (Exception e)
                            {
                                errors.Add($"Error adding {ip}: {e.Message}");
                                _log.LogError(e, $"Error adding permitted IP Address from bulk upload: {ip}");
                            }
                        }
                    }
                }
            }
            if (errors.Count > 0 && errors.Count <= 10)
            {
                string errorSummary = string.Join("; ", errors);

                return RedirectToAction("Index", new { status = StringLocalizer[$"Added {successCount} IP(s). Errors: {errorSummary}"] });
            }
            else if (errors.Count > 10)
            {
                string errorSummary = string.Join("; ", errors.GetRange(0, 10));

                return RedirectToAction("Index", new { status = StringLocalizer[$"Added {successCount} IP(s). First 10 errors: {errorSummary}"] });
            }
            else
            {
                return RedirectToAction("Index", new { status = StringLocalizer[$"Success! {successCount} IP Address(es) have been added."] });
            }
        }
    }
}