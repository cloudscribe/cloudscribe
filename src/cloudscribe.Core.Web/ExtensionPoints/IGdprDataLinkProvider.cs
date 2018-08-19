using cloudscribe.Common.Gdpr;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ExtensionPoints
{
    public interface IGdprDataLinkProvider
    {
        Task<GdprDataLink> GetLink(IUrlHelper urlHelper, string userId);
    }
}
