// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2019-02-03
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
            CurrentSite = currentSite; 
            DataManager = geoDataManager;
            UIOptions = uiOptionsAccessor.Value;
            StringLocalizer = localizer;
        }

        protected ISiteContext CurrentSite { get; private set; }
        protected GeoDataManager DataManager { get; private set; }
        protected UIOptions UIOptions { get; private set; }
        protected IStringLocalizer StringLocalizer { get; private set; }

        // GET: /CoreData/
        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpGet]
        public virtual IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "CoreDataPolicy")]
        [HttpGet]
        public virtual async Task<IActionResult> CountryListPage(
            int pageNumber = 1,
            int pageSize = -1)
        {
            var itemsPerPage = UIOptions.DefaultPageSize_CountryList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new CountryListPageViewModel
            {
                Countries = await DataManager.GetCountriesPage(pageNumber, itemsPerPage)
            };

            return View(model);
        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> CountryEdit(
            Guid? countryId,
            int returnPageNumber = 1
            )
        {
            GeoCountryViewModel model;
            if ((countryId != null) && (countryId.Value != Guid.Empty))
            {
                ViewData["Title"] = StringLocalizer["Edit Country"];
                var country = await DataManager.FetchCountry(countryId.Value);
                model = GeoCountryViewModel.FromIGeoCountry(country);

                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
                {
                    KeyToAdjust = "CountryEdit",
                    AdjustedText = StringLocalizer["Edit Country"],
                    ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
                };
                currentCrumbAdjuster.AddToContext();
            }
            else
            {
                ViewData["Title"] = StringLocalizer["New Country"];
                model = new GeoCountryViewModel();
            }

            model.ReturnPageNumber = returnPageNumber;
            
            return View(model);
        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CountryEdit(
            GeoCountryViewModel model,
            int returnPageNumber = 1)
        {
            ViewData["Title"] = StringLocalizer["Edit Country"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
          
            string successFormat;
            if (model.Id == Guid.Empty)
            {
                successFormat = StringLocalizer["The country {0} was successfully created."];
                await DataManager.Add(model);
            }
            else
            {
                successFormat = StringLocalizer["The country {0} was successfully updated."];
                await DataManager.Update(model);
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
        public virtual async Task<IActionResult> CountryDelete(Guid countryId, int returnPageNumber = 1)
        {
            var country = await DataManager.FetchCountry(countryId);
            
            if (country != null)
            {
                await DataManager.DeleteCountry(country);
                
                this.AlertWarning(string.Format(
                        StringLocalizer["The country {0} was successfully deleted."],
                        country.Name)
                        , true);     
            }

            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber });
        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpGet]
        public virtual async Task<IActionResult> StateListPage(
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
            
            var itemsPerPage = UIOptions.DefaultPageSize_StateList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new StateListPageViewModel();

            var country = await DataManager.FetchCountry(countryId.Value);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);
            model.States = await DataManager.GetGeoZonePage(countryId.Value, pageNumber, itemsPerPage);
            model.CountryListReturnPageNumber = crp;

            // below we are just manipiulating the bread crumbs
            var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "StateListPage",
                AdjustedText = string.Format(StringLocalizer["{0} States"], model.Country.Name),
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
        public virtual async Task<IActionResult> CountryAutoSuggestJson(string query)
        {
            var matches = await DataManager.CountryAutoComplete(query, 10);
            return Json(matches);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<IActionResult> StateAutoSuggestJson(
           string countryCode,
           string query)
        {
            var country = await DataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await DataManager.StateAutoComplete(country.Id, query, 10);
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
        public virtual async Task<IActionResult> GetStatesJson(
           string countryCode)
        {
            var country = await DataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await DataManager.GetGeoZonesByCountry(country.Id);
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
        public virtual async Task<IActionResult> StateEdit(
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
                var state = await DataManager.FetchGeoZone(stateId.Value);
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

            var country = await DataManager.FetchCountry(countryId);
            model.CountryName = country.Name;

            var stateListCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext)
            {
                KeyToAdjust = "StateListPage",
                AdjustedText = string.Format(StringLocalizer["{0} States"], model.CountryName),
                ViewFilterName = NamedNavigationFilters.Breadcrumbs // this is default but showing here for readers of code 
            };
            stateListCrumbAdjuster.AddToContext();
            
            return View(model);

        }

        [Authorize(Policy = PolicyConstants.CoreDataPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> StateEdit(GeoZoneViewModel model)
        {  
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            if (model.Id == Guid.Empty)
            {
                successFormat = StringLocalizer["The state {0} was successfully created."];
                await DataManager.Add(model);
            }
            else
            {
                successFormat = StringLocalizer["The state {0} was successfully updated."];
                await DataManager.Update(model);
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
        public virtual async Task<IActionResult> StateDelete(
            Guid countryId,
            Guid stateId,
            int crp = 1,
            int returnPageNumber = 1)
        {
            var state = await DataManager.FetchGeoZone(stateId);
            
            if (state != null)
            {
                await DataManager.DeleteGeoZone(state);
                
                this.AlertWarning(string.Format(
                        StringLocalizer["The state {0} was successfully deleted."],
                        state.Name)
                        , true);
            }

            return RedirectToAction("StateListPage", new { countryId, crp, pageNumber = returnPageNumber });
        }

    }
}
