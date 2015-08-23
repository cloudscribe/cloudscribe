// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-08-23
// 

//using cloudscribe.Configuration.DataAnnotations;
//using cloudscribe.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc.Rendering;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteBasicSettingsViewModel
    {
        public SiteBasicSettingsViewModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        private int siteID = -1;

        [Display(Name = "SiteId")]
        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private Guid siteGuid = Guid.Empty;

        [Display(Name = "SiteGuid")]
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }

        }

        private string siteName = string.Empty;

        //[Required(ErrorMessageResourceName = "SiteNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "SiteName", ResourceType = typeof(CommonResources))]
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        private string siteFolderName = string.Empty;

        //[Display(Name = "SiteFolderName", ResourceType = typeof(CommonResources))]
        public string SiteFolderName
        {
            get { return siteFolderName; }
            set { siteFolderName = value; }
        }

        private string hostName = string.Empty;
        //[Display(Name = "PreferredHostName", ResourceType = typeof(CommonResources))]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private string timeZoneId = "Eastern Standard Time";

        [Required]
        //[Display(Name = "TimeZone", ResourceType = typeof(CommonResources))]
        public string TimeZoneId
        {
            get { return timeZoneId; }
            set { timeZoneId = value; }
        }

        private ReadOnlyCollection<TimeZoneInfo> allTimeZones = null;

        public ReadOnlyCollection<TimeZoneInfo> AllTimeZones
        {
            get { return allTimeZones; }
            set { allTimeZones = value; }
        }

        private string slogan = string.Empty;

        //[RequiredWithConfig(RequiredKey = "SloganRequired", ErrorMessageResourceName = "SiteNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Slogan Length Error")]
        //[Display(Name = "SiteSlogan", ResourceType = typeof(CommonResources))]
        public string Slogan
        {
            get { return slogan; }
            set { slogan = value; }
        }

        private string companyName = string.Empty;

        //[Display(Name = "CompanyName", ResourceType = typeof(CommonResources))]
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string companyStreetAddress = string.Empty;

        //[Display(Name = "CompanyAddress1", ResourceType = typeof(CommonResources))]
        public string CompanyStreetAddress
        {
            get { return companyStreetAddress; }
            set { companyStreetAddress = value; }
        }

        private string companyStreetAddress2 = string.Empty;

        //[Display(Name = "CompanyAddress2", ResourceType = typeof(CommonResources))]
        public string CompanyStreetAddress2
        {
            get { return companyStreetAddress2; }
            set { companyStreetAddress2 = value; }
        }

        private string companyLocality = string.Empty;

        //[Display(Name = "CompanyLocality", ResourceType = typeof(CommonResources))]
        public string CompanyLocality
        {
            get { return companyLocality; }
            set { companyLocality = value; }
        }

        private string companyCountry = string.Empty;

        //[Display(Name = "CompanyCountry", ResourceType = typeof(CommonResources))]
        public string CompanyCountry
        {
            get { return companyCountry; }
            set { companyCountry = value; }
        }

        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        private string companyRegion = string.Empty;

        //[Display(Name = "CompanyRegion", ResourceType = typeof(CommonResources))]
        public string CompanyRegion
        {
            get { return companyRegion; }
            set { companyRegion = value; }
        }

        private string companyPostalCode = string.Empty;

        //[Display(Name = "CompanyPostalCode", ResourceType = typeof(CommonResources))]
        public string CompanyPostalCode
        {
            get { return companyPostalCode; }
            set { companyPostalCode = value; }
        }



        private string companyPhone = string.Empty;

        //[Display(Name = "CompanyPhone", ResourceType = typeof(CommonResources))]
        public string CompanyPhone
        {
            get { return companyPhone; }
            set { companyPhone = value; }
        }

        private string companyFax = string.Empty;

        ///[Display(Name = "CompanyFax", ResourceType = typeof(CommonResources))]
        public string CompanyFax
        {
            get { return companyFax; }
            set { companyFax = value; }
        }

        private string companyPublicEmail = string.Empty;

        [EmailAddress]
        //[Display(Name = "CompanyPublicEmail", ResourceType = typeof(CommonResources))]
        public string CompanyPublicEmail
        {
            get { return companyPublicEmail; }
            set { companyPublicEmail = value; }
        }

        private int returnPageNumber = 1;

        public int ReturnPageNumber
        {
            get { return returnPageNumber; }
            set { returnPageNumber = value; }
        }

        private bool showDelete = false;

        public bool ShowDelete
        {
            get { return showDelete; }
            set { showDelete = value; }
        }


        private string closedMessage = string.Empty;

        //[AllowHtml]
        //[Display(Name = "ClosedMessage", ResourceType = typeof(CommonResources))]
        public string ClosedMessage
        {
            get { return closedMessage; }
            set { closedMessage = value; }
        }

        private bool isClosed = false;

        //[Display(Name = "IsClosed", ResourceType = typeof(CommonResources))]
        public bool IsClosed
        {
            get { return isClosed; }
            set { isClosed = value; }
        }

        private bool requireCaptchaOnLogin = false;

        //[Display(Name = "RequireCaptchaOnLogin", ResourceType = typeof(CommonResources))]
        public bool RequireCaptchaOnLogin
        {
            get { return requireCaptchaOnLogin; }
            set { requireCaptchaOnLogin = value; }
        }

        private bool requireCaptchaOnRegistration = false;

        //[Display(Name = "RequireCaptchaOnRegistration", ResourceType = typeof(CommonResources))]
        public bool RequireCaptchaOnRegistration
        {
            get { return requireCaptchaOnRegistration; }
            set { requireCaptchaOnRegistration = value; }
        }

        private string recaptchaPublicKey = string.Empty;

        //[Display(Name = "RecaptchaPublicKey", ResourceType = typeof(CommonResources))]
        public string RecaptchaPublicKey
        {
            get { return recaptchaPublicKey; }
            set { recaptchaPublicKey = value; }
        }

        private string recaptchaPrivateKey = string.Empty;

        //[Display(Name = "RecaptchaPrivateKey", ResourceType = typeof(CommonResources))]
        public string RecaptchaPrivateKey
        {
            get { return recaptchaPrivateKey; }
            set { recaptchaPrivateKey = value; }
        }

    }
}
