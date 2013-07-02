using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using Relax.Security;
using Relax.Services;
using Relax.Services.Interfaces;

namespace Relax
{
    public class RelaxWindowsService : ServiceBase
    {
        private ServiceHost _serviceHost = null;
        private static EventLog _eventLog;

        public RelaxWindowsService()
        {
            _eventLog = new EventLog();

            ServiceName = "Relax for TFS";
            _eventLog.Source = ServiceName;
            _eventLog.Log = "Application";

            if (!EventLog.SourceExists(_eventLog.Source))
                EventLog.CreateEventSource(_eventLog.Source, _eventLog.Log);
        }

        public static void Main(string[] args)
        {
            var service = new RelaxWindowsService();
            try
            {

                if (Environment.UserInteractive)
                {
                    service.OnStart(args);
                    Console.WriteLine("Press any key to stop program");
                    Console.Read();
                    service.OnStop();
                }
                else
                {
                    ServiceBase.Run(new RelaxWindowsService());
                }
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            X509Certificate2 certificate = Certificate.Instantiate().LoadCertificate();
            
            var addressHttps = String.Format("https://{0}:{1}", Dns.GetHostEntry("").HostName, Config.Port);
            var wsHttpBinding = new WebHttpBinding
                                    {
                                        Name = "StreamedRequestWebBinding",
                                        BypassProxyOnLocal = true,
                                        UseDefaultWebProxy = true,
                                        HostNameComparisonMode = HostNameComparisonMode.WeakWildcard,
                                        SendTimeout = new TimeSpan(10, 15, 00),
                                        OpenTimeout = new TimeSpan(10, 15, 00),
                                        ReceiveTimeout = new TimeSpan(10, 15, 00),
                                        MaxReceivedMessageSize = 2147483647,
                                        MaxBufferSize = 2147483647,
                                        MaxBufferPoolSize = 2147483647,
                                        TransferMode = TransferMode.StreamedRequest,
                                        //ReaderQuotas =
                                        //    {
                                        //        MaxArrayLength = 2147483647,
                                        //        MaxStringContentLength = 2147483647
                                        //    },
                                        Security =
                                            {
                                                Mode = WebHttpSecurityMode.Transport,
                                                Transport =
                                                    {
                                                        ClientCredentialType = HttpClientCredentialType.None,
                                                    }
                                            }
                                    };
            _serviceHost = new ServiceHost(typeof(RelaxService), new Uri(addressHttps));
            _serviceHost.AddServiceEndpoint(typeof(IRelaxService), wsHttpBinding, string.Empty);
            _serviceHost.Credentials.ServiceCertificate.Certificate = new X509Certificate2(certificate);
            _serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior
                                                       {
                                                           HttpGetEnabled = false,
                                                           HttpsGetEnabled = true,
                                                           HttpsGetUrl =
                                                               new Uri(
                                                               _serviceHost.Description.Endpoints[0].ListenUri.
                                                                   AbsoluteUri + "mex"),
                                                       });
            _serviceHost.Description.Endpoints[0].Behaviors.Add(new WebHttpBehavior
                                                                    {
                                                                        AutomaticFormatSelectionEnabled = true
                                                                    });
            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            _serviceHost.Open();
            HttpModifier.Instantiate().AddCert(certificate.Thumbprint);
            _eventLog.WriteEntry("Service started.", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            Firewall.Instantiate().CloseFirewall();

            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
            HttpModifier.Instantiate().DeleteCert();
            _eventLog.WriteEntry("Service stopped.", EventLogEntryType.Information);
        }

        
    }
}