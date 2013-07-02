using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using Relax.Model;
using Relax.Services.Interfaces;

namespace Relax.Services
{
    public class NotificationService : INotificationService
    {
        public void Notify(RelaxNotificationMessage relaxNotificationMessage)
        {
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.UploadDataAsync(new Uri(String.Format("{0}/{1}/{2}", ConfigurationManager.AppSettings["RelaxForTfs.WebServiceApi.BaseUrl"],"api", "messaging")), 
                                   "POST", 
                                   System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(relaxNotificationMessage)));

        }
    }
}