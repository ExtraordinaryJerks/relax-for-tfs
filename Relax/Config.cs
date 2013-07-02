using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Relax
{
    public static class Config
    {
        public static int Port
        {
            get { return 1800; }
        }

        public static string CertificateName
        {
            get { return "RelaxForTFS"; }
        }

        public static string GetLocalHostName()
        {
            var result = Dns.GetHostEntry("").HostName;
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            if (!String.IsNullOrWhiteSpace(ipProperties.DomainName))
            {
                result = string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
            }
            return result;
        }
    }
}
