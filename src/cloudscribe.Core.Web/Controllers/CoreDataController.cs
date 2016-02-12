// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2016-02-12
// 

using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.ViewModels.CoreData;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Policy = "CoreDataPolicy")]
    public class CoreDataController : CloudscribeBaseController
    {
        public CoreDataController(
            SiteSettings currentSite,
            GeoDataManager geoDataManager,
            IOptions<UIOptions> uiOptionsAccessor
            )
        {
            Site = currentSite; 
            dataManager = geoDataManager;
            uiOptions = uiOptionsAccessor.Value;
        }

        private ISiteSettings Site;
        private GeoDataManager dataManager;
        private UIOptions uiOptions;

        
        // GET: /CoreData/
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "Core Data Administration";
            ViewBag.Heading = "Core Data Administration";
 
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> CountryListPage(
            int pageNumber = 1,
            int pageSize = -1)
        {
            ViewBag.Title = "Country List Administration";
            int itemsPerPage = uiOptions.DefaultPageSize_CountryList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            CountryListPageViewModel model = new CountryListPageViewModel();
            model.Countries = await dataManager.GetCountriesPage(pageNumber, itemsPerPage);
            model.Heading = "Country List Administration";
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await dataManager.GetCountryCount();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CountryEdit(
            Guid? guid,
            int returnPageNumber = 1,
            bool partial = false)
        {
            ViewBag.Title = "Edit Country";

            GeoCountryViewModel model;

            if ((guid != null) && (guid.Value != Guid.Empty))
            {
                IGeoCountry country = await dataManager.FetchCountry(guid.Value);
                model = GeoCountryViewModel.FromIGeoCountry(country);

                NavigationNodeAdjuster currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
                currentCrumbAdjuster.KeyToAdjust = "CountryEdit";
                currentCrumbAdjuster.AdjustedText = "Edit Country";
                currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
                currentCrumbAdjuster.AddToContext();

               
            }
            else
            {
                ViewBag.Title = "New Country";
                model = new GeoCountryViewModel();
            }

            model.ReturnPageNumber = returnPageNumber;


            if (partial)
            {
                return PartialView("CountryEditPartial", model);
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryEdit(
            GeoCountryViewModel model,
            int returnPageNumber = 1)
        {
            ViewBag.Title = "Edit Country";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool result;
            string successFormat;
            if (model.Guid == Guid.Empty)
            {
                successFormat = "The country <b>{0}</b> was successfully created.";
                result = await dataManager.Add(model);
            }
            else
            {
                successFormat = "The country <b>{0}</b> was successfully updated.";
                result = await dataManager.Update(model);
            }
            
            if (result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            model.Name), true);
            }

            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber });

        }


        // probably should hide by config by default
        // seems like an unusual event to delete a country and its states
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryDelete(Guid countryGuid, int returnPageNumber = 1)
        {
            IGeoCountry country = await dataManager.FetchCountry(countryGuid);
            
            if (country != null)
            {
                bool result = await dataManager.DeleteCountry(country);
                if(result)
                {
                    this.AlertWarning(string.Format(
                            "The country <b>{0}</b> was successfully deleted.",
                            country.Name)
                            , true);
                }
                
            }

            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber });
        }

        [HttpGet]
        public async Task<IActionResult> StateListPage(
            Guid? countryGuid,
            int pageNumber = 1,
            int pageSize = -1,
            int crp = 1,
            bool ajaxGrid = false,
            bool partial = false)
        {
            if (!countryGuid.HasValue)
            {
                return RedirectToAction("CountryListPage");
            }

            ViewBag.Title = "State List Administration";
            int itemsPerPage = uiOptions.DefaultPageSize_StateList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            StateListPageViewModel model = new StateListPageViewModel();

            IGeoCountry country = await dataManager.FetchCountry(countryGuid.Value);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);
            model.States = await dataManager.GetGeoZonePage(countryGuid.Value, pageNumber, itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await dataManager.GetGeoZoneCount(countryGuid.Value);
            model.CountryListReturnPageNumber = crp;

            // below we are just manipiulating the bread crumbs
            NavigationNodeAdjuster currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
            currentCrumbAdjuster.KeyToAdjust = "StateListPage";
            currentCrumbAdjuster.AdjustedText = model.Country.Name + " States";
            currentCrumbAdjuster.AdjustedUrl = Request.Path.ToString()
                + "?countryGuid=" + country.Guid.ToString()
                + "&crp=" + crp.ToInvariantString();
            currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
            currentCrumbAdjuster.AddToContext();

            NavigationNodeAdjuster countryListCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
            countryListCrumbAdjuster.KeyToAdjust = "CountryListPage";
            countryListCrumbAdjuster.AdjustedUrl = Request.Path.ToString().Replace("StateListPage", "CountryListPage")
                + "?pageNumber=" + crp.ToInvariantString(); 
            countryListCrumbAdjuster.AddToContext();
            
            if (ajaxGrid)
            {
                return PartialView("StateListGridPartial", model);
            }

            if (partial)
            {
                return PartialView("StateListPagePartial", model);
            }


            return View(model);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CountryAutoSuggestJson(
           string query)
        {

            List<IGeoCountry> matches = await dataManager.CountryAutoComplete(
                query,
                10);


            return Json(matches);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> StateAutoSuggestJson(
           string countryCode,
           string query)
        {
            IGeoCountry country = await dataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await dataManager.StateAutoComplete(country.Guid, query, 10);
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
            IGeoCountry country = await dataManager.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await dataManager.GetGeoZonesByCountry(country.Guid);
            }
            else
            {
                states = new List<IGeoZone>(); //empty list
            }

            SelectList selecteList = new SelectList(states, "Code", "Name");

            return Json(selecteList);

        }

        [HttpGet]
        public async Task<IActionResult> StateEdit(
            Guid countryGuid,
            Guid? guid,
            int crp = 1,
            int returnPageNumber = 1
            )
        {
            if (countryGuid == Guid.Empty)
            {
                return RedirectToAction("CountryListPage");
            }

            //int returnPage = 1;
            //if (returnPageNumber.HasValue) { returnPage = returnPageNumber.Value; }

            ViewBag.Title = "Edit State";

            GeoZoneViewModel model;

            if ((guid.HasValue) && (guid.Value != Guid.Empty))
            {
                IGeoZone state = await dataManager.FetchGeoZone(guid.Value);
                if ((state != null) && (state.CountryGuid == countryGuid))
                {
                    model = GeoZoneViewModel.FromIGeoZone(state);
                    model.Heading = "Edit State";

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
                model.Heading = "Create New State";
                model.CountryGuid = countryGuid;
            }

            model.ReturnPageNumber = returnPageNumber;
            model.CountryListReturnPageNumber = crp;

            IGeoCountry country = await dataManager.FetchCountry(countryGuid);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);

            NavigationNodeAdjuster currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
            currentCrumbAdjuster.KeyToAdjust = "StateEdit";
            currentCrumbAdjuster.AdjustedText = model.Heading;
            currentCrumbAdjuster.ViewFilterName = NamedNavigationFilters.Breadcrumbs; // this is default but showing here for readers of code 
            currentCrumbAdjuster.AddToContext();

            //var node = SiteMaps.Current.FindSiteMapNodeFromKey("StateEdit");
            //if (node != null)
            //{
            //    node.Title = model.Heading;
            //    var parent = node.ParentNode;
            //    if (parent != null)
            //    {
            //        parent.Title = model.Country.Name + " States";

            //    }
            //}

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StateEdit(
            GeoZoneViewModel model)
        {
            ViewBag.Title = "Edit State";

            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            bool result;
            string successFormat;
            if (model.Guid == Guid.Empty)
            {
                successFormat = "The state <b>{0}</b> was successfully created.";
                result = await dataManager.Add(model);
            }
            else
            {
                successFormat = "The state <b>{0}</b> was successfully updated.";
                result = await dataManager.Update(model);
            }

            
            if (result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            model.Name), true);
            }

            //IGeoCountry country = await geoRepo.FetchCountry(model.CountryGuid);


            //IGeoZone state = model;
            //model = GeoZoneViewModel.FromIGeoZone(state);
            //model.Country = GeoCountryViewModel.FromIGeoCountry(country);

            //model.Heading = "Edit State";

            //return PartialView(model);


            return RedirectToAction("StateListPage",
                new
                {
                    countryGuid = model.CountryGuid,
                    crp = model.CountryListReturnPageNumber,
                    pageNumber = model.ReturnPageNumber
                });


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StateDelete(
            Guid countryGuid,
            Guid guid,
            int crp = 1,
            int returnPageNumber = 1)
        {
            IGeoZone state = await dataManager.FetchGeoZone(guid);
            
            if (state != null)
            {
                bool result = await dataManager.DeleteGeoZone(state);
                if(result)
                {
                    this.AlertWarning(string.Format(
                            "The state <b>{0}</b> was successfully deleted.",
                            state.Name)
                            , true);
                }
                
            }

            return RedirectToAction("StateListPage",
                new
                {
                    countryGuid = countryGuid,
                    crp = crp,
                    pageNumber = returnPageNumber
                });
        }


        [HttpGet]
        public async Task<IActionResult> CurrencyList()
        {
            ViewBag.Title = "Currency Administration";
            ViewBag.Heading = "Currency Administration";

            List<ICurrency> model = await dataManager.GetAllCurrencies();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CurrencyEdit(Guid? currencyGuid)
        {
            ViewBag.Title = "Edit Currency";
            ViewBag.Heading = "Edit Currency";

            CurrencyViewModel model = new CurrencyViewModel();

            if (currencyGuid.HasValue)
            {
                ICurrency currency = await dataManager.FetchCurrency(currencyGuid.Value);
                model.Guid = currency.Guid;
                model.Title = currency.Title;
                model.Code = currency.Code;

                //var node = SiteMaps.Current.FindSiteMapNodeFromKey("CurrencyEdit");
                //if (node != null)
                //{
                //    node.Title = "Edit Currency";
                //}
            }


            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CurrencyEdit(CurrencyViewModel model)
        {
            ViewBag.Title = "Edit Currency";
            ViewBag.Heading = "Edit Currency";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result;
            string successFormat;
            ICurrency currency = null;
            bool add = false;
            if (model.Guid != Guid.Empty)
            {
                currency = await dataManager.FetchCurrency(model.Guid);
                successFormat = "The currency <b>{0}</b> was successfully updated.";
            }
            else
            {
                add = true;
                currency = new Currency();
                successFormat = "The currency <b>{0}</b> was successfully created.";
            }

            currency.Code = model.Code;
            currency.Title = model.Title;

            if(add)
            {
                result = await dataManager.Add(currency);
            }
            else
            {
                result = await dataManager.Update(currency);
            }

            
            if (result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            currency.Title), true);
            }


            return RedirectToAction("CurrencyList");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CurrencyDelete(Guid currencyGuid)
        {
            ICurrency currency = await dataManager.FetchCurrency(currencyGuid);
            
            if (currency != null)
            {
                bool result = await dataManager.DeleteCurrency(currency);
                if(result)
                {
                    this.AlertWarning(string.Format(
                            "The currency <b>{0}</b> was successfully deleted.",
                            currency.Title)
                            , true);
                }
                
            }

            return RedirectToAction("CurrencyList");
        }
    }
}
