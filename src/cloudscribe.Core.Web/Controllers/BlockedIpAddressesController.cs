using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.IpAddresses;
using cloudscribe.Web.Common.Extensions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    public partial class SiteAdminController : Controller
    {
        [HttpGet]
        [Authorize(Policy = PolicyConstants.AdminMenuPolicy)]
        public virtual async Task<IActionResult> BlockedIpAddresses(string? status, string? q, int pageNumber = 1, int pageSize = -1)
        {
            ViewData["Title"] = StringLocalizer["Blocked IP Addresses"];
            ViewBag.status = status;
            int itemsPerPage = UIOptions.DefaultPageSize_IpAddresses;
            string usersIpAddress = string.Empty;

            if (pageSize > 0)
                itemsPerPage = pageSize;

            try
            {
                if (q != null)
                {
                    return await SearchBlockedIpAddresses(q, pageNumber, pageSize);
                }
                else
                {
                    PaginatedIpAddressesViewModel blockedIps = new PaginatedIpAddressesViewModel
                    {
                        BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.GetBlockedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    var loc = await _userManager.GetUserLocations(User.GetUserSiteIdAsGuid(), User.GetUserIdAsGuid(), 1, 1);

                    foreach (var item in loc.Data)
                    {
                        usersIpAddress = item.IpAddress.ToString() ?? StringLocalizer["Unknown"];
                    }

                    if (usersIpAddress != StringLocalizer["Unknown"])
                    {
                        if (usersIpAddress == "0.0.0.1")
                        {
                            usersIpAddress = "::1";
                        }
                    }

                    ViewBag.UsersIpAddress = usersIpAddress;

                    return View(blockedIps);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error getting blocked IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> AddBlockedIpAddress(string ipAddress, string? reason, string ipTypeRadio)
        {
            if (ipAddress.Length <= 0)
            {
                _log.LogError("IP Address missing");
                this.AlertWarning(StringLocalizer[$"Error: IP Address missing"]);

                return RedirectToAction("BlockedIpAddresses");
            }
            if (ipTypeRadio.Length <= 0)
            {
                _log.LogError("IP Type is required");
                this.AlertWarning(StringLocalizer[$"Error: The IP Type is required"]);

                return RedirectToAction("BlockedIpAddresses");
            }

            if (ipTypeRadio == "singleIpAddress")
            {
                ValidationResult validationResult = ValidateIpAddress.IpAddressValidation(ipAddress);

                if (validationResult != null)
                {
                    _log.LogError($"{validationResult.ErrorMessage} {ipAddress}");
                    this.AlertWarning(StringLocalizer[$"{validationResult.ErrorMessage}  {ipAddress}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }
            }
            else
            {
                bool validIpRange = IPAddressRange.TryParse(ipAddress, out IPAddressRange ipRange);

                if (!validIpRange)
                {
                    _log.LogError($"Invalid IP Range {ipAddress}");
                    this.AlertWarning(StringLocalizer[$"Invalid IP Address Range: {ipAddress}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }
            }

            BlockedPermittedIpAddressesModel ipAddressModel = new BlockedPermittedIpAddressesModel
            {
                Id = Guid.NewGuid(),
                IpAddress = ipAddress,
                Reason = reason,
                CreatedDate = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                SiteId = User.GetUserSiteIdAsGuid(),
                IsPermitted = false,
                IsRange = ipTypeRadio == "ipAddressRange" ? true : false
            };

            try
            {
                await _blockedOrPermittedIpService.AddBlockedIpAddress(ipAddressModel, CurrentSite.Id, CancellationToken.None);

                this.AlertSuccess(StringLocalizer[$"Success! IP Address has been added."]);

                return RedirectToAction("BlockedIpAddresses");
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error adding blocked IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("BlockedIpAddresses");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> UpdateBlockedIpAddress(IpAddressesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                this.AlertWarning(StringLocalizer[$"Error: The Model is invalid"]);

                return RedirectToAction("BlockedIpAddresses");
            }

            BlockedPermittedIpAddressesModel ipAddressModel;

            if (!model.IsRange)
            {
                ValidationResult validationResult = ValidateIpAddress.IpAddressValidation(model.IpAddress);

                if (validationResult != null)
                {
                    _log.LogError($"{validationResult.ErrorMessage} {model.IpAddress}");
                    this.AlertWarning(StringLocalizer[$"{validationResult.ErrorMessage}  {model.IpAddress}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }

                ipAddressModel = new BlockedPermittedIpAddressesModel
                {
                    Id = model.Id,
                    IpAddress = model.IpAddress,
                    Reason = model.Reason,
                    CreatedDate = model.CreatedDate,
                    LastUpdated = DateTime.UtcNow,
                    SiteId = User.GetUserSiteIdAsGuid(),
                    IsPermitted = false,
                    IsRange = false
                };
            }
            else
            {
                bool validIpRange = IPAddressRange.TryParse(model.IpAddress, out IPAddressRange ipRange);

                if (!validIpRange)
                {
                    _log.LogError($"Invalid IP Range {model.IpAddress}");
                    this.AlertWarning(StringLocalizer[$"Invalid IP Address Range: {model.IpAddress}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }

                ipAddressModel = new BlockedPermittedIpAddressesModel
                {
                    Id = model.Id,
                    IpAddress = model.IpAddress,
                    Reason = model.Reason,
                    CreatedDate = model.CreatedDate,
                    LastUpdated = DateTime.UtcNow,
                    SiteId = User.GetUserSiteIdAsGuid(),
                    IsPermitted = false,
                    IsRange = true
                };
            }

            try
            {
                await _blockedOrPermittedIpService.UpdateBlockedIpAddress(ipAddressModel, CurrentSite.Id, CancellationToken.None);
                this.AlertSuccess(StringLocalizer[$"Success! IP Address updated."]);

                return RedirectToAction("BlockedIpAddresses");
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error updating blocked IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("BlockedIpAddresses");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> DeleteBlockedIpAddress(string ipAddressId)
        {
            if (ipAddressId == string.Empty)
            {
                _log.LogError("Invalid IP Address");
                this.AlertWarning(StringLocalizer[$"Error: Invalid IP Address"]);

                return RedirectToAction("BlockedIpAddresses");
            }

            Guid siteId = User.GetUserSiteIdAsGuid();

            if (siteId == Guid.Empty)
            {
                _log.LogError("Invalid Site ID");
                this.AlertWarning(StringLocalizer[$"Error: Invalid Site ID"]);

                return RedirectToAction("BlockedIpAddresses");
            }

            Guid parsedId = new Guid(ipAddressId.Replace("_", "-"));

            try
            {
                await _blockedOrPermittedIpService.DeleteBlockedIpAddress(parsedId, siteId, CancellationToken.None);
                this.AlertSuccess(StringLocalizer[$"Success! The IP Address has been removed"]);

                return RedirectToAction("BlockedIpAddresses");
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, "Error deleting IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("BlockedIpAddresses");
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public async virtual Task<IActionResult> SearchBlockedIpAddresses(string searchTerm, int pageNumber = 1, int pageSize = -1, CancellationToken cancellationToken = default)
        {
            PaginatedIpAddressesViewModel blockedIps;
            int itemsPerPage = UIOptions.DefaultPageSize_IpAddresses;

            if (pageSize > 0)
                itemsPerPage = pageSize;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ViewBag.status = StringLocalizer["Please enter a search term"];

                try
                {
                    blockedIps = new PaginatedIpAddressesViewModel
                    {
                        BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.GetBlockedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    return View("Index", blockedIps);
                }
                catch (Exception e)
                {
                    _log.LogError(e, "Error searching blocked IP Addresses");

                    if (e.Message != null)
                    {
                        this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                        return RedirectToAction("BlockedIpAddresses");
                    }
                    else
                    {
                        this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                        return RedirectToAction("BlockedIpAddresses");
                    }
                }
            }

            try
            {
                blockedIps = new PaginatedIpAddressesViewModel
                {
                    BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.SearchBlockedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, searchTerm, CancellationToken.None)
                };
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error searching blocked IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("BlockedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("BlockedIpAddresses");
                }
            }

            if (blockedIps.BlockedPermittedIpAddresses.Data.Count == 0)
            {
                ViewBag.status = StringLocalizer["No results found for the search term. Showing all IP Addresses"];

                blockedIps = new PaginatedIpAddressesViewModel
                {
                    BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.GetBlockedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                };

                return View("BlockedIpAddresses", blockedIps);
            }
            else
            {
                blockedIps.SearchTerm = searchTerm;
                ViewBag.status = StringLocalizer[$"{blockedIps.BlockedPermittedIpAddresses.TotalItems} result(s) found for '{searchTerm}'"];

                return View("BlockedIpAddresses", blockedIps);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public async Task<IActionResult> BulkUploadBlockedIpAddress(BulkUploadIpAddressesModel model)
        {
            if (!ModelState.IsValid)
            {
                _log.LogError("Model is invalid");
                this.AlertWarning(StringLocalizer[$"Error: The Model is invalid"]);

                return RedirectToAction("BlockedIpAddresses");
            }

            Stream ipAddresses = model.BulkIpAddresses.OpenReadStream();

            if (ipAddresses == null)
            {
                _log.LogError("No IP Addresses found in the uploaded file");
                this.AlertWarning(StringLocalizer[$"Error: No IP Addresses found in the uploaded file"]);

                return RedirectToAction("BlockedIpAddresses");
            }

            var errors = new List<string>();
            int successCount = 0;
            bool isRange = false;

            CsvConfiguration csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                MissingFieldFound = null
            };

            using (StreamReader reader = new StreamReader(ipAddresses))
            using (var csv = new CsvReader(reader, csvReaderConfig))
            {
                while (await csv.ReadAsync())
                {
                    string ip = csv.GetField(0)?.Trim();
                    string reason = csv.GetField(1)?.Trim().Length > 1 ? csv.GetField(1)?.Trim() : string.Empty;

                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        ValidationResult validationResult = ValidateIpAddress.IpAddressValidation(ip);

                        if (validationResult == null)
                        {
                            isRange = false;
                        }
                        else
                        {
                            bool validIpRange = IPAddressRange.TryParse(ip, out IPAddressRange ipRange);

                            if (validIpRange)
                            {
                                isRange = true;
                            }
                            else
                            {
                                errors.Add($"Invalid IP Address or Range: {ip}");
                                _log.LogError($"Invalid IP Address or Range: {ip}");
                                continue;
                            }
                        }

                        var ipAddressModel = new BlockedPermittedIpAddressesModel
                        {
                            Id = Guid.NewGuid(),
                            IpAddress = ip,
                            Reason = string.IsNullOrEmpty(reason) ? string.Empty : reason,
                            CreatedDate = DateTime.UtcNow,
                            LastUpdated = DateTime.UtcNow,
                            SiteId = User.GetUserSiteIdAsGuid(),
                            IsPermitted = false,
                            IsRange = isRange
                        };

                        try
                        {
                            await _blockedOrPermittedIpService.AddBlockedIpAddress(ipAddressModel, CurrentSite.Id, CancellationToken.None);
                            successCount++;
                        }
                        catch (Exception e)
                        {
                            errors.Add($"Error adding {ip}: {e.Message}");
                            _log.LogError(e, $"Error adding blocked IP Address from bulk upload: {ip}");
                        }
                    }
                }
            }
            if (errors.Count > 0 && errors.Count <= 10)
            {
                string errorSummary = string.Join("; ", errors);
                this.AlertWarning(StringLocalizer[$"Added {successCount} IP(s). Errors: {errorSummary}"]);

                return RedirectToAction("BlockedIpAddresses");
            }
            else if (errors.Count > 10)
            {
                string errorSummary = string.Join("; ", errors.GetRange(0, 10));
                this.AlertWarning(StringLocalizer[$"Added {successCount} IP(s). First 10 errors: {errorSummary}"]);

                return RedirectToAction("BlockedIpAddresses");
            }
            else
            {
                this.AlertSuccess(StringLocalizer[$"Success! {successCount} IP Address(es) have been added."]);

                return RedirectToAction("BlockedIpAddresses");
            }
        }
    }
}