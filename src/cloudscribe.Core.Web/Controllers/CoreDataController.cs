// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2015-01-04
// 

using cloudscribe.Configuration;
using cloudscribe.Core.Models.Geography;
using cloudscribe.Core.Web.ViewModels.CoreData;
using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace cloudscribe.Core.Web.Controllers
{
    [Authorize(Roles = "ServerAdmins")]
    public class CoreDataController : CloudscribeBaseController
    {

        private IGeoRepository geoRepo;
        
        public CoreDataController(IGeoRepository geoRepository)
        {
            geoRepo = geoRepository;
        }

        // GET: /CoreData/
        public async Task<ActionResult> Index()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Core Data Administration";
            ViewBag.Heading = "Core Data Administration";

            return View();

            
        }

        public async Task<ActionResult> CountryListPage(int pageNumber = 1, int pageSize = -1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Country List Administration";
            int itemsPerPage = AppSettings.DefaultPageSize_CountryList;
            if(pageSize > 0)
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

        
        
        public async Task<ActionResult> CountryEdit(Guid? guid, int returnPageNumber = 1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            GeoCountryViewModel model;

            if((guid != null)&&(guid.Value != Guid.Empty))
            {
                IGeoCountry country = await geoRepo.FetchCountry(guid.Value);
                model = GeoCountryViewModel.FromIGeoCountry(country);

                var node = SiteMaps.Current.FindSiteMapNodeFromKey("CountryEdit");
                if (node != null)
                {
                    node.Title = "Edit Country";
                }
            }
            else
            {
                ViewBag.Title = "New Country";
                model = new GeoCountryViewModel();
            }

            

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CountryEdit(GeoCountryViewModel model, int returnPageNumber = 1)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Country";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string successFormat;
            if(model.Guid == Guid.Empty)
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
            
            return RedirectToAction("CountryListPage", new { pageNumber = returnPageNumber} );

        }

        //TODO: there is currently nothing in the UI that posts here
        // probably should implement it but hide by config by default
        // seems like an unusual event to delete a country and its states
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CountryDelete(Guid countryGuid)
        {
            IGeoCountry country = await geoRepo.FetchCountry(countryGuid);
            bool result = await geoRepo.DeleteGeoZonesByCountry(countryGuid);
            result = result && await geoRepo.DeleteCountry(countryGuid);

            if (result && (country != null))
            {
                this.AlertWarning(string.Format(
                            "The country <b>{0}</b> was successfully deleted.",
                            country.Name)
                            , true);
            }

            return RedirectToAction("CountryListPage");
        }

        
        public async Task<ActionResult> StateListPage(
            Guid? countryGuid, 
            int pageNumber = 1, 
            int pageSize = -1,
            int countryReturnPageNumber = 1)
        {
            if(!countryGuid.HasValue)
            {
                return RedirectToAction("CountryListPage");
            }

            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "State List Administration";
            int itemsPerPage = AppSettings.DefaultPageSize_CountryList;
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
            model.CountryListReturnPageNumber = countryReturnPageNumber;

            // below we are just manipiulating the bread crumbs
            var node = SiteMaps.Current.FindSiteMapNodeFromKey("StateListPage");
            if (node != null)
            {
                node.Title = model.Country.Name + " States";
            }
            
            return View(model);

        }

       
        public async Task<ActionResult> StateEdit(Guid countryGuid, Guid? guid, int? returnPageNumber)
        {
            if (countryGuid == Guid.Empty)
            {
                return RedirectToAction("CountryListPage");
            }

            int returnPage = 1;
            if (returnPageNumber.HasValue) { returnPage = returnPageNumber.Value; }

            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit State";

            GeoZoneViewModel model;

            if((guid.HasValue)&&(guid.Value != Guid.Empty))
            {
                IGeoZone state = await geoRepo.FetchGeoZone(guid.Value);
                if((state != null)&&(state.CountryGuid == countryGuid))
                {
                    model = GeoZoneViewModel.FromIGeoZone(state);
                    model.Heading = "Edit State";
                    
                }
                else
                {
                    return RedirectToAction("CountryListPage", new { pageNumber = returnPage });
                }

            }
            else
            {
                model = new GeoZoneViewModel();
                model.Heading = "Create New State";
                model.CountryGuid = countryGuid;
            }

            model.ReturnPageNumber = returnPage;
            
            IGeoCountry country = await geoRepo.FetchCountry(countryGuid);
            model.Country = GeoCountryViewModel.FromIGeoCountry(country);

            var node = SiteMaps.Current.FindSiteMapNodeFromKey("StateEdit");
            if (node != null)
            {
                node.Title = model.Heading;
                var parent = node.ParentNode;
                if(parent != null)
                {
                    parent.Title = model.Country.Name + " States";
                    
                }
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StateEdit(GeoZoneViewModel model)
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
                successFormat = "The state <b>{0}</b> was successfully created.";
            }
            else
            {
                successFormat = "The state <b>{0}</b> was successfully updated.";
            }

            bool result = await geoRepo.Save(model);
            if(result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            model.Name), true);
            }
            

            return RedirectToAction("StateListPage", new { countryGuid = model.CountryGuid, pageNumber = model.ReturnPageNumber });
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StateDelete(Guid countryGuid, Guid guid, int returnPageNumber =1)
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

            return RedirectToAction("StateListPage", new { countryGuid = countryGuid, pageNumber = returnPageNumber });
        }

        public async Task<ActionResult> CurrencyList()
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Currency Administration";
            ViewBag.Heading = "Currency Administration";

            List<ICurrency> model = await geoRepo.GetAllCurrencies();

            return View(model);
        }

        
        public async Task<ActionResult> CurrencyEdit(Guid? currencyGuid)
        {
            ViewBag.SiteName = Site.SiteSettings.SiteName;
            ViewBag.Title = "Edit Currency";
            ViewBag.Heading = "Edit Currency";

            CurrencyViewModel model = new CurrencyViewModel();

            if(currencyGuid.HasValue)
            {
                ICurrency currency = await geoRepo.FetchCurrency(currencyGuid.Value);
                model.Guid = currency.Guid;
                model.Title = currency.Title;
                model.Code = currency.Code;

                var node = SiteMaps.Current.FindSiteMapNodeFromKey("CurrencyEdit");
                if (node != null)
                {
                    node.Title = "Edit Currency";
                }
            }


            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CurrencyEdit(CurrencyViewModel model)
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
            if(model.Guid != Guid.Empty)
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
            if(result)
            {
                this.AlertSuccess(string.Format(successFormat,
                            currency.Title), true);
            }
            

            return RedirectToAction("CurrencyList");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CurrencyDelete(Guid currencyGuid)
        {
            ICurrency currency = await geoRepo.FetchCurrency(currencyGuid);
            bool result = await geoRepo.DeleteCurrency(currencyGuid);
            if(result && (currency != null))
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
