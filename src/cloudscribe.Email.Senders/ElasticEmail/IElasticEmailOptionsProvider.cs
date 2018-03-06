using System.Threading.Tasks;

namespace cloudscribe.Email.ElasticEmail
{
    public interface IElasticEmailOptionsProvider
    {
        Task<ElasticEmailOptions> GetElasticEmailOptions(string lookupKey = null);
    }
}
