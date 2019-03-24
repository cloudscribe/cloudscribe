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
            // I guess the idea would be to have the certificate exist in the local store of the machine
            // and validate the passed one vs that one.
            // but how to do that would be platform specific
            // here we are just trusting the certificate and returning true

            // for ideas how to implement this for your scenario see https://stackoverflow.com/questions/7331666/c-sharp-how-can-i-validate-a-root-ca-cert-certificate-x509-chain

            //_log.LogWarning("returning true for ldap ssl certificate validation");

            return true;
        }
    }
}
