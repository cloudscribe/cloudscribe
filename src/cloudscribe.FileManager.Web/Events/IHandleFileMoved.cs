using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Events
{
    /// <summary>
    /// Implement and inject to respond to a file moved event.
    /// You can inject as many of these as you like.
    /// </summary>
    public interface IHandleFileMoved
    {
        Task Handle(string siteId, string oldUrl, string newUrl);
    }
}
