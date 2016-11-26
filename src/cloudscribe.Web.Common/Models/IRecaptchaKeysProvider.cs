using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Models
{
    public interface IRecaptchaKeysProvider
    {
        Task<RecaptchaKeys> GetKeys();
    }
}
