using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Resources;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteBasicSettingsViewModel
    {
        private int siteID = -1;

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private Guid siteGuid = Guid.Empty;

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }

        }

        private string siteName = string.Empty;

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        private string siteFolderName = string.Empty;

        public string SiteFolderName
        {
            get { return siteFolderName; }
            set { siteFolderName = value; }
        }

        private string timeZoneId = "Eastern Standard Time";

        public string TimeZoneId
        {
            get { return timeZoneId; }
            set { timeZoneId = value; }
        }

        private string slogan = string.Empty;

        public string Slogan
        {
            get { return slogan; }
            set { slogan = value; }
        }

        private string companyName = string.Empty;

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string companyStreetAddress = string.Empty;

        public string CompanyStreetAddress
        {
            get { return companyStreetAddress; }
            set { companyStreetAddress = value; }
        }

        private string companyStreetAddress2 = string.Empty;

        public string CompanyStreetAddress2
        {
            get { return companyStreetAddress2; }
            set { companyStreetAddress2 = value; }
        }

        private string companyLocality = string.Empty;

        public string CompanyLocality
        {
            get { return companyLocality; }
            set { companyLocality = value; }
        }

        private string companyRegion = string.Empty;

        public string CompanyRegion
        {
            get { return companyRegion; }
            set { companyRegion = value; }
        }

        private string companyPostalCode = string.Empty;

        public string CompanyPostalCode
        {
            get { return companyPostalCode; }
            set { companyPostalCode = value; }
        }

        private string companyCountry = string.Empty;

        public string CompanyCountry
        {
            get { return companyCountry; }
            set { companyCountry = value; }
        }

        private string companyPhone = string.Empty;

        public string CompanyPhone
        {
            get { return companyPhone; }
            set { companyPhone = value; }
        }

        private string companyFax = string.Empty;

        public string CompanyFax
        {
            get { return companyFax; }
            set { companyFax = value; }
        }

        private string companyPublicEmail = string.Empty;

        public string CompanyPublicEmail
        {
            get { return companyPublicEmail; }
            set { companyPublicEmail = value; }
        }

    }
}
