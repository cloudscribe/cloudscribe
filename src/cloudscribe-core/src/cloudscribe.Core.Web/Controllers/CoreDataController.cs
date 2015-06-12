// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2015-06-11
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.ViewModels.CoreData;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
//using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;




namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "ServerAdmins")]
    public class CoreDataController : CloudscribeBaseController
    {
        public CoreDataController(
            ISiteContext siteContext,
            IGeoRepository geoRepository
            )
        {
            Site = siteContext;
            geoRepo = geoRepository;
        }

        private ISiteContext Site;
        private IGeoRepository geoRepo;

        //disable warning about not really being async
        // we know it is not, it is not needed to hit the db in these
#pragma warning disable 1998

        // GET: /CoreData/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Core Data Administration";
            ViewBag.Heading = "Core Data Administration";

            return View();


        }

#pragma warning restore 1998

        [HttpGet]
        public async Task<IActionResult> CountryListPage(
            int pageNumber = 1,
            int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Country List Administration";
            int itemsPerPage = AppSettings.DefaultPageSize_CountryList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            CountryListPageViewModel model = new CountryListPageViewModel();
            model.Countries = await geoRepo.GetCountriesPage(pageNumber, itemsPerPage);
            model.Heading = "Country List Administration";
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await geoRepo.GetCountryCount();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CountryEdit(
            Guid? guid,
            int returnPageNumber = 1,
            bool partial = false)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            GeoCountryViewModel model;

            if ((guid != null) && (guid.Value != Guid.Empty))
            {
                IGeoCountry country = await geoRepo.FetchCountry(guid.Value);
                model = GeoCountryViewModel.FromIGeoCountry(country);

                //var node = SiteMaps.Current.FindSiteMapNodeFromKey("CountryEdit");
                //if (node != null)
                //{
                //    node.Title = "Edit Country";
                //}
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
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            if (model.Guid == Guid.Empty)
            {
                successFormat = "The country <b>{0}</b> was successfully created.";
            }
            else
            {
                successFormat = "The country <b>{0}</b> was successfully updated.";
            }

            bool result = await geoRepo.Save(model);

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
            IGeoCountry country = await geoRepo.FetchCountry(countryGuid);
            bool result = await geoRepo.DeleteGeoZonesByCountry(countryGuid);
            result = await geoRepo.DeleteCountry(countryGuid);

            if (result && (country != null))
            {
                this.AlertWarning(string.Format(
                            "The country <b>{0}</b> was successfully deleted.",
                            country.Name)
                            , true);
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

            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "State List Administration";
            int itemsPerPage = AppSettings.DefaultPageSize_StateList;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            StateListPageViewModel model = new StateListPageViewModel();

            IGeoCountry country = await geoRepo.FetchCountry(countryGuid.Value);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);
            model.States = await geoRepo.GetGeoZonePage(countryGuid.Value, pageNumber, itemsPerPage);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = await geoRepo.GetGeoZoneCount(countryGuid.Value);
            model.CountryListReturnPageNumber = crp;

            // below we are just manipiulating the bread crumbs
            //var node = SiteMaps.Current.FindSiteMapNodeFromKey("StateListPage");
            //if (node != null)
            //{
            //    node.Title = model.Country.Name + " States";

            //}

            // too bad this does not work
            //node = SiteMaps.Current.FindSiteMapNodeFromKey("CountryListPage");
            //if (node != null)
            //{
            //    node.RouteValues.Add("pageNumber", countryReturnPageNumber);

            //}



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
        public async Task<IActionResult> CountryAutoSuggestJson(
           string query)
        {

            List<IGeoCountry> matches = await geoRepo.CountryAutoComplete(
                query,
                10);


            return Json(matches);

        }

        [HttpGet]
        public async Task<IActionResult> StateAutoSuggestJson(
           string countryCode,
           string query)
        {
            IGeoCountry country = await geoRepo.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await geoRepo.StateAutoComplete(country.Guid, query, 10);
            }
            else
            {
                states = new List<IGeoZone>(); //empty list
            }



            return Json(states);

        }


        [HttpGet]
        public async Task<IActionResult> GetStatesJson(
           string countryCode)
        {
            IGeoCountry country = await geoRepo.FetchCountry(countryCode);
            List<IGeoZone> states;
            if (country != null)
            {
                states = await geoRepo.GetGeoZonesByCountry(country.Guid);
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

            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit State";

            GeoZoneViewModel model;

            if ((guid.HasValue) && (guid.Value != Guid.Empty))
            {
                IGeoZone state = await geoRepo.FetchGeoZone(guid.Value);
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

            IGeoCountry country = await geoRepo.FetchCountry(countryGuid);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);

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
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit State";

            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            string successFormat;
            if (model.Guid == Guid.Empty)
            {
                successFormat = "The state <b>{0}</b> was successfully created.";
            }
            else
            {
                successFormat = "The state <b>{0}</b> was successfully updated.";
            }

            bool result = await geoRepo.Save(model);
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
            IGeoZone state = await geoRepo.FetchGeoZone(guid);
            bool result = await geoRepo.DeleteGeoZone(guid);

            if (result && (state != null))
            {
                this.AlertWarning(string.Format(
                            "The state <b>{0}</b> was successfully deleted.",
                            state.Name)
                            , true);
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
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Currency Administration";
            ViewBag.Heading = "Currency Administration";

            List<ICurrency> model = await geoRepo.GetAllCurrencies();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CurrencyEdit(Guid? currencyGuid)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Currency";
            ViewBag.Heading = "Edit Currency";

            CurrencyViewModel model = new CurrencyViewModel();

            if (currencyGuid.HasValue)
            {
                ICurrency currency = await geoRepo.FetchCurrency(currencyGuid.Value);
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
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Currency";
            ViewBag.Heading = "Edit Currency";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            ICurrency currency = null;
            if (model.Guid != Guid.Empty)
            {
                currency = await geoRepo.FetchCurrency(model.Guid);
                successFormat = "The currency <b>{0}</b> was successfully updated.";
            }
            else
            {
                currency = new Currency();
                successFormat = "The currency <b>{0}</b> was successfully created.";
            }

            currency.Code = model.Code;
            currency.Title = model.Title;

            bool result = await geoRepo.Save(currency);
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
            ICurrency currency = await geoRepo.FetchCurrency(currencyGuid);
            bool result = await geoRepo.DeleteCurrency(currencyGuid);
            if (result && (currency != null))
            {
                this.AlertWarning(string.Format(
                            "The currency <b>{0}</b> was successfully deleted.",
                            currency.Title)
                            , true);
            }

            return RedirectToAction("CurrencyList");
        }
    }
}
