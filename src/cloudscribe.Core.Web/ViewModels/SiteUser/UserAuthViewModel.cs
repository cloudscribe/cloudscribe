using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class UserAuthViewModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public Boolean IsAuthenticated { get; set; }    
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
