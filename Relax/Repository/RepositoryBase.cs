using System;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Relax.Repository.Interfaces;
using Relax.Security;

namespace Relax.Repository
{
    public abstract class RepositoryBase
    {
        protected const String TempDirectory = "RelaxTFS";

        protected TfsTeamProjectCollection _tfs;
        protected TfsConfigurationServer _configuration;

        protected RepositoryBase(String username, String password, string domain, string projectCollection, String url)
        {
            string fullUrl = url;
            bool collectionExists = !String.IsNullOrEmpty(projectCollection);


            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(username, "Username is null or empty!");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(password, "Password is null or empty!");
            if (collectionExists)
                fullUrl = url.LastIndexOf('/') == url.Length - 1
                              ? String.Concat(url, projectCollection)
                              : String.Concat(url, "/", projectCollection);
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException(url, "TFSServerUrl is null or empty!");

            var credentials = new NetworkCredential(username, password, domain);
            
            _configuration = new TfsConfigurationServer(new Uri(url), credentials); 
            _configuration.EnsureAuthenticated();

            if (collectionExists)
            {
                _tfs = new TfsTeamProjectCollection(new Uri(fullUrl), credentials);
                _tfs.EnsureAuthenticated();
            }
        }
    }
}