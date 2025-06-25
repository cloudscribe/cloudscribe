using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;

namespace cloudscribe.Core.Web.ViewModels.IpAddresses
{
    public class PaginatedIpAddressesViewModel
    {
        public PaginatedIpAddressesViewModel()
        {
            BlackWhitelistIpAddresses = new PagedResult<IpAddressesViewModel>();

        }

        public PagedResult<IpAddressesViewModel> BlackWhitelistIpAddresses { get; set; }
        public string SearchTerm { get; set; }
    }
}