// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2018-03-14
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.CoreData;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    
    public class CoreDataController : Controller
    {
        public CoreDataController(
            SiteContext currentSite,
            GeoDataManager geoDataManager,
            IStringLocalizer<CloudscribeCore> localizer,
            IOptions<UIOptions> uiOptionsAccessor
            )
        {
            _currentSite = currentSite; 
            _dataManager = geoDataManager;
            _uiOptions = uiOptionsAccessor.Value;
            _sr = localizer;
        }

        private ISiteContext _currentSite;
        private GeoDataManager _dataManager;
        private UIOptions _uiOptions;
        private IStringLocalizer _sr;

        // GET: /CoreData/
        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "CoreDataPolicy")]
        [HttpGet]
        public async Task<IActionResult> CountryListPage(
            int pageNumber = 1,
            int pageSize = -1)
        {
            var itemsPerPage = _uiOptions.DefaultPageSize_CountryList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new CountryListPageViewModel
            {
                Countries = await _dataManager.GetCountriesPage(pageNumber, itemsPerPage)
            };

            return View(model);
        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpGet]
        public async Task<IActionResult> CountryEdit(
            Guid? countryId,
            int returnPageNumber = 1
            )
        {
            GeoCountryViewModel model;
            if ((countryId != null) && (countryId.Value != Guid.Empty))
            {
                ViewData["Title"] = _sr["Edit Country"];
                var country = await _dataManager.FetchCountry(countryId.Value);
                model = GeoCountryViewModel.FromIGeoCountry(country);

                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
                {
                    KeyToAdjust = "CountryEdit",
                    AdjustedText = _sr["Edit Country"],
                    ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
                };
                currentCrumbAdjuster.AddToContext();
            }
            else
            {
                ViewData["Title"] = _sr["New Country"];
                model = new GeoCountryViewModel();
            }

            model.ReturnPageNumber = returnPageNumber;
            
            return View(model);
        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryEdit(
            GeoCountryViewModel model,
            int returnPageNumber = 1)
        {
            ViewData["Title"] = _sr["Edit Country"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
          
            string successFormat;
            if (model.Id == Guid.Empty)
            {
                successFormat = _sr["The country {0} was successfully created."];
                await _dataManager.Add(model);
            }
            else
            {
                successFormat = _sr["The country {0} was successfully updated."];
                await _dataManager.Update(model);
            }
            
            this.AlertSuccess(string.Format(successFormat,
                        model.Name), true);
            
            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber });

        }


        // probably should hide by config by default
        // seems like an unusual event to delete a country and its states
        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryDelete(Guid countryId, int returnPageNumber = 1)
        {
            var country = await _dataManager.FetchCountry(countryId);
            
            if (country != null)
            {
                await _dataManager.DeleteCountry(country);
                
                this.AlertWarning(string.Format(
                        _sr["The country {0} was successfully deleted."],
                        country.Name)
                        , true);     
            }

            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber });
        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpGet]
        public async Task<IActionResult> StateListPage(
            Guid? countryId,
            int pageNumber = 1,
            int pageSize = -1,
            int crp = 1 
            )
        {
            if (!countryId.HasValue)
            {
                return RedirectToAction("CountryListPage");
            }
            
            var itemsPerPage = _uiOptions.DefaultPageSize_StateList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new StateListPageViewModel();

            var country = await _dataManager.FetchCountry(countryId.Value);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);
            model.States = await _dataManager.GetGeoZonePage(countryId.Value, pageNumber, itemsPerPage);
            model.CountryListReturnPageNumber = crp;

            // below we are just manipiulating the bread crumbs
            var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "StateListPage",
                AdjustedText = string.Format(_sr["{0} States"], model.Country.Name),
                AdjustedUrl = Request.Path.ToString()
                + "?countryId=" + country.Id.ToString()
                + "&crp=" + crp.ToInvariantString(),
                ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
            };
            currentCrumbAdjuster.AddToContext();

            var countryListCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "CountryListPage",
                AdjustedUrl = Request.Path.ToString().Replace("StateListPage", "CountryListPage")
                + "?pageNumber=" + crp.ToInvariantString()
            };
            countryListCrumbAdjuster.AddToContext();
            
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CountryAutoSuggestJson(string query)
        {
            var matches = await _dataManager.CountryAutoComplete(query, 10);
            return Json(matches);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> StateAutoSuggestJson(
           string countryCode,
           string query)
        {
            var country = await _dataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await _dataManager.StateAutoComplete(country.Id, query, 10);
            }
            else
            {
                states = new List<IGeoZone>(); //empty list
            }

            var selecteList = new SelectList(states, "Code", "Name");

            return Json(selecteList);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatesJson(
           string countryCode)
        {
            var country = await _dataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await _dataManager.GetGeoZonesByCountry(country.Id);
            }
            else
            {
                states = new List<IGeoZone>(); //empty list
            }

            var selecteList = new SelectList(states, "Code", "Name");

            return Json(selecteList);

        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpGet]
        public async Task<IActionResult> StateEdit(
            Guid countryId,
            Guid? stateId,
            int crp = 1,
            int returnPageNumber = 1
            )
        {
            if (countryId == Guid.Empty)
            {
                return RedirectToAction("CountryListPage");
            }
            
            GeoZoneViewModel model;

            if ((stateId.HasValue) && (stateId.Value != Guid.Empty))
            {
                var state = await _dataManager.FetchGeoZone(stateId.Value);
                if ((state != null) && (state.CountryId == countryId))
                {
                    model = GeoZoneViewModel.FromIGeoZone(state);

                    var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
                    {
                        KeyToAdjust = "StateEdit",
                        AdjustedText = model.Name,
                        ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
                    };
                    currentCrumbAdjuster.AddToContext();
                }
                else
                {
                    // invalid guid provided
                    return RedirectToAction("CountryListPage", new { pageNumber = crp });
                }

            }
            else
            {
                model = new GeoZoneViewModel
                {
                    CountryId = countryId
                };
            }

            model.ReturnPageNumber = returnPageNumber;
            model.CountryListReturnPageNumber = crp;

            var country = await _dataManager.FetchCountry(countryId);
            model.CountryName = country.Name;

            var stateListCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "StateListPage",
                AdjustedText = string.Format(_sr["{0} States"], model.CountryName),
                ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
            };
            stateListCrumbAdjuster.AddToContext();
            
            return View(model);

        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StateEdit(GeoZoneViewModel model)
        {  
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            if (model.Id == Guid.Empty)
            {
                successFormat = _sr["The state {0} was successfully created."];
                await _dataManager.Add(model);
            }
            else
            {
                successFormat = _sr["The state {0} was successfully updated."];
                await _dataManager.Update(model);
            }
            
            this.AlertSuccess(string.Format(successFormat, model.Name), true);
            
            return RedirectToAction("StateListPage",
                new
                {
                    countryId = model.CountryId,
                    crp = model.CountryListReturnPageNumber,
                    pageNumber = model.ReturnPageNumber
                });

        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StateDelete(
            Guid countryId,
            Guid stateId,
            int crp = 1,
            int returnPageNumber = 1)
        {
            var state = await _dataManager.FetchGeoZone(stateId);
            
            if (state != null)
            {
                await _dataManager.DeleteGeoZone(state);
                
                this.AlertWarning(string.Format(
                        _sr["The state {0} was successfully deleted."],
                        state.Name)
                        , true);
            }

            return RedirectToAction("StateListPage", new { countryId, crp, pageNumber = returnPageNumber });
        }

    }
}
