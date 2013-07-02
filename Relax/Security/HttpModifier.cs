using System;

namespace Relax.Security
{
    public class HttpModifier
    {
        public static HttpModifier Instantiate()
        {
            return new HttpModifier();
        }

        public void AddUrl()
        {
            var parameter = String.Format(@"http add urlacl url=https://+:{0}/ user=\'Network Service'", Config.Port);
            RunCommand("netsh", parameter);
        }

        public void DeleteUrl()
        {
            var parameter = String.Format(@"http delete urlacl url=https://+:{0}/", Config.Port);
            RunCommand("netsh", parameter);
        }

        public void AddCert(string thumbprint)
        {
            var parameter = String.Format(@"http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}", Config.Port, thumbprint, Guid.NewGuid().ToString().ToUpper());
            RunCommand("netsh", parameter);
        }

        public void DeleteCert()
        {
            var parameter = String.Format(@"http delete sslcert ipport=0.0.0.0:{0}", Config.Port);
            RunCommand("netsh", parameter);
        }

        public void CreateCertificate()
        {
            var parameter = String.Format(@"-r -pe -n ""CN={0}"" -b {1} -e {2} -eku 1.3.6.1.5.5.7.3.1 -ss my -sr localMachine -sky exchange -sp ""Microsoft RSA SChannel Cryptographic Provider"" -sy 12", 
                Config.CertificateName, 
                DateTime.Now.ToString("MM/dd/yyyy"), 
                DateTime.Now.AddYears(1000).ToString("MM/dd/yyyy"));
            RunCommand(@"makecert.exe", parameter);
        }

        private void RunCommand(String cmd, String parameter)
        {
            var psi = new System.Diagnostics.ProcessStartInfo(cmd, parameter)
            {
                Verb = "runas",
                RedirectStandardOutput = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            System.Diagnostics.Process.Start(psi);
        }
    }
}
