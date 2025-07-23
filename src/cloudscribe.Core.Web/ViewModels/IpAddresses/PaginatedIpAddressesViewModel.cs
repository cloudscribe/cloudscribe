using cloudscribe.Pagination.Models;

namespace cloudscribe.Core.Web.ViewModels.IpAddresses
{
    public class PaginatedIpAddressesViewModel
    {
        public PaginatedIpAddressesViewModel()
        {
            BlockedPermittedIpAddresses = new PagedResult<IpAddressesViewModel>();

        }

        public PagedResult<IpAddressesViewModel> BlockedPermittedIpAddresses { get; set; }
        public string SearchTerm { get; set; }
    }
}