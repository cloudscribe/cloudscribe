// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2016-06-03
// 

using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.ViewModels.CoreData;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Policy = "CoreDataPolicy")]
    public class CoreDataController : Controller
    {
        public CoreDataController(
            SiteSettings currentSite,
            GeoDataManager geoDataManager,
            IStringLocalizer<CloudscribeCore> localizer,
            IOptions<UIOptions> uiOptionsAccessor
            )
        {
            Site = currentSite; 
            dataManager = geoDataManager;
            uiOptions = uiOptionsAccessor.Value;
            sr = localizer;
        }

        private ISiteSettings Site;
        private GeoDataManager dataManager;
        private UIOptions uiOptions;
        private IStringLocalizer sr;


        // GET: /CoreData/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> CountryListPage(
            int pageNumber = 1,
            int pageSize = -1)
        {
            var itemsPerPage = uiOptions.DefaultPageSize_CountryList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new CountryListPageViewModel();
            model.Countries = await dataManager.GetCountriesPage(pageNumber, itemsPerPage);
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await dataManager.GetCountryCount();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CountryEdit(
            Guid? countryId,
            int returnPageNumber = 1
            )
        {
            GeoCountryViewModel model;
            if ((countryId != null) && (countryId.Value != Guid.Empty))
            {
                ViewData["Title"] = sr["Edit Country"];
                var country = await dataManager.FetchCountry(countryId.Value);
                model = GeoCountryViewModel.FromIGeoCountry(country);

                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
                currentCrumbAdjuster.KeyToAdjust = "CountryEdit";
                currentCrumbAdjuster.AdjustedText = sr["Edit Country"];
                currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
                currentCrumbAdjuster.AddToContext();
            }
            else
            {
                ViewData["Title"] = sr["New Country"];
                model = new GeoCountryViewModel();
            }

            model.ReturnPageNumber = returnPageNumber;
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryEdit(
            GeoCountryViewModel model,
            int returnPageNumber = 1)
        {
            ViewData["Title"] = sr["Edit Country"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
          
            string successFormat;
            if (model.Id == Guid.Empty)
            {
                successFormat = sr["The country {0} was successfully created."];
                await dataManager.Add(model);
            }
            else
            {
                successFormat = sr["The country {0} was successfully updated."];
                await dataManager.Update(model);
            }
            
            this.AlertSuccess(string.Format(successFormat,
                        model.Name), true);
            
            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber });

        }


        // probably should hide by config by default
        // seems like an unusual event to delete a country and its states
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryDelete(Guid countryId, int returnPageNumber = 1)
        {
            var country = await dataManager.FetchCountry(countryId);
            
            if (country != null)
            {
                await dataManager.DeleteCountry(country);
                
                this.AlertWarning(string.Format(
                        sr["The country {0} was successfully deleted."],
                        country.Name)
                        , true);     
            }

            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber });
        }

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
            
            var itemsPerPage = uiOptions.DefaultPageSize_StateList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new StateListPageViewModel();

            var country = await dataManager.FetchCountry(countryId.Value);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);
            model.States = await dataManager.GetGeoZonePage(countryId.Value, pageNumber, itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await dataManager.GetGeoZoneCount(countryId.Value);
            model.CountryListReturnPageNumber = crp;

            // below we are just manipiulating the bread crumbs
            var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
            currentCrumbAdjuster.KeyToAdjust = "StateListPage";
            currentCrumbAdjuster.AdjustedText = string.Format(sr["{0} States"], model.Country.Name);
            currentCrumbAdjuster.AdjustedUrl = Request.Path.ToString()
                + "?countryId=" + country.Id.ToString()
                + "&crp=" + crp.ToInvariantString();
            currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
            currentCrumbAdjuster.AddToContext();

            var countryListCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
            countryListCrumbAdjuster.KeyToAdjust = "CountryListPage";
            countryListCrumbAdjuster.AdjustedUrl = Request.Path.ToString().Replace("StateListPage", "CountryListPage")
                + "?pageNumber=" + crp.ToInvariantString(); 
            countryListCrumbAdjuster.AddToContext();
            
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CountryAutoSuggestJson(string query)
        {
            var matches = await dataManager.CountryAutoComplete(query, 10);
            return Json(matches);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> StateAutoSuggestJson(
           string countryCode,
           string query)
        {
            var country = await dataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await dataManager.StateAutoComplete(country.Id, query, 10);
            }
            else
            {
                states = new List<IGeoZone>(); //empty list
            }
            
            return Json(states);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatesJson(
           string countryCode)
        {
            var country = await dataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await dataManager.GetGeoZonesByCountry(country.Id);
            }
            else
            {
                states = new List<IGeoZone>(); //empty list
            }

            var selecteList = new SelectList(states, "Code", "Name");

            return Json(selecteList);

        }

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
                var state = await dataManager.FetchGeoZone(stateId.Value);
                if ((state != null) && (state.CountryId == countryId))
                {
                    model = GeoZoneViewModel.FromIGeoZone(state);
                }
                else
                {
                    // invalid guid provided
                    return RedirectToAction("CountryListPage", new { pageNumber = crp });
                }

            }
            else
            {
                model = new GeoZoneViewModel();
                model.CountryId = countryId;
            }

            model.ReturnPageNumber = returnPageNumber;
            model.CountryListReturnPageNumber = crp;

            var country = await dataManager.FetchCountry(countryId);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);

            var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
            currentCrumbAdjuster.KeyToAdjust = "StateEdit";
            currentCrumbAdjuster.AdjustedText = model.Heading;
            currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
            currentCrumbAdjuster.AddToContext();
            
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StateEdit(GeoZoneViewModel model)
        {  
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            string successFormat;
            if (model.Id == Guid.Empty)
            {
                successFormat = sr["The state {0} was successfully created."];
                await dataManager.Add(model);
            }
            else
            {
                successFormat = sr["The state {0} was successfully updated."];
                await dataManager.Update(model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StateDelete(
            Guid countryId,
            Guid stateId,
            int crp = 1,
            int returnPageNumber = 1)
        {
            var state = await dataManager.FetchGeoZone(stateId);
            
            if (state != null)
            {
                await dataManager.DeleteGeoZone(state);
                
                this.AlertWarning(string.Format(
                        sr["The state {0} was successfully deleted."],
                        state.Name)
                        , true);
            }

            return RedirectToAction("StateListPage",
                new
                {
                    countryId = countryId,
                    crp = crp,
                    pageNumber = returnPageNumber
                });
        }


        [HttpGet]
        public async Task<IActionResult> CurrencyList()
        {
            var model = await dataManager.GetAllCurrencies();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CurrencyEdit(Guid? currencyId)
        {
            var model = new CurrencyViewModel();

            if (currencyId.HasValue)
            {
                var currency = await dataManager.FetchCurrency(currencyId.Value);
                model.CurrencyId = currency.Id;
                model.Title = currency.Title;
                model.Code = currency.Code;
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CurrencyEdit(CurrencyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            ICurrency currency = null;
            bool add = false;
            if (model.CurrencyId != Guid.Empty)
            {
                currency = await dataManager.FetchCurrency(model.CurrencyId);
                successFormat = sr["The currency {0} was successfully updated."];
            }
            else
            {
                add = true;
                currency = new Currency();
                successFormat = sr["The currency {0} was successfully created."];
            }

            currency.Code = model.Code;
            currency.Title = model.Title;

            if(add)
            {
                await dataManager.Add(currency);
            }
            else
            {
                await dataManager.Update(currency);
            }
            
            this.AlertSuccess(string.Format(successFormat, currency.Title), true);
            
            return RedirectToAction("CurrencyList");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CurrencyDelete(Guid currencyId)
        {
            var currency = await dataManager.FetchCurrency(currencyId);
            
            if (currency != null)
            {
                await dataManager.DeleteCurrency(currency);
                
                this.AlertWarning(string.Format(
                        sr["The currency {0} was successfully deleted."],
                        currency.Title)
                        , true);
            }

            return RedirectToAction("CurrencyList");
        }
    }
}
