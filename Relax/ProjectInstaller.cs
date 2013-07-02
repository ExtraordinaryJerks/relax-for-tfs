using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using Relax.Security;

namespace Relax
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller
                          {
                              Account = ServiceAccount.LocalSystem
                          };

            service = new ServiceInstaller
                          {
                              ServiceName = "Relax for TFS",
                              StartType = ServiceStartMode.Automatic,
                              DisplayName = "Relax for TFS",
                              Description = "A restful WCF service which allows access to TFS."
                          };

            Installers.Add(process);
            Installers.Add(service);
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            HttpModifier.Instantiate().AddUrl();
        }

        protected override void OnAfterInstall(IDictionary saveState)
        {
            Firewall.Instantiate().OpenFirewall();
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            Firewall.Instantiate().CloseFirewall();
            HttpModifier.Instantiate().DeleteUrl();
        }

        //public string GetContextParameter(string key)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        returnValue = this.Context.Parameters[key].ToString();
        //    }
        //    catch
        //    {
        //        returnValue = "";
        //    }
        //    return returnValue;
        //}

        // Override the 'OnBeforeInstall' method.
        //protected override void OnBeforeInstall(IDictionary savedState)
        //{
        //    base.OnBeforeInstall(savedState);

        //    string username = GetContextParameter("username").Trim();
        //    string password = GetContextParameter("password").Trim();
        //    string domain = GetContextParameter("domain").Trim();

        //    if (username != "")
        //        process.Username = !string.IsNullOrEmpty(domain) ? domain + "\\" + username : username;
        //    if (password != "")
        //        process.Password = password;
        //}
    }
}