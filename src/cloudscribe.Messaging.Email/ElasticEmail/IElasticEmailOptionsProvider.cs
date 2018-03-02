using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email.ElasticEmail
{
    public interface IElasticEmailOptionsProvider
    {
        Task<ElasticEmailOptions> GetElasticEmailOptions(string lookupKey = null);
    }
}
