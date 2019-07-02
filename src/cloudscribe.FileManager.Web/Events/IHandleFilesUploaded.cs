using cloudscribe.FileManager.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Events
{
    /// <summary>
    /// implement and inject to be able to learn of urls for new file uploads
    /// you can inject as many of these as you like.
    /// </summary>
    public interface IHandleFilesUploaded
    {
        Task Handle(IEnumerable<UploadResult> uploadInfoList);
    }
}
