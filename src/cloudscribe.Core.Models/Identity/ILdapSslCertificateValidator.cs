using Microsoft.Extensions.Logging;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace cloudscribe.Core.Models.Identity
{
    /// <summary>
    /// implement and inject one of these to validate the ssl certificate of the ldap server
    /// https://stackoverflow.com/questions/7331666/c-sharp-how-can-i-validate-a-root-ca-cert-certificate-x509-chain
    /// </summary>
    public interface ILdapSslCertificateValidator
    {
        bool ValidateCertificate(
           object sender,
           X509Certificate certificate,
           X509Chain chain,
           SslPolicyErrors sslPolicyErrors);
    }

    public class AlwaysValidLdapSslCertificateValidator : ILdapSslCertificateValidator
    {
        public AlwaysValidLdapSslCertificateValidator(ILogger<AlwaysValidLdapSslCertificateValidator> logger)
        {

            _log = logger;

        }

        private readonly ILogger _log;

        public bool ValidateCertificate(
           object sender,
           X509Certificate certificate,
           X509Chain chain,
           SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
