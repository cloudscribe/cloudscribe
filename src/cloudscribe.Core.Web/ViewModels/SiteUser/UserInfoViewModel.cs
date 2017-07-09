using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class UserInfoViewModel
    {
        [StringLength(100, ErrorMessage = "First Name has a maximum length of 100 characters")]
        public string FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Last Name has a maximum length of 100 characters")]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Url(ErrorMessage ="You must provide a valid URL")]
        [StringLength(300, ErrorMessage = "Website Url has a maximum length of 300 characters")]
        public string WebSiteUrl { get; set; }
    }
}
