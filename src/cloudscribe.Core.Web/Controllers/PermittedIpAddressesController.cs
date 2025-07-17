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
        public virtual async Task<IActionResult > PermittedIpAddresses(string? status, string? q, int pageNumber = 1, int pageSize = -1)
        {
            ViewData["Title"] = StringLocalizer["Permitted IP Addresses"];
            ViewBag.status = status;
            int itemsPerPage = UIOptions.DefaultPageSize_IpAddresses;
            string usersIpAddress = string.Empty;

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
                        BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.GetPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    var loc = await _userManager.GetUserLocations(User.GetUserSiteIdAsGuid(), User.GetUserIdAsGuid(), 1, 1);

                    foreach (var item in loc.Data)
                    {
                        usersIpAddress = item.IpAddress ?? StringLocalizer["Unknown"];
                    }

                    if (usersIpAddress != StringLocalizer["Unknown"])
                    {
                        if (usersIpAddress == "0.0.0.1")
                        {
                            usersIpAddress = "::1";
                        }
                    }

                    ViewBag.UsersIpAddress = usersIpAddress;

                    return View(permittedIps);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error getting permitted IP Addresses");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"Error: {e.Message}"]);
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"Error: There has been an error. Please try again later or contact your administrator."]);
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual async Task<IActionResult> AddPermittedIpAddress(string ipAddress, string? reason, string ipTypeRadio)
        {
            if (ipAddress.Length <= 0)
            {
                _log.LogError("IP Address missing");
                this.AlertWarning(StringLocalizer[$"Error: IP Address missing"]);

                return RedirectToAction("PermittedIpAddresses");
            }
            if (ipTypeRadio.Length <= 0)
            {
                _log.LogError("IP Type is required");
                this.AlertWarning(StringLocalizer[$"Error: The IP Type is required"]);

                return RedirectToAction("PermittedIpAddresses");
            }           

            if (ipTypeRadio == "singleIpAddress")
            {
                ValidationResult validationResult = ValidateIpAddress.IpAddressValidation(ipAddress);

                if (validationResult != null)
                {
                    _log.LogError($"{validationResult.ErrorMessage} {ipAddress}");
                    this.AlertWarning(StringLocalizer[$"Error: {validationResult.ErrorMessage}  {ipAddress}"]);

                    return RedirectToAction("PermittedIpAddresses");
                }
            }
            else
            {
                bool validIpRange = IPAddressRange.TryParse(ipAddress, out IPAddressRange ipRange);

                if (!validIpRange)
                {
                    _log.LogError($"Invalid IP Range {ipAddress}");
                    this.AlertWarning(StringLocalizer[$"Error: Invalid IP Address Range: { ipAddress}"]);

                    return RedirectToAction("PermittedIpAddresses");
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
                IsPermitted = true,
                IsRange = ipTypeRadio == "ipAddressRange" ? true : false
            };

            try
            {
                await _blockedOrPermittedIpService.AddPermittedIpAddress(ipAddressModel, CancellationToken.None);

                this.AlertSuccess(StringLocalizer[$"Success! IP Address added."]);

                return RedirectToAction("PermittedIpAddresses");
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error adding permitted IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"Error: {e.Message}"]);

                    return RedirectToAction("PermittedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("PermittedIpAddresses");
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
                this.AlertWarning(StringLocalizer[$"Error: The Model is invalid"]);

                return RedirectToAction("PermittedIpAddresses");
            }

            BlockedPermittedIpAddressesModel ipAddressModel;

            if (!model.IsRange)
            {
                ValidationResult validationResult = ValidateIpAddress.IpAddressValidation(model.IpAddress);

                if (validationResult != null)
                {
                    _log.LogError($"{validationResult.ErrorMessage} {model.IpAddress}");
                    this.AlertWarning(StringLocalizer[$"{validationResult.ErrorMessage}  {model.IpAddress}"]);

                    return RedirectToAction("PermittedIpAddresses");
                }

                ipAddressModel = new BlockedPermittedIpAddressesModel
                {
                    Id = model.Id,
                    IpAddress = model.IpAddress,
                    Reason = model.Reason,
                    CreatedDate = model.CreatedDate,
                    LastUpdated = DateTime.UtcNow,
                    SiteId = User.GetUserSiteIdAsGuid(),
                    IsPermitted = true,
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

                    return RedirectToAction("PermittedIpAddresses");
                }
                
                ipAddressModel = new BlockedPermittedIpAddressesModel
                {
                    Id = model.Id,
                    IpAddress = model.IpAddress,
                    Reason = model.Reason,
                    CreatedDate = model.CreatedDate,
                    LastUpdated = DateTime.UtcNow,
                    SiteId = User.GetUserSiteIdAsGuid(),
                    IsPermitted = true,
                    IsRange = true
                };
            }

            try
            {
                await _blockedOrPermittedIpService.UpdatePermittedIpAddress(ipAddressModel, CancellationToken.None);
                this.AlertSuccess(StringLocalizer[$"Success! IP Address updated."]);

                return RedirectToAction("PermittedIpAddresses");
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error updating permitted IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("PermittedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("PermittedIpAddresses");
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
                this.AlertWarning(StringLocalizer[$"Error: Invalid IP Address"]);

                return RedirectToAction("PermittedIpAddresses");
            }

            Guid siteId = User.GetUserSiteIdAsGuid();

            if (siteId == Guid.Empty)
            {
                _log.LogError("Invalid Site ID");
                this.AlertWarning(StringLocalizer[$"Error: Invalid Site ID"]);

                return RedirectToAction("PermittedIpAddresses");
            }

            Guid parsedId = new Guid(ipAddressId.Replace("_", "-"));

            try
            {
                await _blockedOrPermittedIpService.DeletePermittedIpAddress(parsedId, siteId, CancellationToken.None);
                this.AlertSuccess(StringLocalizer[$"Success! The IP Address has been removed"]);

                return RedirectToAction("PermittedIpAddresses");
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, "Error deleting IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("PermittedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error.Please try again later or contact your administrator."]);

                    return RedirectToAction("PermittedIpAddresses");
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
                        BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.GetPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                    };

                    return View("PermittedIpAddresses", permittedIps);
                }
                catch (Exception e)
                {
                    _log.LogError(e, "Error searching permitted IP Addresses");

                    if (e.Message != null)
                    {
                        this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                        return RedirectToAction("PermittedIpAddresses");
                    }
                    else
                    {
                        this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                        return RedirectToAction("PermittedIpAddresses");
                    }
                }
            }

            try
            {
                permittedIps = new PaginatedIpAddressesViewModel
                {
                    BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.SearchPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, searchTerm, CancellationToken.None)
                };
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error searching permitted IP Address");

                if (e.Message != null)
                {
                    this.AlertDanger(StringLocalizer[$"{e.Message}"]);

                    return RedirectToAction("PermittedIpAddresses");
                }
                else
                {
                    this.AlertDanger(StringLocalizer[$"There has been an error. Please try again later or contact your administrator."]);

                    return RedirectToAction("PermittedIpAddresses");
                }
            }

            if (permittedIps.BlockedPermittedIpAddresses.Data.Count == 0)
            {
                ViewBag.status = StringLocalizer["No results found for the search term. Showing all IP Addresses"];

                permittedIps = new PaginatedIpAddressesViewModel
                {
                    BlockedPermittedIpAddresses = await _blockedOrPermittedIpService.GetPermittedIpAddressesAsync(User.GetUserSiteIdAsGuid(), pageNumber, itemsPerPage, CancellationToken.None)
                };

                return View("PermittedIpAddresses", permittedIps);
            }
            else
            {
                permittedIps.SearchTerm = searchTerm;
                ViewBag.status = StringLocalizer[$"{permittedIps.BlockedPermittedIpAddresses.TotalItems} result(s) found for '{searchTerm}'"];

                return View("PermittedIpAddresses", permittedIps);
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
                this.AlertWarning(StringLocalizer[$"Error: The Model is invalid"]);

                return RedirectToAction("PermittedIpAddresses");
            }

            Stream ipAddresses = model.BulkIpAddresses.OpenReadStream();

            if (ipAddresses == null)
            {
                _log.LogError("No IP Addresses found in the uploaded file");
                this.AlertWarning(StringLocalizer[$"Error: No IP Addresses found in the uploaded file"]);

                return RedirectToAction("PermittedIpAddresses");
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
                            IsPermitted = true,
                            IsRange = isRange
                        };

                        try
                        {
                            await _blockedOrPermittedIpService.AddPermittedIpAddress(ipAddressModel, CancellationToken.None);
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
            if (errors.Count > 0 && errors.Count <= 10)
            {
                string errorSummary = string.Join("; ", errors);
                this.AlertWarning(StringLocalizer[$"Added {successCount} IP(s). Errors: {errorSummary}"]);

                return RedirectToAction("PermittedIpAddresses");
            }
            else if (errors.Count > 10)
            {
                string errorSummary = string.Join("; ", errors.GetRange(0, 10));
                this.AlertWarning(StringLocalizer[$"Added {successCount} IP(s). First 10 errors: {errorSummary}"]);

                return RedirectToAction("PermittedIpAddresses");
            }
            else
            {
                this.AlertSuccess(StringLocalizer[$"Success! {successCount} IP Address(es) have been added."]);

                return RedirectToAction("PermittedIpAddresses");
            }
        }
    }
}