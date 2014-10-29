// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2014-10-26
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Resources;
using cloudscribe.Configuration.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteBasicSettingsViewModel
    {
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

        //[Required]
        [StringLengthAppSettings(MinimumLength = 40, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength",  ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Display(Name = "SiteName", ResourceType = typeof(CommonResources))]
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        private string siteFolderName = string.Empty;

        [Display(Name = "SiteFolderName", ResourceType = typeof(CommonResources))]
        public string SiteFolderName
        {
            get { return siteFolderName; }
            set { siteFolderName = value; }
        }

        private string timeZoneId = "Eastern Standard Time";

        [Required]
        [Display(Name = "TimeZone", ResourceType = typeof(CommonResources))]
        public string TimeZoneId
        {
            get { return timeZoneId; }
            set { timeZoneId = value; }
        }

        private string slogan = string.Empty;

        [StringLength(255, MinimumLength = 3, ErrorMessage = "Slogan Length Error")]
        [Display(Name = "SiteSlogan", ResourceType = typeof(CommonResources))]
        public string Slogan
        {
            get { return slogan; }
            set { slogan = value; }
        }

        private string companyName = string.Empty;

        [Display(Name = "CompanyName", ResourceType = typeof(CommonResources))]
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string companyStreetAddress = string.Empty;

        [Display(Name = "CompanyAddress1", ResourceType = typeof(CommonResources))]
        public string CompanyStreetAddress
        {
            get { return companyStreetAddress; }
            set { companyStreetAddress = value; }
        }

        private string companyStreetAddress2 = string.Empty;

        [Display(Name = "CompanyAddress2", ResourceType = typeof(CommonResources))]
        public string CompanyStreetAddress2
        {
            get { return companyStreetAddress2; }
            set { companyStreetAddress2 = value; }
        }

        private string companyLocality = string.Empty;

        [Display(Name = "CompanyLocality", ResourceType = typeof(CommonResources))]
        public string CompanyLocality
        {
            get { return companyLocality; }
            set { companyLocality = value; }
        }

        private string companyRegion = string.Empty;

        [Display(Name = "CompanyRegion", ResourceType = typeof(CommonResources))]
        public string CompanyRegion
        {
            get { return companyRegion; }
            set { companyRegion = value; }
        }

        private string companyPostalCode = string.Empty;

        [Display(Name = "CompanyPostalCode", ResourceType = typeof(CommonResources))]
        public string CompanyPostalCode
        {
            get { return companyPostalCode; }
            set { companyPostalCode = value; }
        }

        private string companyCountry = string.Empty;

        [Display(Name = "CompanyCountry", ResourceType = typeof(CommonResources))]
        public string CompanyCountry
        {
            get { return companyCountry; }
            set { companyCountry = value; }
        }

        private string companyPhone = string.Empty;

        [Display(Name = "CompanyPhone", ResourceType = typeof(CommonResources))]
        public string CompanyPhone
        {
            get { return companyPhone; }
            set { companyPhone = value; }
        }

        private string companyFax = string.Empty;

        [Display(Name = "CompanyFax", ResourceType = typeof(CommonResources))]
        public string CompanyFax
        {
            get { return companyFax; }
            set { companyFax = value; }
        }

        private string companyPublicEmail = string.Empty;

        [EmailAddress]
        [Display(Name = "CompanyPublicEmail", ResourceType = typeof(CommonResources))]
        public string CompanyPublicEmail
        {
            get { return companyPublicEmail; }
            set { companyPublicEmail = value; }
        }

    }
}
