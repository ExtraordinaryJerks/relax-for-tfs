using System.Security.Cryptography.X509Certificates;
using Relax.Security;

namespace Relax
{
    public class Certificate
    {
        public static Certificate Instantiate()
        {
            return new Certificate();
        }

        public X509Certificate2 LoadCertificate()
        {
            var cert = RetrieveCertificate();
            if (cert == null)
            {
                HttpModifier.Instantiate().CreateCertificate();
                System.Threading.Thread.Sleep(1000); //give time for the certificate store to close.
                cert = RetrieveCertificate();
            }
            return cert;
        }

        private X509Certificate2 RetrieveCertificate()
        {
            X509Certificate2 result = null;
            var store = new X509Store("MY", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var collection = (X509Certificate2Collection)store.Certificates;
            var fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByIssuerDistinguishedName, string.Format("CN={0}", Config.CertificateName), false);
            if (fcollection.Count > 0)
            {
                result = fcollection[0];
            }
            store.Close();
            return result;
        }
    }
}