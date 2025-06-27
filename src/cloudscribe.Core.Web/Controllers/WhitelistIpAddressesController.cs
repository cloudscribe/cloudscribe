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
    public class WhitelistIpAddressesController : Controller
    {
        protected IStringLocalizer StringLocalizer { get; private set; }
        protected SiteUserManager<SiteUser> UserManager { get; private set; }
        protected ISiteContext CurrentSite { get; private set; }
        protected IWhitelistService _whitelistService;
        protected UIOptions UIOptions { get; private set; }
        private ILogger _log;

        public WhitelistIpAddressesController(IStringLocalizer<CloudscribeCore> localizer, SiteContext currentSite, SiteUserManager<SiteUser> userManager, IWhitelistService whitelistService, ILogger<WhitelistIpAddressesController> logger, IOptions<UIOptions> uiOptionsAccessor)
        {
            StringLocalizer = localizer;
            CurrentSite = currentSite;
            UserManager = userManager;
            _whitelistService = whitelistService;
            _log = logger;
            UIOptions = uiOptionsAccessor.Value;
        }

        [Authorize(Policy = PolicyConstants.AdminMenuPolicy)]
        public virtual async Task<IActionResult > Index(string? status, string? q, int pageNumber = 1, int pageSize = -1)
        {
            ViewData["Title"] = StringLocalizer["Whilelist IP Addresses"];
            ViewBag.status = status;
            int itemsPerPage = UIOptions.DefaultPageSize_IpAddresses;

            if (pageSize > 0)
                itemsPerPage = pageSize;

            try
            {
                if (q != null)
                {
                    return await SearchWhitelistedIpAddresses(q, pageNumber, pageSize);
                }
                else
                {
                    PaginatedIpAddressesViewModel whiteListedIps = new PaginatedIpAddressesViewModel
                    {
                        BlackWhitelistIpAddresses = await _whitelistService.GetWhitelistedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    var loc = await UserManager.GetUserLocations(User.GetUserSiteIdAsGuid(), User.GetUserIdAsGuid(), 1, 1);

                    foreach (var item in loc.Data)
                    {
                        ViewBag.UsersIpAddress = item.IpAddress ?? "Unknown";
                    }

                    return View(whiteListedIps);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error getting whitelisted IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = "There has been an error. Please try again later or contact your administrator." });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> AddWhitelistedIpAddress(IpAddressesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                return RedirectToAction("Index", new { status = "Error: The Model is invalid" });
            }

            BlackWhiteListedIpAddressesModel ipAddressModel = new BlackWhiteListedIpAddressesModel
            {
                Id = Guid.NewGuid(),
                IpAddress = model.IpAddress,
                Reason = model.Reason,
                CreatedDate = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                SiteId = User.GetUserSiteIdAsGuid(),
                IsWhitelisted = true
            };
            try
            {
                await _whitelistService.AddWhitelistedIpAddress(ipAddressModel, CancellationToken.None);

                return RedirectToAction("Index", new { status = "Success! IP Address added." });
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error adding whitelisted IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = "There has been an error. Please try again later or contact your administrator." });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> UpdateWhitelistedIpAddress(IpAddressesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                return RedirectToAction("Index", new { status = "Error: The Model is invalid" });
            }

            BlackWhiteListedIpAddressesModel ipAddressModel = new BlackWhiteListedIpAddressesModel
            {
                Id = model.Id,
                IpAddress = model.IpAddress,
                Reason = model.Reason,
                CreatedDate = model.CreatedDate,
                LastUpdated = DateTime.UtcNow,
                SiteId = User.GetUserSiteIdAsGuid(),
                IsWhitelisted = true
            };

            try
            {
                await _whitelistService.UpdateWhitelistedIpAddress(ipAddressModel, CancellationToken.None);

                return RedirectToAction("Index", new { status = "Success! IP Address updated." });
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error updating whitelisted IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = "There has been an error. Please try again later or contact your administrator." });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> DeleteWhitelistedIpAddress(string ipAddressId)
        {
            if (ipAddressId == string.Empty)
            {
                _log.LogError("Invalid IP Address");
                return RedirectToAction("Index", new { status = "Error: Invalid IP Address" });
            }

            Guid siteId = User.GetUserSiteIdAsGuid();

            if (siteId == Guid.Empty)
            {
                _log.LogError("Invalid Site ID");
                return RedirectToAction("Index", new { status = "Error: Invalid Site ID" });
            }

            Guid parsedId = new Guid(ipAddressId.Replace("_", "-"));

            try
            {
                await _whitelistService.DeleteWhitelistedIpAddress(parsedId, siteId, CancellationToken.None);

                return RedirectToAction("Index", new { status = "Success! The IP Address has been removed" });
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
                    return RedirectToAction("Index", new { status = "There has been an error.Please try again later or contact your administrator." });
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public async virtual Task<IActionResult> SearchWhitelistedIpAddresses(string searchTerm, int pageNumber = 1, int pageSize = -1, CancellationToken cancellationToken = default)
        {
            PaginatedIpAddressesViewModel whitelistedIps;
            int itemsPerPage = UIOptions.DefaultPageSize_IpAddresses;

            if (pageSize > 0)
                itemsPerPage = pageSize;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ViewBag.status = "Please enter a search term";

                try
                {
                    whitelistedIps = new PaginatedIpAddressesViewModel
                    {
                        BlackWhitelistIpAddresses = await _whitelistService.GetWhitelistedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    return View("Index", whitelistedIps);
                }
                catch (Exception e)
                {
                    _log.LogError(e, "Error searching whitelisted IP Address");

                    if (e.Message != null)
                    {
                        return RedirectToAction("Index", new { status = $"{e.Message}" });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { status = "There has been an error. Please try again later or contact your administrator." });
                    }
                }
            }

            try
            {
                whitelistedIps = new PaginatedIpAddressesViewModel
                {
                    BlackWhitelistIpAddresses = await _whitelistService.SearchWhitelistedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, searchTerm, CancellationToken.None)
                };
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error searching whitelisted IP Address");

                if (e.Message != null)
                {
                    return RedirectToAction("Index", new { status = $"{e.Message}" });
                }
                else
                {
                    return RedirectToAction("Index", new { status = "There has been an error. Please try again later or contact your administrator." });
                }
            }

            if (whitelistedIps.BlackWhitelistIpAddresses.Data.Count == 0)
            {
                ViewBag.status = "No results found for the search term. Showing all IP Addresses";

                whitelistedIps = new PaginatedIpAddressesViewModel
                {
                    BlackWhitelistIpAddresses = await _whitelistService.GetWhitelistedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                };

                return View("Index", whitelistedIps);
            }
            else
            {
                whitelistedIps.SearchTerm = searchTerm;
                ViewBag.status = $"{whitelistedIps.BlackWhitelistIpAddresses.TotalItems} result(s) found for '{searchTerm}'";

                return View("Index", whitelistedIps);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public async Task<IActionResult> BulkUploadWhitelistedIpAddress(BulkUploadIpAddressesModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                return RedirectToAction("Index", new { status = "Error: The Model is invalid" });
            }

            Stream ipAddresses = model.BulkIpAddresses.OpenReadStream();

            if (ipAddresses == null)
            {
                _log.LogError("No IP Addresses found in the uploaded file");
                return RedirectToAction("Index", new { status = "Error: No IP Addresses found in the uploaded file" });
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

                            var ipAddressModel = new BlackWhiteListedIpAddressesModel
                            {
                                Id = Guid.NewGuid(),
                                IpAddress = ip,
                                Reason = string.IsNullOrEmpty(reason) ? string.Empty : reason,
                                CreatedDate = DateTime.UtcNow,
                                LastUpdated = DateTime.UtcNow,
                                SiteId = User.GetUserSiteIdAsGuid(),
                                IsWhitelisted = true
                            };

                            try
                            {
                                await _whitelistService.AddWhitelistedIpAddress(ipAddressModel, CancellationToken.None);
                                successCount++;
                            }
                            catch (Exception e)
                            {
                                errors.Add($"Error adding {ip}: {e.Message}");
                                _log.LogError(e, $"Error adding whitelisted IP Address from bulk upload: {ip}");
                            }
                        }
                    }
                }
            }
            if (errors.Count > 0)
            {
                string errorSummary = string.Join("; ", errors);

                return RedirectToAction("Index", new { status = $"Added {successCount} IP(s). Errors: {errorSummary}" });
            }
            else
            {
                return RedirectToAction("Index", new { status = $"Success! {successCount} IP Address(es) have been added." });
            }
        }
    }
}